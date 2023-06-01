using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace Datos
{
    public class Conexion
    {

        public static string ConnectionString = ConfigurationManager.ConnectionStrings["cadena_conexion"].ConnectionString;

        protected SqlConnection _Conexion;
        //cadena de conexion con la base de datos y objetos de conexion
        public Conexion(string connectionString)
        {
                _Conexion = new SqlConnection(connectionString);
        }

        //abre la conexion con la base de datos//
        public void Open()
        {
            _Conexion.Open();

        }
        //Cierra la conexion con la base de datos//
        public void Closed()
        {
            _Conexion.Close();
        }

        
    }
}
