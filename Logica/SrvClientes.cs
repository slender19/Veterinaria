using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class SrvClientes
    {

        ArchivoCliente archivoCliente;
        public SrvClientes(string conexion)
        {
            archivoCliente = new ArchivoCliente(conexion);
        }

        public List<Cliente> MostrarCliente()
        {
            return archivoCliente.GetAll();
        }


        public int Registar(Cliente Cliente, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (Cliente.NombreCliente == "")
            {
                Mensaje += "Es necesario el nombre  del Cliente\n";
            }
            if (Cliente.Documento == "")
            {
                Mensaje += "Es necesario el documento del Cliente\n";
            }
            if (Cliente.Telefono  == "")
            {
                Mensaje += "Es necesario el telefono del cliente\n";

            }
          
            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return archivoCliente.Registrar(Cliente, out Mensaje);
            }
        }


        public bool Editar(Cliente Cliente, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (Cliente.NombreCliente == "")
            {
                Mensaje += "Es necesario el nombre  del Cliente\n";
            }
            if (Cliente.Documento == "")
            {
                Mensaje += "Es necesario el documento del Cliente\n";
            }
            if (Cliente.Telefono == "")
            {
                Mensaje += "Es necesario el telefono del cliente\n";

            }
            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {

                return archivoCliente.Editar(Cliente, out Mensaje);
            }
        }



        public bool Eliminar(Cliente Cliente, out string Mensaje)
        {
            return archivoCliente.Eliminar(Cliente, out Mensaje);
        }
    }
}
