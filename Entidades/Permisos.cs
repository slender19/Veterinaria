using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Permisos
    {
        public int IdPermisos { get; set; }
        public Rol oRol { get; set; }
        public string NombreMenu { get; set; }
        public DateTime FechaRegistro {  get; set; } 
    }
}
