using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class SrvNegocio
    {

        ArchivoNegocio archivoNegocio;
        public SrvNegocio(string conexion)
        {
            archivoNegocio = new ArchivoNegocio(conexion);
        }

        public Negocio obtenerDatos()
        {
            return archivoNegocio.obtenerDatos();
        }


        public bool Guardardatos(Negocio negocio, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (negocio.Nombre == "")
            {
                Mensaje += "Es necesario el nombre\n";
            }
            if (negocio.RUC == "")
            {
                Mensaje += "Es necesario el RUC\n";
            }
            if (negocio.Direccion == "")
            {
                Mensaje += "Es necesario la direccion\n";

            }
           

            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return archivoNegocio.Guardardatos(negocio, out Mensaje);
            }
        }


        public byte[] ObtenerLogo(out bool obtenido)
        {
            return archivoNegocio.ObtenerLogo(out obtenido);
        }

        public bool ActualizarLogo(byte[] imagen ,out string mensaje)
        {
            return archivoNegocio.ActualizarLogo(imagen, out mensaje);
        }
    }
}
