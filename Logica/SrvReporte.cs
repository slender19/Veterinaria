using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class SrvReporte
    {

        ArchivoReporte archivoReporte;
        public SrvReporte(string conexion)
        {
            archivoReporte = new ArchivoReporte(conexion);
        }

        public List<ReporteVentas> ReporteVenta(string fechainicio, string fechafin)
        {
            return archivoReporte.ReporteVenta(fechainicio, fechafin);
        }
    }
}
