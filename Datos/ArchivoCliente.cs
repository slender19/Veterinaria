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
    public class ArchivoCliente : Conexion
    {
        public ArchivoCliente(string connectionString) : base(connectionString)
        {

        }



        public List<Cliente> GetAll()
        {
            //lista de Clientes para almacenarlos despues de obtenerlos con la sentencia sql//
            List<Cliente> listaCliente = new List<Cliente>();
            var comando = _Conexion.CreateCommand();
            comando.CommandText = "select IdCliente,  Documento, NombreCliente, Telefono, Estado  from CLIENTES ";
            Open();
            SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                listaCliente.Add(Mapper(lector));
            }
            Closed();
            return listaCliente;
        }

        //mappeador para atribuir todos los parametros de la entidad Cliente con los de la base de datos//
        private Cliente Mapper(SqlDataReader dataReader)
        {
            if (!dataReader.HasRows) return null;
            Cliente Cliente = new Cliente();
            Cliente.IdCliente = dataReader.GetInt32(0);
            Cliente.Documento = dataReader.GetString(1);
            Cliente.NombreCliente = dataReader.GetString(2);
            Cliente.Telefono = dataReader.GetString(3);
            Cliente.Estado = dataReader.GetBoolean(4);
            ;

            return Cliente;
        }


        //metodo para guadar los Cliente en la capa de datos
        public int Registrar(Cliente Cliente, out string Mensaje)
        {
            int Resultado = 0;
            Mensaje = string.Empty;


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.ConnectionString))
                {
                    SqlCommand comando = new SqlCommand("SP_RegistrarCliente", oconexion);
                    comando.Parameters.AddWithValue("@Documento", Cliente.Documento);
                    comando.Parameters.AddWithValue("@NombreCliente", Cliente.NombreCliente);
                    comando.Parameters.AddWithValue("@Telefono", Cliente.Telefono);
                    comando.Parameters.AddWithValue("@Estado", Cliente.Estado);
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


        //metodo para editar los Cliente//
        public bool Editar(Cliente Cliente, out string Mensaje)
        {
            bool Respuesta = false;
            Mensaje = string.Empty;


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.ConnectionString))
                {
                    SqlCommand comando = new SqlCommand("SP_ModificarCliente", oconexion);
                    comando.Parameters.AddWithValue("@IdCliente", Cliente.IdCliente);
                    comando.Parameters.AddWithValue("@Documento", Cliente.Documento);
                    comando.Parameters.AddWithValue("@NombreCliente", Cliente.NombreCliente);
                    comando.Parameters.AddWithValue("@Telefono", Cliente.Telefono);
                    comando.Parameters.AddWithValue("@Estado", Cliente.Estado);
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

        //metodo para eliminar Cliente//
  
        public bool Eliminar(Cliente Cliente, out string Mensaje)
        {
            bool Respuesta = false;
            Mensaje = string.Empty;


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.ConnectionString))
                {
                    SqlCommand comando = new SqlCommand("SP_EliminarCliente", oconexion);
                    comando.Parameters.AddWithValue("@IdCliente", Cliente.IdCliente);
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
