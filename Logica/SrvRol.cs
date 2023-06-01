using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class SrvRol
    {
        ArchivoRol archivoRol;
        public SrvRol(string conexion)
        {
            archivoRol = new ArchivoRol(conexion);
        }

        public List<Rol> Listar()
        {
            return archivoRol.Listar();
        }
    }
}
