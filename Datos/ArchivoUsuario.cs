using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Entidades;

namespace Datos
{
    public class ArchivoUsuario : Conexion
    {
        //constructor de esta clase teniendo encuenta la herencia con la clase "Conexion"//
        public ArchivoUsuario(string connectionString): base (connectionString) 
        {
            
        }
       
        //metodo para obtner todos los usuarios almacenados en la base de datos haciendo uso de sentencias sql//
        public List<Usuarios> GetAll()
        {
            //lista de ususarios para almacenarlos despues de obtenerlos con la sentencia sql//
            List<Usuarios> listaUsuarios = new List<Usuarios>();
            var comando = _Conexion.CreateCommand();
            comando.CommandText = "select u.IdUsuario,u.Documento,u.NombreUsuario,u.Correo,u.Clave,u.Estado, r.IdRol,r.Descripcion from usuario u inner join ROL r on r.IdRol = u.Idrol";
            Open();
            SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                listaUsuarios.Add(Mapper(lector));
            }
            Closed();
            return listaUsuarios;
        }

        //mappeador para atribuir todos los parametros de la entidad usuarios con los de la base de datos//
        private Usuarios Mapper(SqlDataReader dataReader)
        {
            if(!dataReader.HasRows) return null;
            Usuarios usuarios = new Usuarios();
            usuarios.IdUsuario = dataReader.GetInt32(0);
            usuarios.Documento = dataReader.GetString(1);
            usuarios.NombreUsuario = dataReader.GetString(2);
            usuarios.Correo = dataReader.GetString(3);
            usuarios.Clave = dataReader.GetString(4);
            usuarios.Estado = dataReader.GetBoolean(5);
            usuarios.oRol = new Rol() { IdRol = dataReader.GetInt32(6), DescripcionRol = dataReader.GetString(7) };
           
            return usuarios;
        }


        //metodo para guadar los usuarios en la capa de datos
        public int Registrar(Usuarios usuario, out string Mensaje)
        {
            int idusuariogenerado = 0;
            Mensaje = string.Empty;


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.ConnectionString))
                {
                    SqlCommand comando = new SqlCommand("SP_REGISTRARUSUARIO", oconexion);
                    comando.Parameters.AddWithValue("@Documento", usuario.Documento);
                    comando.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    comando.Parameters.AddWithValue("@Correo", usuario.Correo);
                    comando.Parameters.AddWithValue("@Clave ", usuario.Clave);
                    comando.Parameters.AddWithValue("@IdRol", usuario.oRol.IdRol);
                    comando.Parameters.AddWithValue("@Estado", usuario.Estado);
                    comando.Parameters.Add("@IdUsuarioResultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    comando.Parameters.Add("@Mensaje", SqlDbType.VarChar,300).Direction = ParameterDirection.Output;

                    comando.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    

                    comando.ExecuteNonQuery();

                    idusuariogenerado = Convert.ToInt32(comando.Parameters["@IdUsuarioResultado"].Value);
                    Mensaje = (comando.Parameters["@Mensaje"].Value.ToString());

                    oconexion.Close();
                }
                    


            }catch(Exception ex)
            {
                idusuariogenerado = 0;
                Mensaje = ex.Message;
            }



           
            return idusuariogenerado;
        }


        //metodo para editar los usuario//
        public bool Editar(Usuarios usuario, out string Mensaje)
        {
            bool Respuesta = false;
            Mensaje = string.Empty;


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.ConnectionString))
                {
                    SqlCommand comando = new SqlCommand("SP_EDITARUSUARIO", oconexion);
                    comando.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                    comando.Parameters.AddWithValue("@Documento", usuario.Documento);
                    comando.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    comando.Parameters.AddWithValue("@Correo", usuario.Correo);
                    comando.Parameters.AddWithValue("@Clave ", usuario.Clave);
                    comando.Parameters.AddWithValue("@IdRol", usuario.oRol.IdRol);
                    comando.Parameters.AddWithValue("@Estado", usuario.Estado);
                    comando.Parameters.Add("@Respuesta", SqlDbType.Int).Direction = ParameterDirection.Output;
                    comando.Parameters.Add("@Mensaje", SqlDbType.VarChar,300).Direction = ParameterDirection.Output;

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



            Closed();
            return Respuesta;
        }

        //metodo para eliminar usuarios//
        public bool Eliminar(Usuarios usuario, out string Mensaje)
        {
            bool Respuesta = false;
            Mensaje = string.Empty;


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.ConnectionString))
                {
                    SqlCommand comando = new SqlCommand("SP_ELIMINARUSUARIO", oconexion);
                    comando.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
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










        //public List<Usuarios> listar() 
        //{ 


        //    using (SqlConnection connection = new SqlConnection(Conexion.cadena))
        //    {

        //            string query = "select IdUsuario,Documento,NombreUsuario,Correo,Clave,Estado from usuario";
        //            SqlCommand cmd = new SqlCommand(query, connection);
        //            cmd.CommandType= CommandType.Text;
        //            connection.Open();
        //            using (SqlDataReader dr = cmd.ExecuteReader())
        //            {
        //                while (dr.Read())
        //                {
        //                    listaUsuarios.Add(new Usuarios()
        //                    {
        //                        IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
        //                        Documento= dr["Documento"].ToString(),
        //                        NombreUsuario = dr["NombreUsuario"].ToString(),
        //                        Correo = dr["Correo"].ToString(),
        //                        Clave = dr["Clave"].ToString(),
        //                        Estado = Convert.ToBoolean(dr["Estado"])
        //                    });
        //                }
        //            }
        //            connection.Close();


        //    }

        //    return listaUsuarios;

        //}



    }
}
