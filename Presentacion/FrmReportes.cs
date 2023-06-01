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
    public partial class FrmReportes : Form
    {
        public FrmReportes()
        {
            InitializeComponent();
        }
        SrvReporte srvReporte = new SrvReporte(ConfigConnection.ConnectionString);
        private void FrmReportes_Load(object sender, EventArgs e)
        {
            monstrarcbBuscar();
        }


        void monstrarcbBuscar()
        {
            foreach (DataGridViewColumn columna in dgvReporte.Columns)
            {
                
                cbBusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                
            }
            cbBusqueda.DisplayMember = "Texto";
            cbBusqueda.ValueMember = "Valor";
            cbBusqueda.SelectedIndex = 0;
        }

        private void cargargrilla()
        {
            dgvReporte.Rows.Clear();
            //var listareporte = srvReporte.ReporteVenta();
        }

        

        private void btnbuscarreporte_Click(object sender, EventArgs e)
        {
            List<ReporteVentas> lista = new List<ReporteVentas>();
            lista = srvReporte.ReporteVenta(dtFechaInicio.Value.ToString(), dtFechaFin.Value.ToString());

            dgvReporte.Rows.Clear();
            foreach (ReporteVentas item in lista)
            {
                dgvReporte.Rows.Add(new object[] { item.FechaRegistro,item.NumeroDocumento,
                    item.MontoTotal, item.NombreUsuario, item.DocumentoCliente,
                    item.NombreCliente, item.NombreMascota, item.TipoMascota,
                    item.CodigoServicio, item.NombreServicio, item.PrecioVenta
                });
            }
        }


        private void btnbusqueda_Click(object sender, EventArgs e)
        {
            string columnaFlitro = ((OpcionCombo)cbBusqueda.SelectedItem).Valor.ToString();

            if (dgvReporte.Rows.Count > 0)
            {
               

                foreach (DataGridViewRow row in dgvReporte.Rows)
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

            foreach (DataGridViewRow row in dgvReporte.Rows)
            {
                row.Visible = true;
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if(dgvReporte.Rows.Count < 1)
            {
                MessageBox.Show("No hay refgistros para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                Exportar();
            }
            
        }


        void Exportar()
        {
            DataTable dt = new DataTable();

            foreach (DataGridViewColumn column in dgvReporte.Columns)
            {
                
                dt.Columns.Add(column.HeaderText, typeof(string));
                
            }

            foreach (DataGridViewRow row in dgvReporte.Rows)
            {
                if (row.Visible)
                {
                    dt.Rows.Add(new object[]
                    {
                            row.Cells[0].Value.ToString(),
                            row.Cells[1].Value.ToString(),
                            row.Cells[2].Value.ToString(),
                            row.Cells[3].Value.ToString(),
                            row.Cells[4].Value.ToString(),
                            row.Cells[5].Value.ToString(),
                            row.Cells[6].Value.ToString(),
                            row.Cells[7].Value.ToString(),
                            row.Cells[8].Value.ToString(),
                            row.Cells[9].Value.ToString(),
                            row.Cells[10].Value.ToString(),
                    });
                }
            }

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = string.Format("ReporteVenta_{0}.xlsx", DateTime.Now.ToString("ddMMyyyy"));
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
    }
}
