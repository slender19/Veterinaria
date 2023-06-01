using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class ArchivoMascota: Conexion
    {


        public ArchivoMascota(string connectionString) : base(connectionString)
        {

        }

        public List<Mascota> GetAll()
        {
            //lista de Mascotas para almacenarlos despues de obtenerlos con la sentencia sql//
            List<Mascota> listaMascota = new List<Mascota>();
            var comando = _Conexion.CreateCommand();
            comando.CommandText = "select IdMascota, NombreMascota, Raza, Tipo, c.Documento,c.NombreCliente,   m.Estado  from MASCOTA m inner join  CLIENTES c on  m.IdCliente = c.IdCliente";
            Open();
            SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                listaMascota.Add(Mapper(lector));
            }
            Closed();
            return listaMascota;
        }

        //mappeador para atribuir todos los parametros de la entidad Mascota con los de la base de datos//
        private Mascota Mapper(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows) return null;
            Mascota Mascota = new Mascota();
            Mascota.IdMascota = dataReader.GetInt32(0);
            Mascota.NombreMascota = dataReader.GetString(1);
            Mascota.Raza = dataReader.GetString(2);
            Mascota.Tipo = dataReader.GetString(3);
            Mascota.oCliente = new Cliente { Documento = dataReader.GetString(4), NombreCliente = dataReader.GetString(5)};
            Mascota.Estado = dataReader.GetBoolean(6);
            ;

            return Mascota;
        }


        //metodo para guadar los Mascota en la capa de datos
        public int Registrar(Mascota Mascota, out string Mensaje)
        {
            int Resultado = 0;
            Mensaje = string.Empty;


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.ConnectionString))
                {
                    SqlCommand comando = new SqlCommand("SP_RegistrarMascota", oconexion);
                    comando.Parameters.AddWithValue("@IdCliente", Mascota.oCliente.IdCliente);
                    comando.Parameters.AddWithValue("@NombreMascota", Mascota.NombreMascota);
                    comando.Parameters.AddWithValue("@Raza", Mascota.Raza);
                    comando.Parameters.AddWithValue("@Tipo", Mascota.Tipo);
                    comando.Parameters.AddWithValue("@Estado", Mascota.Estado);
                    comando.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    comando.Parameters.Add("@Mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output;

                    comando.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();


                    comando.ExecuteNonQuery();

                    Resultado = Convert.ToInt32(comando.Parameters["@Resultado"].Value);
                    Mensaje = (comando.Parameters["@Mensaje"].Value.ToString());

                    oconexion.Close();
                }



            }
            catch (Exception ex)
            {
                Resultado = 0;
                Mensaje = ex.Message;
            }




            return Resultado;
        }


        //metodo para editar los Mascota//
        public bool Editar(Mascota Mascota, out string Mensaje)
        {
            bool Respuesta = false;
            Mensaje = string.Empty;


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.ConnectionString))
                {
                    SqlCommand comando = new SqlCommand("SP_ModificarMascota", oconexion);
                    comando.Parameters.AddWithValue("@IdMascota", Mascota.IdMascota);
                    comando.Parameters.AddWithValue("@IdCliente", Mascota.oCliente.IdCliente);
                    comando.Parameters.AddWithValue("@NombreMascota", Mascota.NombreMascota);
                    comando.Parameters.AddWithValue("@Raza", Mascota.Raza);
                    comando.Parameters.AddWithValue("@Tipo", Mascota.Tipo);
                    comando.Parameters.AddWithValue("@Estado", Mascota.Estado);
                    comando.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    comando.Parameters.Add("@Mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output;

                    comando.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();

                    comando.ExecuteNonQuery();

                    Respuesta = Convert.ToBoolean(comando.Parameters["@Resultado"].Value);
                    Mensaje = (comando.Parameters["@Mensaje"].Value.ToString());

                    oconexion.Close();
                }



            }
            catch (Exception ex)
            {
                Respuesta = false;
                Mensaje = ex.Message;
            }



            Closed();
            return Respuesta;
        }

        //metodo para eliminar Mascota//
        public bool Eliminar(Mascota Mascota, out string Mensaje)
        {
            bool Respuesta = false;
            Mensaje = string.Empty;


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.ConnectionString))
                {
                    SqlCommand comando = new SqlCommand("delete from MASCOTA where IdMascota = @IdMascota ", oconexion);
                    comando.Parameters.AddWithValue("@IdMascota", Mascota.IdMascota);
                    comando.CommandType = CommandType.Text;

                    oconexion.Open();

                    Respuesta = comando.ExecuteNonQuery() > 0 ? true : false;

                    oconexion.Close();
                }



            }
            catch (Exception ex)
            {
                Respuesta = false;
                Mensaje = ex.Message;
            }




            return Respuesta;
        }
    }
}
