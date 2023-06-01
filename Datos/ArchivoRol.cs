using Entidades;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class ArchivoRol:Conexion
    {

        public ArchivoRol(string connectionString) : base(connectionString)
        {

        }

        //metodo para obtner todos los permisos almacenados en la base de datos haciendo uso de sentencias sql//
        public List<Rol> Listar()
        {

            //lista de rol para almacenarlos despues de obtenerlos con la sentencia sql//
            List<Rol> listarol = new List<Rol>();
            var comando = _Conexion.CreateCommand();
            comando.CommandText = "select Idrol, Descripcion from ROL";
            Open();
            SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                listarol.Add(Mapper(lector));
            }
            Closed();
            return listarol;
        }


        //mappeador para atribuir ciertos parametros de la entidad rol con los de la base de datos//
        private Rol Mapper(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows) return null;
            Rol rol = new Rol();

            rol.IdRol = dataReader.GetInt32(0) ;
            rol.DescripcionRol = dataReader.GetString(1);

            return rol;
        }

    }
}
