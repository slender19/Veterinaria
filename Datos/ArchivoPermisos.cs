using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class ArchivoPermisos : Conexion
    {
        //constructor de esta clase teniendo encuenta la herencia con la clase "Conexion"//
        public ArchivoPermisos(string connectionString) : base(connectionString)
        {

        }


        //metodo para obtner todos los permisos almacenados en la base de datos haciendo uso de sentencias sql//
        public List<Permisos> Listar(int idusuario)
        {

           
            List<Permisos> listapermisos = new List<Permisos>();
            var comando = _Conexion.CreateCommand();

            //sentencia sql utilizando inner join para juntar tablas para así obtner resultados más precisos dependiedo al parametro que se le manda("@idusuario")
            comando.CommandText = "select p.Idrol, p.NombreMenu from PERMISOS p \r\ninner join ROL r on r.IdRol = p.Idrol\r\ninner join USUARIO u on u.Idrol = r.IdRol \r\nwhere u.IdUsuario = @idusuario";

            //al parametro de la sentencia sql se le atribuye el dato que almacena idusuario//
            comando.Parameters.AddWithValue("@idusuario", idusuario);

            Open();
            SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                listapermisos.Add(Mapper(lector));
            }
            Closed();
            return listapermisos;
        }


        //mappeador para atribuir ciertos parametros de la entidad permisos con los de la base de datos//
        private Permisos Mapper(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows) return null;
            Permisos permisos = new Permisos();

            permisos.oRol = new Rol { IdRol = dataReader.GetInt32(0) };
            permisos.NombreMenu = dataReader.GetString(1);

            return permisos;
        }
    }
}
