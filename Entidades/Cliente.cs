using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Cliente
    {
        public Cliente()
        {
            
        }
        public int IdCliente { get; set; }
        public string Documento { get; set; }
        public string NombreCliente { get; set; } 
        public string Telefono{ get; set;}
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        

        
    }
}
