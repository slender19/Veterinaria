using ClosedXML.Excel;
using Datos;
using Entidades;
using Logica;
using Presentacion.Utilidades;
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
    public partial class FrmClientes : Form
    {
        public FrmClientes()
        {
            InitializeComponent();
        }
        SrvClientes srvClientes = new SrvClientes(ConfigConnection.ConnectionString);
        private void FrmClientes_Load(object sender, EventArgs e)
        {
            mostrarcbEstado();
            monstrarcbBuscar();
            CargarGrilla();
        }

        void mostrarcbEstado()
        {
            cbEstado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo" });
            cbEstado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "Inactivo" });
            //muestra el dato de "Texto"//
            cbEstado.DisplayMember = "Texto";
            //no lo muestra pero lo evalua como valor interno//
            cbEstado.ValueMember = "Valor";
            cbEstado.SelectedIndex = 0;

        }

        void monstrarcbBuscar()
        {
            foreach (DataGridViewColumn columna in dgvCliente.Columns)
            {
                if (columna.Visible == true && columna.Name != "btnSeleccionar")
                {
                    cbBusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cbBusqueda.DisplayMember = "Texto";
            cbBusqueda.ValueMember = "Valor";
            cbBusqueda.SelectedIndex = 0;
        }

        private void CargarGrilla()
        {
            dgvCliente.Rows.Clear();
            var listacliente = srvClientes.MostrarCliente();
            foreach (Cliente item in listacliente)
            {
                dgvCliente.Rows.Add(new object[] {"",item.IdCliente,
                    item.Documento,item.NombreCliente,
                    item.Telefono, item.Estado == true?1:0,
                    item.Estado ==true ?"Activo":"Inactivo"

                });
            }
        }

        void limpiar()
        {
            txtDocumento.Text = "";
            txtNombreCliente.Text = "";
            txtTelefono.Text = "";
            cbEstado.SelectedIndex = 0;

            txtDocumento.Select();
        }

        private void dgvCliente_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                //variable para obtener el ancho de la imagen//
                var w = Properties.Resources.check20.Width;
                //variable para obtener el alto de la imagen//
                var h = Properties.Resources.check20.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;


                e.Graphics.DrawImage(Properties.Resources.check20, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }

        private void dgvCliente_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCliente.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    txtIndice.Text = indice.ToString();
                    txtId.Text = dgvCliente.Rows[indice].Cells["Id"].Value.ToString();
                    txtDocumento.Text = dgvCliente.Rows[indice].Cells["Documento"].Value.ToString();
                    txtNombreCliente.Text = dgvCliente.Rows[indice].Cells["NombreCliente"].Value.ToString();
                    txtTelefono.Text = dgvCliente.Rows[indice].Cells["Telefono"].Value.ToString();

                    foreach (OpcionCombo oc in cbEstado.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvCliente.Rows[indice].Cells["EstadoValor"].Value))
                        {
                            int indice_combo = cbEstado.Items.IndexOf(oc);
                            cbEstado.SelectedIndex = indice_combo;
                            break;
                        }
                    }
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string Mensaje = string.Empty;
            Cliente objcliente = new Cliente()
            {
                IdCliente = Convert.ToInt32(txtId.Text),
                Documento = txtDocumento.Text,
                NombreCliente = txtNombreCliente.Text,
                Telefono = txtTelefono.Text,
                Estado = Convert.ToInt32(((OpcionCombo)cbEstado.SelectedItem).Valor) == 1 ? true : false
            };
            var idusuariogenerado = srvClientes.Registar(objcliente, out Mensaje);

            if (idusuariogenerado == 0)
            {
                MessageBox.Show(Mensaje);
            }
            CargarGrilla();
            limpiar();

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            string Mensaje = string.Empty;
            Cliente objcliente = new Cliente()
            {
                IdCliente = Convert.ToInt32(txtId.Text),
                Documento = txtDocumento.Text,
                NombreCliente = txtNombreCliente.Text,
                Telefono = txtTelefono.Text,
                Estado = Convert.ToInt32(((OpcionCombo)cbEstado.SelectedItem).Valor) == 1 ? true : false
            };

            bool resultado = srvClientes.Editar(objcliente, out Mensaje);
            if (resultado == false)
            {
                MessageBox.Show(Mensaje);

            }
            CargarGrilla();
            limpiar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtId.Text) != 0)
            {
                if (MessageBox.Show("¿Desea eliminar el cliente?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string Mensaje = string.Empty;
                    Cliente objcliente = new Cliente()
                    {
                        IdCliente = Convert.ToInt32(txtId.Text)
                    };
                    bool respuesta = srvClientes.Eliminar(objcliente, out Mensaje);

                    if (respuesta == false)
                    {
                        MessageBox.Show(Mensaje);
                    }
                }
            }
            CargarGrilla();
            limpiar();
        }

        private void btnbusqueda_Click(object sender, EventArgs e)
        {
            string columnaFlitro = ((OpcionCombo)cbBusqueda.SelectedItem).Valor.ToString();

            if (dgvCliente.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvCliente.Rows)
                {
                    if (row.Cells[columnaFlitro].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))
                    {
                        row.Visible = true;
                    }
                    else
                    {
                        row.Visible = false;
                    }
                }
            }
        }


        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";

            foreach (DataGridViewRow row in dgvCliente.Rows)
            {
                row.Visible = true;
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dgvCliente.Rows.Count < 1)
            {
                MessageBox.Show("No hay datos para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                Exportar();

            }
        }


        //funcion para exportar los datos de la grilla//
        void Exportar()
        {
            DataTable dt = new DataTable();

            foreach (DataGridViewColumn column in dgvCliente.Columns)
            {
                if (column.HeaderText != "" && column.Visible)
                {
                    dt.Columns.Add(column.HeaderText, typeof(string));
                }
            }

            foreach (DataGridViewRow row in dgvCliente.Rows)
            {
                if (row.Visible)
                {
                    //comprabar datos de cabeceras visibles//
                    dt.Rows.Add(new object[]
                    {
                            row.Cells[2].Value.ToString(),
                            row.Cells[3].Value.ToString(),
                            row.Cells[4].Value.ToString(),
                            row.Cells[6].Value.ToString(),
                    });
                }
            }

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = string.Format("ReporteServicio_{0}.xlsx", DateTime.Now.ToString("ddMMyyyy"));
            saveFile.Filter = "Excel files | *.xslx";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    XLWorkbook wb = new XLWorkbook();
                    var hoja = wb.Worksheets.Add(dt, "Informe");
                    hoja.ColumnsUsed().AdjustToContents();
                    wb.SaveAs(saveFile.FileName);
                    MessageBox.Show("Reporte generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                catch
                {
                    MessageBox.Show("Error al generar el reporte", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        //validaciones//
        private void txtDocumento_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (Char.IsControl(e.KeyChar))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            
        }

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (Char.IsControl(e.KeyChar))
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
}