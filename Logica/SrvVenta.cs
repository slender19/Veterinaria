using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class SrvVenta
    {

        ArchivoVenta archivoVenta;
        public SrvVenta(string conexion)
        {
            archivoVenta = new ArchivoVenta(conexion);
        }

        public int ObtenerCorrelativo()
        {
            return archivoVenta.ObtenerCorrelativo();
        }

        public bool Registrar(Venta venta, DataTable DetalleVenta, out string Mensaje)
        {
            return archivoVenta.Registrar(venta, DetalleVenta, out Mensaje);
        }


        public Venta obtenerVenta(string numero)
        {
            Venta oventa = archivoVenta.ObtenerVenta(numero);
            if(oventa.IdVenta != 0) 
            {
                List<DetalleVenta> oDetalleVenta = archivoVenta.ObtenerDetalleVenta(oventa.IdVenta);
                oventa.oDetalleVentas = oDetalleVenta;
            }

            return oventa;
        }
    }
}
