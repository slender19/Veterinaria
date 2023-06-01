using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Servicio
    {
        public Servicio()
        {
            
        }

        public int IdServicio { get; set; }
        public string Codigo { get; set; }
        public string NombreServicio { get; set; }
        public decimal PrecioVenta { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }

        
    }
}
