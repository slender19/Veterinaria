using ClosedXML.Excel;
using Datos;
using Entidades;
using Logica;
using Presentacion.Modales;
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
    public partial class FrmMascotas : Form
    {
        public FrmMascotas()
        {
            InitializeComponent();
        }
        SrvMascota srvMascota = new SrvMascota(ConfigConnection.ConnectionString);
        private void FrmMascotas_Load(object sender, EventArgs e)
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
            foreach (DataGridViewColumn columna in dgvMascota.Columns)
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
            dgvMascota.Rows.Clear();
            var listaMascota = srvMascota.MostrarMascota();
            foreach (Mascota item in listaMascota)
            {
                dgvMascota.Rows.Add(new object[] {"",item.IdMascota,
                    item.NombreMascota,
                    item.Raza,item.Tipo, 
                    item.oCliente.IdCliente,  item.oCliente.Documento,
                    item.oCliente.NombreCliente,
                    item.Estado == true?1:0,
                    item.Estado ==true ?"Activo":"Inactivo"

                });
            }
        }

        void limpiar()
        {
            txtNombreMascota.Text = "";
            txtRaza.Text = "";
            txtTipo.Text = "";
            cbEstado.SelectedIndex = 0;
            txtDocumento.Text = "";
            txtNombreCliente.Text = "";

            txtNombreMascota.Select();
        }

        private void dgvMascota_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
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


        private void dgvMascota_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMascota.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    txtIndice.Text = indice.ToString();
                    txtId.Text = dgvMascota.Rows[indice].Cells["Id"].Value.ToString();
                    txtNombreMascota.Text = dgvMascota.Rows[indice].Cells["NombreMascota"].Value.ToString();
                    txtRaza.Text = dgvMascota.Rows[indice].Cells["Raza"].Value.ToString();
                    txtTipo.Text = dgvMascota.Rows[indice].Cells["Tipo"].Value.ToString();
                    txtIdCliente.Text = dgvMascota.Rows[indice].Cells["IdCliente"].Value.ToString();
                    txtDocumento.Text = dgvMascota.Rows[indice].Cells["Documento"].Value.ToString();
                    txtNombreCliente.Text = dgvMascota.Rows[indice].Cells["NombreCliente"].Value.ToString();
                    foreach (OpcionCombo oc in cbEstado.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvMascota.Rows[indice].Cells["EstadoValor"].Value))
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
            Mascota objmascota = new Mascota()
            {
                IdMascota = Convert.ToInt32(txtId.Text),
                NombreMascota = txtNombreMascota.Text,
                Raza = txtRaza.Text,
                Tipo = txtTipo.Text,
                Estado = Convert.ToInt32(((OpcionCombo)cbEstado.SelectedItem).Valor) == 1 ? true : false,
                oCliente = new Cliente { IdCliente = Convert.ToInt32(txtIdCliente.Text) }
            };
            var idusuariogenerado = srvMascota.Registar(objmascota, out Mensaje);

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
            Mascota objmascota = new Mascota()
            {
                IdMascota = Convert.ToInt32(txtId.Text),
                NombreMascota = txtNombreMascota.Text,
                Raza = txtRaza.Text,
                Tipo = txtTipo.Text,
                Estado = Convert.ToInt32(((OpcionCombo)cbEstado.SelectedItem).Valor) == 1 ? true : false,
                oCliente = new Cliente { IdCliente = Convert.ToInt32(txtIdCliente.Text)}
            };

            bool resultado = srvMascota.Editar(objmascota, out Mensaje);
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
                    Mascota objmascota = new Mascota()
                    {
                        IdMascota = Convert.ToInt32(txtId.Text)
                    };
                    bool respuesta = srvMascota.Eliminar(objmascota, out Mensaje);

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

            if (dgvMascota.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvMascota.Rows)
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

            foreach (DataGridViewRow row in dgvMascota.Rows)
            {
                row.Visible = true;
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dgvMascota.Rows.Count < 1)
            {
                MessageBox.Show("No hay datos para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                Exportar();

            }
        }


        void Exportar()
        {
            DataTable dt = new DataTable();

            foreach (DataGridViewColumn column in dgvMascota.Columns)
            {
                if (column.HeaderText != "" && column.Visible)
                {
                    dt.Columns.Add(column.HeaderText, typeof(string));
                }
            }

            foreach (DataGridViewRow row in dgvMascota.Rows)
            {
                if (row.Visible)
                {
                    dt.Rows.Add(new object[]
                    {
                            row.Cells[2].Value.ToString(),
                            row.Cells[3].Value.ToString(),
                            row.Cells[4].Value.ToString(),
                            row.Cells[6].Value.ToString(),
                            row.Cells[7].Value.ToString(),
                            row.Cells[9].Value.ToString(),
                    });
                }
            }

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = string.Format("ReporteServicio_{0}.xlsx", DateTime.Now.ToString("ddMMyyyyHHmmss"));
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

        private void iconButton1_Click(object sender, EventArgs e)
        {
            using (var modal = new mdCliente())
            {
                var result = modal.ShowDialog();

                if (result == DialogResult.OK)
                {
                    txtIdCliente.Text = modal._Cliente.IdCliente.ToString();
                    txtDocumento.Text = modal._Cliente.Documento;
                    txtNombreCliente.Text = modal._Cliente.NombreCliente;
                }
                else
                {
                    txtDocumento.Select();
                }
            }
        }
    }
}
