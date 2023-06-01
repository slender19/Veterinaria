using Datos;
using Entidades;
using Logica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class FrmDetalleVenta : Form
    {
        public FrmDetalleVenta()
        {
            InitializeComponent();
        }
        SrvVenta srvVenta = new SrvVenta(ConfigConnection.ConnectionString);

        private void FrmDetalleVenta_Load(object sender, EventArgs e)
        {
            txtNumeroDocumento.Select();
        }
        private void btnbuscar_Click(object sender, EventArgs e)
        {
            Venta oVenta = srvVenta.obtenerVenta(txtBusqueda.Text);

            if(oVenta.IdVenta != 0)
            {
                txtFecha.Text = Convert.ToString(oVenta.FechaRegistro);
                txtNumeroDocumento.Text = oVenta.NumeroDocumento;
                txtUsuario.Text = oVenta.oUsuario.NombreUsuario;

                txtNombreCliente.Text = oVenta.NombreCliente;
                txtDocumentoCliente.Text = oVenta.DocumentoCliente;

                txtNombreMascota.Text = oVenta.NombreMacota;
                txtTipoMacota.Text = oVenta.TipoMascota;

                dgvVenta.Rows.Clear();
                foreach(DetalleVenta dv in oVenta.oDetalleVentas)
                {
                    dgvVenta.Rows.Add(new object[] { dv.oServicio.NombreServicio, dv.PrecioVenta });
                }
                txtMontoTotal.Text = oVenta.MontoTotal.ToString("0.00");
            }
        }

        private void btnborrar_Click(object sender, EventArgs e)
        {
            txtFecha.Text = "";
            txtNumeroDocumento.Text = "";
            txtUsuario.Text = "";

            txtNombreCliente.Text = "";
            txtDocumentoCliente.Text = "";

            txtNombreMascota.Text = "";
            txtTipoMacota.Text = "";

            dgvVenta.Rows.Clear();
            txtMontoTotal.Text = "0.00";
        }

        
    } 
}
