using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class ReporteVentas
    {

        public string FechaRegistro { get; set; }
        public string NumeroDocumento { get; set; }
        public string MontoTotal { get; set; }
        public string NombreUsuario { get; set; }
        public string DocumentoCliente { get; set; }
        public string NombreCliente { get; set; }
        public string NombreMascota { get; set; }
        public string TipoMascota { get; set; }
        public string CodigoServicio { get; set; }
        public string NombreServicio { get; set; }
        public string PrecioVenta { get; set; }
    }
}
