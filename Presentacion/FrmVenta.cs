using Datos;
using Entidades;
using Logica;
using Org.BouncyCastle.Crypto.Tls;
using Presentacion.Modales;
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
    
    public partial class FrmVenta : Form
    {
        private Usuarios usuarioactual;
        SrvServicio srvServicio = new SrvServicio((ConfigConnection.ConnectionString));
        SrvVenta srvVenta = new SrvVenta(ConfigConnection.ConnectionString);
        public FrmVenta(Usuarios oUsuarios = null)
        {
            usuarioactual = oUsuarios;
            InitializeComponent();
        }



        private void FrmVenta_Load(object sender, EventArgs e)
        {
            lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtIdServicio.Text = "0";
            txtMontoTotal.Text = "0";
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            using (var modal = new mdCliente())
            {
                var result = modal.ShowDialog();

                if (result == DialogResult.OK)
                {

                    txtDocumento.Text = modal._Cliente.Documento;
                    txtNombreCliente.Text = modal._Cliente.NombreCliente;
                }
                else
                {
                    txtDocumento.Select();
                }
            }
        }

        private void btnBuscarMascota_Click(object sender, EventArgs e)
        {
            if (txtDocumento.Text == "")
            {
                MessageBox.Show("Debe seleccionar un cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                string documento = txtDocumento.Text;
                using (var modal = new mdMascota(documento))
                {
                    var result = modal.ShowDialog();

                    if (result == DialogResult.OK)
                    {

                        txtNombreMascota.Text = modal._Mascota.NombreMascota;
                        txtTipoMacota.Text = modal._Mascota.Tipo;
                    }
                    else
                    {
                        txtNombreMascota.Select();
                    }
                }
            }
        }

        private void btnBuscarServicio_Click(object sender, EventArgs e)
        {
            using (var modal = new mdServicio())
            {
                var result = modal.ShowDialog();

                if (result == DialogResult.OK)
                {
                    txtIdServicio.Text = modal._Servicio.IdServicio.ToString();
                    txtCodigo.Text = modal._Servicio.Codigo;
                    txtNombreServicio.Text = modal._Servicio.NombreServicio;
                    txtPrecio.Text = modal._Servicio.PrecioVenta.ToString();
                }
                else
                {
                    txtIdServicio.Select();
                }

            }
        }

        private void txtCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                var oServicio = srvServicio.MostrarServicio().Where(p => p.Codigo == txtCodigo.Text && p.Estado == true).FirstOrDefault();

                if (oServicio != null)
                {
                    txtCodigo.BackColor = Color.Honeydew;
                    txtIdServicio.Text = oServicio.IdServicio.ToString();
                    txtCodigo.Text = oServicio.Codigo;
                    txtNombreServicio.Text = oServicio.NombreServicio;
                    txtPrecio.Text = oServicio.PrecioVenta.ToString();
                }
                else
                {
                    txtCodigo.BackColor = Color.MistyRose;
                    txtIdServicio.Text = "0";
                    txtCodigo.Text = "";
                    txtNombreServicio.Text = "";
                    txtPrecio.Text = "";
                }
            }
            
            
        }

        private void btnagregarproducto_Click(object sender, EventArgs e)
        {
            decimal precio = 0;
            bool productoExiste = false;

            if(int.Parse(txtIdServicio.Text  ) == 0)
            {
                MessageBox.Show("Debe selecionar un servicio", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if(!decimal.TryParse(txtPrecio.Text, out precio))
            {
                MessageBox.Show("Precio - Formato incorreto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPrecio.Select();
                return;
            }
            foreach(DataGridViewRow fila in dgvVenta.Rows) 
            {
                if (fila.Cells["IdServicio"].Value.ToString() == txtIdServicio.Text) 
                {
                    productoExiste = true;
                    break;
                }
            }

            if(!productoExiste)
            {
                dgvVenta.Rows.Add(new object[] { txtIdServicio.Text,
                    txtNombreServicio.Text,
                    precio.ToString("0.00")
                });

                calcularTotal();
                limpiarServicio();
                txtCodigo.Select();
            }
        }

        private void calcularTotal()
        {
            decimal total = 0;
            if(dgvVenta.Rows.Count > 0)
            {
                foreach(DataGridViewRow row in dgvVenta.Rows)
                {
                    total += Convert.ToDecimal(row.Cells["Precio"].Value.ToString());
                }
                txtMontoTotal.Text = total.ToString("0.00");
            }

        }

        private void limpiarServicio()
        {
            txtIdServicio.Text = "0";
            txtCodigo.Text = "";
            txtNombreServicio.Text="";
            txtPrecio.Text = "";
        }

        private void dgvVenta_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == 3)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                var w = Properties.Resources.check20.Width;
                var h = Properties.Resources.check20.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;


                e.Graphics.DrawImage(Properties.Resources.delete32, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }

        private void dgvVenta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvVenta.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                int index = e.RowIndex;
                if (index >= 0)
                {
                    dgvVenta.Rows.RemoveAt(index);
                    calcularTotal();
                }
            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (txtPrecio.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true;
                }
                else
                {
                    if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }

            }
        }

        private void btncrearventa_Click(object sender, EventArgs e)
        {
            Validar();

            guardarVenta();

        }

        //metodo validacion de ingresar datos para guardar la venta y detalle venta//
        void Validar()
        {
            if (txtDocumento.Text == "")
            {
                MessageBox.Show("Debe ingresar el documento del cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (txtNombreCliente.Text == "")
            {
                MessageBox.Show("Debe ingresar el nombre del cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (txtNombreMascota.Text == "")
            {
                MessageBox.Show("Debe ingresar el nombre de la mascota", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (txtTipoMacota.Text == "")
            {
                MessageBox.Show("Debe ingresar el tipo de la mascota", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (dgvVenta.Rows.Count < 1)
            {
                MessageBox.Show("Debe ingresar productos en la venta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }


        //metodo para guardar todos los datos de venta y detalle venta presentes en el formulario//
        void guardarVenta()
        {
            DataTable detalle_venta = new DataTable();
            detalle_venta.Columns.Add("IdServicio", typeof(int));
            detalle_venta.Columns.Add("PrecioVenta", typeof(decimal));

            foreach (DataGridViewRow row in dgvVenta.Rows)
            {
                detalle_venta.Rows.Add(new object[]
                {
                    row.Cells["IdServicio"].Value.ToString(),
                    row.Cells["Precio"].Value.ToString()
                });
            }

            int idcorrelativo = srvVenta.ObtenerCorrelativo();
            string numeroDocumento = string.Format("{0:00000}", idcorrelativo);

            Venta oventa = new Venta()
            {
                oUsuario = new Usuarios() { IdUsuario = usuarioactual.IdUsuario },
                NumeroDocumento = numeroDocumento,
                DocumentoCliente = txtDocumento.Text,
                NombreCliente = txtNombreCliente.Text,
                MontoTotal = Convert.ToDecimal(txtMontoTotal.Text),
                NombreMacota = txtNombreMascota.Text,
                TipoMascota = txtTipoMacota.Text
            };

            string mensaje = string.Empty;
            bool respuesta = srvVenta.Registrar(oventa, detalle_venta, out mensaje);
            if (respuesta)
            {
                var result = MessageBox.Show("Numero de venta generado:\n" + numeroDocumento + "\n\n¿Desea copiar al portapapeles?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Yes)
                {
                    Clipboard.SetText(numeroDocumento);
                }

                limpiarTodo();
            }
            else
            {
                MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void limpiarTodo()
        {
            txtDocumento.Text = "";
            txtNombreCliente.Text = "";
            txtMontoTotal.Text = "";
            txtNombreMascota.Text = "";
            txtTipoMacota.Text = "";
            dgvVenta.Rows.Clear();
        }
    }
}
