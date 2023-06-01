using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace Datos
{
    public class ArchivoVenta : Conexion
    {
        public ArchivoVenta(string connectionString) : base(connectionString)
        {

        }


        public int ObtenerCorrelativo()
        {
            int idcorrelativo = 0;

            using (SqlConnection oconexion = new SqlConnection(Conexion.ConnectionString))
            {

                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select count(*) + 1 from VENTA");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;

                    Open();
                    oconexion.Open();

                    idcorrelativo = Convert.ToInt32(cmd.ExecuteScalar());

                }
                catch 
                {
                    idcorrelativo = 0;
                }
            }
            Closed();
            return idcorrelativo;
        }


        public bool Registrar(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {
            bool Respuesta = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarVenta", oconexion);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.oUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("NumeroDocumento", obj.NumeroDocumento);
                    cmd.Parameters.AddWithValue("DocumentoCliente", obj.DocumentoCliente);
                    cmd.Parameters.AddWithValue("NombreCliente", obj.NombreCliente);
                    cmd.Parameters.AddWithValue("MontoTotal", obj.MontoTotal);
                    cmd.Parameters.AddWithValue("NombreMascota", obj.NombreMacota);
                    cmd.Parameters.AddWithValue("TipoMascota", obj.TipoMascota);
                    cmd.Parameters.AddWithValue("DetalleVenta", DetalleVenta);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    Open();
                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    Respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
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

        public Venta ObtenerVenta(string numero)
        {

            Venta venta = new Venta();

            using (SqlConnection conexion = new SqlConnection(Conexion.ConnectionString))
            {
                try
                {
                    Open();
                    conexion.Open();
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("select v.IdVenta, u.NombreUsuario, v.DocumentoCliente, v.NombreCliente, v.NombreMacota, v.TipoMascota, v.NumeroDocumento, v.MontoTotal, ");
                    query.AppendLine("convert(char(10), v.FechaRegistro,103)[FechaRegistro]");
                    query.AppendLine("from VENTA v inner join USUARIO u on u.IdUsuario = v.IdUsuario where v.NumeroDocumento = @numero");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@numero", numero);
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {

                        while (dr.Read())
                        {
                            venta = new Venta()
                            {
                                IdVenta = int.Parse(dr["IdVenta"].ToString()),
                                oUsuario = new Usuarios() { NombreUsuario = dr["NombreUsuario"].ToString() },
                                DocumentoCliente = dr["DocumentoCliente"].ToString(),
                                NombreCliente = dr["NombreCliente"].ToString(),
                                NombreMacota = dr["NombreMacota"].ToString(),
                                TipoMascota = dr["TipoMascota"].ToString(),
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"].ToString()),
                                FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"].ToString())
                            };
                        }
                    }

                }
                catch
                {
                    venta = new Venta();
                }

            }
            Closed();
            return venta;

        }

        public List<DetalleVenta> ObtenerDetalleVenta(int IdVenta)
        {
            List<DetalleVenta> oLista = new List<DetalleVenta>();

            using (SqlConnection conexion = new SqlConnection(Conexion.ConnectionString))
            {
                try
                {
                    Open();
                    conexion.Open();
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select s.NombreServicio, dv.PrecioVenta from DETALLE_VENTA dv ");
                    query.AppendLine("inner join SERVICIO s on s.IdServicio = dv.IdServicio where dv.IdVenta = @IdVenta");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@IdVenta", IdVenta);
                    cmd.CommandType = System.Data.CommandType.Text;


                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new DetalleVenta()
                            {
                                oServicio    = new Servicio() { NombreServicio = dr["NombreServicio"].ToString() },
                                PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"].ToString()),
                            });
                        }
                    }

                }
                catch
                {
                    oLista = new List<DetalleVenta>();
                }
            }
            Closed();
            return oLista;
        }



    }
}
