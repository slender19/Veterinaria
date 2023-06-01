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
    public class ArchivoServicio : Conexion
    {
        public ArchivoServicio(string connectionString) : base(connectionString)
        {

        }



        public List<Servicio> GetAll()
        {
            //lista de servicios para almacenarlos despues de obtenerlos con la sentencia sql//
            List<Servicio> listaServicio = new List<Servicio>();
            var comando = _Conexion.CreateCommand();
            comando.CommandText = "select IdServicio, Codigo, NombreServicio, PrecioVenta, Estado from SERVICIO";
            Open();
            SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                listaServicio.Add(Mapper(lector));
            }
            Closed();
            return listaServicio;
        }

        //mappeador para atribuir todos los parametros de la entidad Servicio con los de la base de datos//
        private Servicio Mapper(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows) return null;
            Servicio Servicio = new Servicio();
            Servicio.IdServicio = dataReader.GetInt32(0);
            Servicio.Codigo = dataReader.GetString(1);
            Servicio.NombreServicio = dataReader.GetString(2);
            Servicio.PrecioVenta = dataReader.GetDecimal(3);
            Servicio.Estado = dataReader.GetBoolean(4);
        ;

            return Servicio;
        }


        //metodo para guadar los Servicio en la capa de datos
        public int Registrar(Servicio servicio, out string Mensaje)
        {
            int Resultado = 0;
            Mensaje = string.Empty;


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.ConnectionString))
                {
                    SqlCommand comando = new SqlCommand("SP_RegistrarServicio", oconexion);
                    comando.Parameters.AddWithValue("@Codigo", servicio.Codigo);
                    comando.Parameters.AddWithValue("@NombreServicio", servicio.NombreServicio);
                    comando.Parameters.AddWithValue("@PrecioVenta", servicio.PrecioVenta);
                    comando.Parameters.AddWithValue("@Estado", servicio.Estado);
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


        //metodo para editar los servicio//
        public bool Editar(Servicio servicio, out string Mensaje)
        {
            bool Respuesta = false;
            Mensaje = string.Empty;


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.ConnectionString))
                {
                    SqlCommand comando = new SqlCommand("SP_ModificarServicio", oconexion);
                    comando.Parameters.AddWithValue("@IdServicio", servicio.IdServicio);
                    comando.Parameters.AddWithValue("@Codigo", servicio.Codigo);
                    comando.Parameters.AddWithValue("@NombreServicio", servicio.NombreServicio);
                    comando.Parameters.AddWithValue("@PrecioVenta", servicio.PrecioVenta);
                    comando.Parameters.AddWithValue("@Estado", servicio.Estado);
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

        //metodo para eliminar Servicio//
        public bool Eliminar(Servicio servicio, out string Mensaje)
        {
            bool Respuesta = false;
            Mensaje = string.Empty;


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.ConnectionString))
                {
                    SqlCommand comando = new SqlCommand("SP_EliminarServicio", oconexion);
                    comando.Parameters.AddWithValue("@IdServicio", servicio.IdServicio);
                    comando.Parameters.Add("@Respuesta", SqlDbType.Int).Direction = ParameterDirection.Output;
                    comando.Parameters.Add("@Mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output;

                    comando.CommandType = CommandType.StoredProcedure;


                    oconexion.Open();

                    comando.ExecuteNonQuery();

                    Respuesta = Convert.ToBoolean(comando.Parameters["@Respuesta"].Value);
                    Mensaje = (comando.Parameters["@Mensaje"].Value.ToString());


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
