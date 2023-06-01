using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class SrvServicio
    {
        ArchivoServicio archivoServicio;
        public SrvServicio(string conexion)
        {
            archivoServicio = new ArchivoServicio(conexion);
        }

        public List<Servicio> MostrarServicio()
        {
            return archivoServicio.GetAll();
        }


        public int Registar(Servicio servicio, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (servicio.NombreServicio == "")
            {
                Mensaje += "Es necesario el nombre  del servicio\n";
            }
            if (servicio.Codigo == "")
            {
                Mensaje += "Es necesario el Codigo del servicio\n";
            }
           
            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return archivoServicio.Registrar(servicio, out Mensaje);
            }
        }


        public bool Editar(Servicio servicio, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (servicio.NombreServicio == "")
            {
                Mensaje += "Es necesario el nombre  del servicio\n";
            }
            if (servicio.Codigo == "")
            {
                Mensaje += "Es necesario el Codigo del servicio\n";
            }
            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {

                return archivoServicio.Editar(servicio, out Mensaje);
            }
        }



        public bool Eliminar(Servicio servicio, out string Mensaje)
        {
            return archivoServicio.Eliminar(servicio, out Mensaje);
        }


    }
}
