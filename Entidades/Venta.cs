using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public Usuarios oUsuario { get; set; }
        public string NumeroDocumento { get; set; }
        public string DocumentoCliente { get; set; }
        public string NombreCliente { get; set; }
        public string NombreMacota { get; set; } 
        public string TipoMascota { get;set; }
        public decimal MontoTotal { get; set; }
        public List<DetalleVenta> oDetalleVentas { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
