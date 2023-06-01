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
    public class ArchivoReporte : Conexion
    {
        public ArchivoReporte(string connectionString) : base(connectionString)
        {
        }

        public List<ReporteVentas> ReporteVenta(string fechainicio, string fechafin)
        {
            List<ReporteVentas> lista = new List<ReporteVentas>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.ConnectionString))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    SqlCommand cmd = new SqlCommand("SP_ReporteVentas", oconexion);
                    cmd.Parameters.AddWithValue("@FechaInicio", fechainicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechafin);
                    cmd.CommandType = CommandType.StoredProcedure;

                    Open();
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ReporteVentas()
                            {
                                FechaRegistro = dr["FechaRegistro"].ToString(),
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                MontoTotal = dr["MontoTotal"].ToString(),
                                NombreUsuario = dr["NombreUsuario"].ToString(),
                                DocumentoCliente = dr["DocumentoCliente"].ToString(),
                                NombreCliente = dr["NombreCliente"].ToString(),
                                NombreMascota = dr["NombreMacota"].ToString(),
                                TipoMascota = dr["TipoMascota"].ToString(),
                                CodigoServicio = dr["CodigoServicio"].ToString(),
                                NombreServicio = dr["NombreServicio"].ToString(),
                                PrecioVenta = dr["PrecioVenta"].ToString()
                            });
                        }
                    }
                    Closed();
                }
                catch 
                {
                    lista = new List<ReporteVentas>();
                }
            }
            
            return lista;

        }

    }
}
