using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Mascota 
    {
        public Mascota()
        {
            
        }
        public int IdMascota { get; set; }
        public Cliente oCliente { get; set; }
        public string NombreMascota { get; set; }
        public string Tipo { get; set; }
        public string Raza { get; set;}
        
        public bool Estado { get; set; }
        public DateTime FechaRegistro {  get; set; }

       
    }
}
