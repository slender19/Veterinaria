using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Entidades;

namespace Logica
{
    public class ServicioUsuario
    {
        //instancia con la capa de datos
        ArchivoUsuario archivoUsuario;
        public ServicioUsuario(string conexion)
        {
            archivoUsuario = new ArchivoUsuario(conexion);
        }
        //metodo para listar todos los usuarios almacenados en la base de datos haciendo llamando de un metodo de la capa de datos//
        public List<Usuarios> MostrarUsuarios()
        {
            return archivoUsuario.GetAll();
        }

        //metodo para guardar los usuarios haciendo llamado de un metodo de la capa de datos//
        public int  Registar(Usuarios usuario, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (usuario.NombreUsuario == "")
            {
                Mensaje += "Es necesario el nombre del usuario\n";
            }
            if (usuario.Documento == "")
            {
                Mensaje += "Es necesario el documento del usuario\n";
            }
            if (usuario.Clave == "")
            {
                Mensaje += "Es necesario la clave del usuario\n";
            }
            if(Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return archivoUsuario.Registrar(usuario, out Mensaje);
            }
        }


        //metodo para editar los usuarios haciendo llamado de un metodo de la capa de datos//
        public bool Editar(Usuarios usuario, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (usuario.NombreUsuario == "")
            {
                Mensaje += "Es necesario el nombre del usuario\n";
            }
            if (usuario.Documento == "")
            {
                Mensaje += "Es necesario el documento del usuario\n";
            }
            if (usuario.Clave == "")
            {
                Mensaje += "Es necesario la clave del usuario\n";
            }
            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {

                return archivoUsuario.Editar(usuario, out Mensaje);
            }
        }


        //metodo para eliminar usuarios haciendo llamado de un metodo de la capa de datos//
        public bool Eliminar(Usuarios usuario, out string Mensaje)
        {
            return archivoUsuario.Eliminar(usuario, out Mensaje);
        }
    }
}
