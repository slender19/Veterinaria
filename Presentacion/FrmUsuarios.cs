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
    public partial class FrmUsuarios : Form
    {
        ServicioUsuario srvUsuarios = new ServicioUsuario(ConfigConnection.ConnectionString);
        SrvRol srvRol = new SrvRol(ConfigConnection.ConnectionString);
        public FrmUsuarios()
        {
            InitializeComponent();
        }
        
        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            mostrarcbEstado();
            mostrarcbRol();
            monstrarcbBuscar();
            CargarGrilla();
        }

        //Metodo para rellenar el combo box de estado//
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

        //Metodo para rellenar el combo box de Rol//
        void mostrarcbRol()
        {
           
            var listarol = srvRol.Listar();
            //recore la lista de roles y assigna la descripcion al combobox//
            foreach (Rol item in listarol)
            {
                cbRol.Items.Add(new OpcionCombo() { Valor = item.IdRol, Texto = item.DescripcionRol.ToUpper() });
            }
            //muestra el dato de "Texto"//
            cbRol.DisplayMember = "Texto";
            //no lo muestra pero lo evalua como valor interno//
            cbRol.ValueMember = "Valor";
            cbRol.SelectedIndex = 0;

        }


        //Metodo para rellenar el combo box de busqueda//
        void monstrarcbBuscar()
        {
            foreach(DataGridViewColumn columna in dgvUsuarios.Columns)
            {
                if(columna.Visible == true && columna.Name != "btnSeleccionar")
                {
                    cbBusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cbBusqueda.DisplayMember = "Texto";
            cbBusqueda.ValueMember = "Valor";
            cbBusqueda.SelectedIndex = 0;
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtConfirmarClave.Text != txtClave.Text)
            {
                MessageBox.Show("¿La confirmacion de la contraseña no coicide con la contraseña", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {

                string Mensaje = string.Empty;
                Usuarios objusuarios = new Usuarios()
                {
                    IdUsuario = Convert.ToInt32(txtId.Text),
                    Documento = txtDocumento.Text,
                    NombreUsuario = txtNombreUsuario.Text,
                    Correo = txtCorreo.Text,
                    Clave = txtClave.Text,
                    oRol = new Rol() { IdRol = Convert.ToInt32(((OpcionCombo)cbRol.SelectedItem).Valor) },
                    Estado = Convert.ToInt32(((OpcionCombo)cbEstado.SelectedItem).Valor) == 1 ? true : false
                };
                var idusuariogenerado = srvUsuarios.Registar(objusuarios, out Mensaje);

                if (idusuariogenerado == 0)
                {
                    MessageBox.Show(Mensaje);
                }
                CargarGrilla();
                Limpiar();
            }
           
        }


        //metodo para cargar en la grilla la lista de usuarios//
        private void CargarGrilla()
        {
            dgvUsuarios.Rows.Clear();
            var listausuarios = srvUsuarios.MostrarUsuarios();
            foreach(Usuarios item in listausuarios)
            {
                dgvUsuarios.Rows.Add(new object[] {"",item.IdUsuario,
                    item.Documento,item.NombreUsuario,item.Correo,item.Clave,
                    item.oRol.IdRol, item.oRol.DescripcionRol,
                    item.Estado == true?1:0,
                    item.Estado ==true ?"Activo":"Inactivo"
                });
            }

        }

        //metodo para limbiar los texbox del detalle usuario//
        private void Limpiar()
        {
            txtIndice.Text = "-1";
            txtId.Text = "0";
            txtDocumento.Text = "";
            txtNombreUsuario.Text = "";
            txtCorreo.Text = "";
            txtClave.Text = "";
            txtConfirmarClave.Text = "";
            cbEstado.SelectedIndex = 0;
            cbRol.SelectedIndex = 0;

            txtDocumento.Select();
        }

        private void dgvUsuarios_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if(e.ColumnIndex == 0)
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


        //metodo para que al momento de seleccionar un usuario de la grilla se rellenen en los txt y cbx//
        private void dgvUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvUsuarios.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    txtIndice.Text = indice.ToString();
                    txtId.Text = dgvUsuarios.Rows[indice].Cells["Id"].Value.ToString();
                    txtDocumento.Text=dgvUsuarios.Rows[indice].Cells["Documento"].Value.ToString();
                    txtNombreUsuario.Text =  dgvUsuarios.Rows[indice].Cells["NombreUsuario"].Value.ToString();
                    txtCorreo.Text = dgvUsuarios.Rows[indice].Cells["Correo"].Value.ToString();
                    txtClave.Text= dgvUsuarios.Rows[indice].Cells["Clave"].Value.ToString();
                    txtConfirmarClave.Text = dgvUsuarios.Rows[indice].Cells["Clave"].Value.ToString();

                    foreach (OpcionCombo oc in cbRol.Items)
                    {
                        if(Convert.ToInt32( oc.Valor) == Convert.ToInt32(dgvUsuarios.Rows[indice].Cells["IdRol"].Value))
                        {
                            int indice_combo = cbRol.Items.IndexOf(oc);
                            cbRol.SelectedIndex = indice_combo;
                            break;
                        }
                    }
                    foreach (OpcionCombo oc in cbEstado.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvUsuarios.Rows[indice].Cells["EstadoValor"].Value))
                        {
                            int indice_combo = cbEstado.Items.IndexOf(oc);
                            cbEstado.SelectedIndex = indice_combo;
                            break;
                        }
                    }
                }
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            string Mensaje = string.Empty;
            Usuarios objusuarios = new Usuarios()
            {
                IdUsuario = Convert.ToInt32(txtId.Text),
                Documento = txtDocumento.Text,
                NombreUsuario = txtNombreUsuario.Text,
                Correo = txtCorreo.Text,
                Clave = txtClave.Text,
                oRol = new Rol() { IdRol = Convert.ToInt32(((OpcionCombo)cbRol.SelectedItem).Valor) },
                Estado = Convert.ToInt32(((OpcionCombo)cbEstado.SelectedItem).Valor) == 1 ? true : false
            };

            bool resultado =  srvUsuarios.Editar(objusuarios, out Mensaje);
            if(resultado == false)
            {
                MessageBox.Show(Mensaje);

            }
            CargarGrilla();
            Limpiar();

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if(Convert.ToInt32(txtId.Text) != 0)
            {
                if(MessageBox.Show("¿Desea eliminar el usuario", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string Mensaje = string.Empty;
                    Usuarios objusuarios = new Usuarios()
                    {
                        IdUsuario = Convert.ToInt32(txtId.Text)
                    };
                    bool respuesta = srvUsuarios.Eliminar(objusuarios, out Mensaje);

                    if(respuesta == false) 
                    { 
                        MessageBox.Show(Mensaje);
                    }
                }
            }
            CargarGrilla();
            Limpiar() ;

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string columnaFlitro = ((OpcionCombo)cbBusqueda.SelectedItem).Valor.ToString();

            if(dgvUsuarios.Rows.Count > 0 ) 
            {
                foreach (DataGridViewRow row in dgvUsuarios.Rows)
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

            foreach (DataGridViewRow row in dgvUsuarios.Rows)
            {
                row.Visible = true;
            }
        }

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


    }
}
