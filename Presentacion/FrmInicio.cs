using Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontAwesome.Sharp;
using Logica;
using Datos;

namespace Presentacion
{
    public partial class FrmInicio : Form
    {
        //variable para almacenar el usuario con el que inicia sesión
        private static Usuarios usuarioActual;

        private static IconMenuItem MenuActivo = null;

        private static Form formularioActivo = null;
        public FrmInicio(Usuarios usuario)
        {
            //se le atribuye a la variable "usuarioActual" el usuario que inicia seccion traido desde el FrmLogin
            usuarioActual = usuario;
            InitializeComponent();
        }
        //instansia con la capa logica
        SrvPermiso srvPermiso = new Logica.SrvPermiso(ConfigConnection.ConnectionString);


        private void Inicio_Load(object sender, EventArgs e)
        {

            cargarmenus();

            lblUsuario.Text = usuarioActual.NombreUsuario;
        }

        //funcion o metodo para que se muestren los menus segun el rol
        private void cargarmenus()
        {
            List<Permisos> listapermisos = srvPermiso.Listar(usuarioActual.IdUsuario);

            foreach (IconMenuItem iconmenu in menu.Items)
            {
                bool encontrado = listapermisos.Any(m => m.NombreMenu == iconmenu.Name);

                if (encontrado == false)
                {
                    iconmenu.Visible = false;
                }
            }
        }

        //funcion o metodo para abrir formularion de paso cambia el color del iconmenu para saber cual se esta abriendo
        private void OpenForm(IconMenuItem menu, Form formulario)
        {
            if (MenuActivo != null)
            {
                MenuActivo.BackColor = Color.White;
            }
            menu.BackColor = Color.Silver;
            MenuActivo = menu;
            if(formularioActivo != null)
            {
                formularioActivo.Close();
            }
            formularioActivo = formulario;
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            formulario.BackColor = Color.MediumAquamarine;
            Contenedor.Controls.Add(formulario);
            formulario.Show();


        }

        //apertura de los formularios dependiendo cual se elija utilizando la funcion de arriba "openform"
        private void menuUsuarios_Click(object sender, EventArgs e)
        {
            OpenForm((IconMenuItem)sender, new FrmUsuarios());
        }

        
        private void subMenuServicio_Click(object sender, EventArgs e)
        {
            OpenForm(menuMantenedor, new FrmServicio());
        }

        private void subMenuNegocio_Click(object sender, EventArgs e)
        {
            OpenForm(menuMantenedor, new FrmNegocio());
        }


        private void subMenuResgistrarVenta_Click(object sender, EventArgs e)
        {
            OpenForm(menuVentas, new FrmVenta(usuarioActual));
        }

        private void SubMenuVerDetalleVenta_Click(object sender, EventArgs e)
        {
            OpenForm(menuVentas, new FrmDetalleVenta());
        }

        private void menuVentas_Click(object sender, EventArgs e)
        {

        }

        private void MenuClientes_Click(object sender, EventArgs e)
        {
           
        }
        private void subMenuCliente_Click(object sender, EventArgs e)
        {
            OpenForm(MenuClientes, new FrmClientes());
        }
        private void subMenuMascota_Click(object sender, EventArgs e)
        {
            OpenForm(MenuClientes, new FrmMascotas());
        }
        private void MenuReportes_Click(object sender, EventArgs e)
        {
            OpenForm((IconMenuItem)sender, new FrmReportes());
        }


        //******************************************************************************//


        private void btnsalir_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("¿Desea salir?","Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

    }
}

