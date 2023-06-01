using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class ArchivoNegocio : Conexion
    {
        public ArchivoNegocio(string connectionString) : base(connectionString)
        {

        }


        public Negocio obtenerDatos()
        {
            Negocio negocio = new Negocio();

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.ConnectionString))
                {
                    conexion.Open();
                    string query = "select IdNegocio, Nombre, RUC, Direccion from NEGOCIO where IdNegocio = 1";
                    SqlCommand cmd = new SqlCommand(query,conexion);
                    cmd.CommandType = CommandType.Text;

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            negocio = new Negocio() { Idnegocio = int.Parse(reader["IdNegocio"].ToString()),
                                Nombre = reader["Nombre"].ToString(),
                                RUC = reader["RUC"].ToString(),
                                Direccion = reader["Direccion"].ToString(),
                            };
                        }
                    }
                    
                }
                Closed();
            }
            catch
            {
                negocio = new Negocio();
            }

            return negocio;
        }



        public bool Guardardatos(Negocio negocio, out string mensaje)
        {
            mensaje = string.Empty;
            bool respuesta = true;
            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.ConnectionString))
                {
                    conexion.Open();
                    
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update NEGOCIO set Nombre = @Nombre,");
                    query.AppendLine("Ruc = @ruc,");
                    query.AppendLine("Direccion = @Direccion");
                    query.AppendLine("where IdNegocio = 1;");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@Nombre", negocio.Nombre);
                    cmd.Parameters.AddWithValue("@ruc", negocio.RUC);
                    cmd.Parameters.AddWithValue("@Direccion", negocio.Direccion);
                    cmd.CommandType = CommandType.Text;

                   if(cmd.ExecuteNonQuery() < 1)
                   {
                        mensaje = "No se pudo guardar los datos";
                        respuesta = false;
                   }
                    
                }
                Closed();
            }
            catch(Exception ex)
            {
                respuesta=false;
                mensaje = ex.Message;

            }

            return respuesta;

        }


        public byte[] ObtenerLogo(out bool obtenido)
        {
            obtenido = true;
            byte[] LogoBytes = new byte[0];

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.ConnectionString))
                {
                    conexion.Open();

                    string query = "select Logo from NEGOCIO where IdNegocio = 1";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LogoBytes = (byte[])reader["Logo"];
                           
                        }
                    }
                    
                }
                Closed();
            }
            catch 
            {
                obtenido = false;
                LogoBytes = new byte[0];

            }

            return LogoBytes;
        }


        public bool ActualizarLogo(byte[] imagen, out string mensaje)
        {
            mensaje = string.Empty;
            bool respuesta = true;
            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.ConnectionString))
                {
                    conexion.Open();

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update NEGOCIO set Logo = @imagen");
                    query.AppendLine("where IdNegocio = 1;");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@imagen", imagen);
                   
                    cmd.CommandType = CommandType.Text;

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        mensaje = "No se pudo actualizar el logo";
                        respuesta = false;
                    }
                   
                }
                Closed();
            }
            catch (Exception ex)
            {
                respuesta = false;
                mensaje = ex.Message;

            }

            return respuesta;
        }

    }
}
