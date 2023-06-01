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

namespace Presentacion.Modales
{
    public partial class mdServicio : Form
    {
        public Servicio _Servicio { get; set; }
        public mdServicio()
        {
            InitializeComponent();
        }
        SrvServicio srvServicio = new SrvServicio((ConfigConnection.ConnectionString));

        private void mdServicio_Load(object sender, EventArgs e)
        {
            monstrarcbBuscar();
            CargarGrilla();
        }

        void monstrarcbBuscar()
        {
            foreach (DataGridViewColumn columna in dgvServicio.Columns)
            {
                if (columna.Visible == true)
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
            dgvServicio.Rows.Clear();
            var listausuarios = srvServicio.MostrarServicio();
            foreach (Servicio item in listausuarios)
            {
                dgvServicio.Rows.Add(new object[] {item.IdServicio,
                    item.Codigo,item.NombreServicio,item.PrecioVenta

                });
            }

        }

        private void dgvServicio_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            int iColum = e.ColumnIndex;

            if (iRow >= 0 && iColum > 0)
            {
                _Servicio = new Servicio()
                {
                    IdServicio = Convert.ToInt32(dgvServicio.Rows[iRow].Cells["Id"].Value.ToString()),
                    Codigo = dgvServicio.Rows[iRow].Cells["Codigo"].Value.ToString(),
                    NombreServicio = dgvServicio.Rows[iRow].Cells["NombreServicio"].Value.ToString(),
                    PrecioVenta = Convert.ToDecimal(dgvServicio.Rows[iRow].Cells["PrecioVenta"].Value.ToString())
                };
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string columnaFlitro = ((OpcionCombo)cbBusqueda.SelectedItem).Valor.ToString();

            if (dgvServicio.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvServicio.Rows)
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

            foreach (DataGridViewRow row in dgvServicio.Rows)
            {
                row.Visible = true;
            }
        }


    }
}
