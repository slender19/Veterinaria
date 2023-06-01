using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class SrvMascota
    {

        ArchivoMascota archivoMascota;
        public SrvMascota(string conexion)
        {
            archivoMascota = new ArchivoMascota(conexion);
        }

        public List<Mascota> MostrarMascota()
        {
            return archivoMascota.GetAll();
        }


        public int Registar(Mascota Mascota, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (Mascota.NombreMascota == "")
            {
                Mensaje += "Es necesario el nombre  de la Mascota\n";
            }
            if (Mascota.Tipo == "")
            {
                Mensaje += "Es necesario el tipo de la Mascota\n";
            }
            if (Mascota.Raza == "")
            {
                Mensaje += "Es necesario la raza de la Mascota\n";

            }
            

            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return archivoMascota.Registrar(Mascota, out Mensaje);
            }
        }


        public bool Editar(Mascota Mascota, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (Mascota.NombreMascota == "")
            {
                Mensaje += "Es necesario el nombre  de la Mascota\n";
            }
            if (Mascota.Tipo == "")
            {
                Mensaje += "Es necesario el tipo de la Mascota\n";
            }
            if (Mascota.Raza == "")
            {
                Mensaje += "Es necesario la raza de la Mascota\n";

            }
            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {

                return archivoMascota.Editar(Mascota, out Mensaje);
            }
        }



        public bool Eliminar(Mascota Mascota, out string Mensaje)
        {
            return archivoMascota.Eliminar(Mascota, out Mensaje);
        }
    }
}
