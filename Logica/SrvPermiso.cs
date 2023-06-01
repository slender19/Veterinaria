using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class SrvPermiso
    {
        //instancia con la capa datos
        ArchivoPermisos archivoPermisos;
        public SrvPermiso(string conexion)
        {
            archivoPermisos = new ArchivoPermisos(conexion);
        }

        //metodo para listar todos los permisos almacenados en la base de datos haciendo llamando de un metodo de la capa de datos
        public List<Permisos> Listar (int IdUsuario)
        {
            return archivoPermisos.Listar(IdUsuario);
        }

    }
}
