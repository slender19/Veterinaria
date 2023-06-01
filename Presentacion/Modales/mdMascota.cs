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
    public partial class mdMascota : Form
    {
        public Mascota _Mascota { get; set; }
        public string dueño;
        public mdMascota(string documento )
        {
            dueño = documento;
            InitializeComponent();
        }
        SrvMascota srvMascota = new SrvMascota(ConfigConnection.ConnectionString);
        private void mdMascota_Load(object sender, EventArgs e)
        {
            monstrarcbBuscar();
            CargarGrilla();
        }

        void monstrarcbBuscar()
        {
            foreach (DataGridViewColumn columna in dgvMascota.Columns)
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
            dgvMascota.Rows.Clear();
            var listaMascota = srvMascota.MostrarMascota();
            foreach (Mascota item in listaMascota)
            {
                if (item.Estado && item.oCliente.Documento == dueño)
                {
                    dgvMascota.Rows.Add(new object[] {
                    item.NombreMascota,item.Tipo, item.oCliente.Documento

                    });
                }
            }
        }

        private void dgvMascota_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            int iColum = e.ColumnIndex;

            if (iRow >= 0 && iColum >= 0)
            {
                _Mascota = new Mascota()
                {
                    NombreMascota = dgvMascota.Rows[iRow].Cells["NombreMascota"].Value.ToString(),
                    Tipo = dgvMascota.Rows[iRow].Cells["Tipo"].Value.ToString()
                };
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
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
    }
}
