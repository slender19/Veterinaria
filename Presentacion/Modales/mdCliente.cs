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
    public partial class mdCliente : Form
    {
        public Cliente _Cliente { get; set; }
        public mdCliente()
        {
            InitializeComponent();
        }
        SrvClientes srvClientes = new SrvClientes(ConfigConnection.ConnectionString);
        private void mdCliente_Load(object sender, EventArgs e)
        {
            monstrarcbBuscar();
            CargarGrilla();
        }

        void monstrarcbBuscar()
        {
            foreach (DataGridViewColumn columna in dgvCliente.Columns)
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
            dgvCliente.Rows.Clear();
            var listacliente = srvClientes.MostrarCliente();
            foreach (Cliente item in listacliente)
            {
                dgvCliente.Rows.Add(new object[] {item.IdCliente,
                    item.Documento,item.NombreCliente
                });
            }
        }

        private void dgvCliente_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            int iColum = e.ColumnIndex;

            if(iRow >= 0 && iColum > 0)
            {
                _Cliente = new Cliente()
                {
                    IdCliente = Convert.ToInt32(dgvCliente.Rows[iRow].Cells["Id"].Value.ToString()),
                    Documento = dgvCliente.Rows[iRow].Cells["Documento"].Value.ToString(),
                    NombreCliente = dgvCliente.Rows[iRow].Cells["NombreCliente"].Value.ToString()
                };
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
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
    }
}
