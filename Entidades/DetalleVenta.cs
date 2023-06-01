using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class DetalleVenta
    {
        public int IdDetalleVenta { get; set; }
        public Servicio oServicio { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal Subtotal { get; set;}
        public DateTime FechaRegistro { get; set; }
    }
}
