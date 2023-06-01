using Datos;
using Entidades;
using Logica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class FrmNegocio : Form
    {
        public FrmNegocio()
        {
            InitializeComponent();
        }
        SrvNegocio srvNegocio = new SrvNegocio(ConfigConnection.ConnectionString);
        private void FrmNegocio_Load(object sender, EventArgs e)
        {
            mostrarImagen();
            mostrarDatos();
        }

        //metodo para comvetir de bytes a imagen//
        public Image byteToImage(byte[] imageBytes)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = new Bitmap(ms);

            return image;
        }

        //metodo para visualizar la imagen//
        void mostrarImagen()
        {
            bool obtenido = true;
            byte[] byteimagen = srvNegocio.ObtenerLogo(out obtenido);

            if (obtenido)
            {
                picLogo.Image = byteToImage(byteimagen);
            }
        }

        //metodo para visualizar los datos//
        void mostrarDatos()
        {
            Negocio datos = srvNegocio.obtenerDatos();
            txtNombre.Text = datos.Nombre;
            txtDireccion.Text = datos.Direccion;
            txtRUC.Text = datos.RUC;
             
        }

        private void btnSubir_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "File| * .jpg; *.jpeg; *.png";

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                byte[] byteImagen = File.ReadAllBytes(ofd.FileName);
                bool respuesta = srvNegocio.ActualizarLogo(byteImagen, out mensaje);

                if (respuesta)
                {
                    picLogo.Image = byteToImage(byteImagen);
                }
                else
                {
                    MessageBox.Show(mensaje, "mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            Negocio negocio = new Negocio()
            {
                Nombre = txtNombre.Text,
                RUC = txtRUC.Text,
                Direccion = txtDireccion.Text,
            };

            bool respuesta = srvNegocio.Guardardatos(negocio, out mensaje);

            if (respuesta)
            {
                MessageBox.Show("Los cambios fueron guardados", "mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("No se pudo guardar los cambios", "mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
