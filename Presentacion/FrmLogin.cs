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
using Logica;
using Datos;


namespace Presentacion
{
    public partial class FrmLogin : Form
    {
        //instancia con la capa logica conectado al servicio ususario(SrvUsuarios)
        
        ServicioUsuario srvUsuarios = new ServicioUsuario(ConfigConnection.ConnectionString);
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            //creacion de variable para almacenar usuarios, llama un metodo de la capa logica para obtenerlos y de paso comprueba los parametro para el inicio de sesión//
            var usuario = srvUsuarios.MostrarUsuarios().Where(u=> u.Documento == txtDocumento.Text && u.Clave == txtContraseña.Text).FirstOrDefault();

            if (usuario != null)
            {
                frmopen(usuario);
            }
            else
            {
                MessageBox.Show("No se encontro el usuario", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }
        //metodo para abir el formulario de inicio(FrmInicio)//
        public void frmopen(Usuarios usuario)
        {
            //se manda el usuario que inicia sesión al formulario de inicio//
            FrmInicio form = new FrmInicio(usuario);
            form.Show();
            this.Hide();
            //se le atribuye el metodo frmclosing al FormClosing para que cuando cierre el formulario de inicio tambien haga lo que esta en el metodo frmclosing//
            form.FormClosing += frmclosing;
        }
        //metodo para mostrar el de nuevo el formulario de login cuando se cierre el formulario de inicio y limpia los campos de los txt de login//
        private void frmclosing(object sender, FormClosingEventArgs e)
        {
            txtContraseña.Text = "";
            txtDocumento.Text = "";
            this.Show();
        }

      
    }
}
