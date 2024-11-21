using AppMonitoreo.Controller;
using AppMonitoreo.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppMonitoreo
{
    public partial class ConfiguracionUsuario : Form
    {
        public int IdUsuarioo;
        public string CeC;
        public string Nombre;
        public string Apellido;
        public string Movil;
        public string Email;
        public string IdEstado;
        public int IdEmpresa;
        public string Roll;
        public string Clavee;
        public string IdEstadoConfiguracion;
        public string IdRolConfiguracion;
        public string Passs;
        public string PascDes;
        public ConfiguracionUsuario()
        {
            InitializeComponent();
        }
        public void Imagen_load()
        {
            Thread.Sleep(3000);
        }
        private void AgregarEstado_Click(object sender, EventArgs e)
        {
            if (textDesEstado.Text == "")
            {
                MessageBox.Show("El Campo Cedula se encuentra vacio");
            }
            else
            {
                Estadoo JEstadoId = new Estadoo();
                JEstadoId.DescripEstado = textDesEstado.Text.Trim();
                int Resultado = ModelsBD.GuardarEstado(JEstadoId);
                if (Resultado > 0)
                {
                    MessageBox.Show("Se ha Registrado Correctamente", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ModelsBD ModelsConexion = new ModelsBD();
                    ModelsConexion.CargarEstado(TablaEstados);
                    textDesEstado.Text = "";
                }
                else if (Resultado == -1)
                {
                    MessageBox.Show("El Estado que Intenta Registrar ya existe en base de datos");
                }
            }
        }

        private void BtModificar_Click(object sender, EventArgs e)
        {
            if (textDesEstado.Text == "")
            {
                MessageBox.Show("Error", "El campo de Cedula es obligatorio", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (MessageBox.Show("Esta Seguro que desea Modificar los datos seleccionados", "Modificar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Estadoo JEstadoId = new Estadoo();
                JEstadoId.DescripEstado = textDesEstado.Text.Trim();
                JEstadoId.IdEstado = Convert.ToInt32(IdEstadoConfiguracion);
                int Resultado = ModelsBD.ModificarEstado(JEstadoId);
                if (Resultado > 0)
                {
                    MessageBox.Show("Actualizado", "Se Han Actualizado Los Datos Correctamente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ModelsBD ModelsConexion = new ModelsBD();
                    ModelsConexion.CargarEstado(TablaEstados);
                    textDesEstado.Text = "";
                }
                else
                {
                    MessageBox.Show("A Ocurrido un Error no se a logrado Actualizar los Datos del Usuario", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void BtEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Esta Seguro que desea Eliminar el Estado Seleccionado", "Modificar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Estadoo JEstadoId = new Estadoo();
                JEstadoId.IdEstado = Convert.ToInt32(IdEstadoConfiguracion);
                int resultado = ModelsBD.EliminarEstado(JEstadoId);
                if (resultado > 0)
                {
                    MessageBox.Show("Se Eliminado Correctamente el Estado", "Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textDesEstado.Text = "";
                    ModelsBD ModelsConexion = new ModelsBD();
                    ModelsConexion.CargarEstado(TablaEstados);
                }
            }
        }

        private void BtAgreRol_Click(object sender, EventArgs e)
        {
            if (textDesRol.Text == "")
            {
                MessageBox.Show("El Campo Cedula se encuentra vacio");
            }
            else
            {
                TipoRol JERolId = new TipoRol();
                JERolId.DescripRol = textDesRol.Text.Trim();
                int Resultado = ModelsBD.GuardarRol(JERolId);
                if (Resultado > 0)
                {
                    MessageBox.Show("Se ha Registrado Correctamente", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ModelsBD ModelsConexion = new ModelsBD();
                    ModelsConexion.CargarRol(TablaRol);
                    textDesRol.Text = "";
                }
                else if (Resultado == -1)
                {
                    MessageBox.Show("El Estado que Intenta Registrar ya existe en base de datos");
                }
            }
        }

        private void BtActuRol_Click(object sender, EventArgs e)
        {
            if (textDesRol.Text == "")
            {
                MessageBox.Show("Error", "El campo de Cedula es obligatorio", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (MessageBox.Show("Esta Seguro que desea Modificar los datos seleccionados", "Modificar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                TipoRol JRol = new TipoRol();
                JRol.DescripRol = textDesRol.Text.Trim();
                JRol.Id_Rol = Convert.ToInt32(IdRolConfiguracion);
                int Resultado = ModelsBD.ModificarRol(JRol);
                if (Resultado > 0)
                {
                    MessageBox.Show("Actualizado", "Se Han Actualizado Los Datos Correctamente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ModelsBD ModelsConexion = new ModelsBD();
                    ModelsConexion.CargarRol(TablaRol);
                    textDesRol.Text = "";
                }
                else
                {
                    MessageBox.Show("A Ocurrido un Error no se a logrado Actualizar los Datos del Usuario", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void BtElimiRol_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Esta Seguro que desea Eliminar el Estado Seleccionado", "Modificar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                TipoRol JRol = new TipoRol();
                JRol.Id_Rol = Convert.ToInt32(IdRolConfiguracion);
                int resultado = ModelsBD.EliminarRol(JRol);
                if (resultado > 0)
                {
                    MessageBox.Show("Se Eliminado Correctamente el Estado", "Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textDesRol.Text = "";
                    ModelsBD ModelsConexion = new ModelsBD();
                    ModelsConexion.CargarRol(TablaRol);
                }
            }
        }

        private async void ConfiguracionUsuario_Load(object sender, EventArgs e)
        {
            Task oTask = new Task(Imagen_load);
            ImagenLoad.Visible = true;
            oTask.Start();
            await oTask;
            Encrypt64 Encry = new Encrypt64();
            ModelsBD ModelsConexion = new ModelsBD();
            ModelsConexion.CargarEstado(TablaEstados);
            ModelsConexion.CargarRol(TablaRol);
            MySqlConnection Conne = Conn.ObteConnetion();
            SelectEmpresa.DropDownStyle = ComboBoxStyle.DropDownList;
            string query = "SELECT Id_Empresa,Nombre FROM Empresas order by Nombre";

            MySqlCommand cmd = new MySqlCommand(query, Conne);
            MySqlDataAdapter da1 = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da1.Fill(dt);
            SelectEmpresa.ValueMember = "Id_Empresa";
            SelectEmpresa.DisplayMember = "Nombre";
            SelectEmpresa.DataSource = dt;
            //----------------------------------------------------
            RolUsuario.DropDownStyle = ComboBoxStyle.DropDownList;
            string query2 = "SELECT * FROM Rol order by Descripcion";

            MySqlCommand cmd2 = new MySqlCommand(query2, Conne);
            MySqlDataAdapter da2 = new MySqlDataAdapter(cmd2);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            RolUsuario.ValueMember = "IdRol";
            RolUsuario.DisplayMember = "Descripcion";
            RolUsuario.DataSource = dt2;

            //----------------------------------------------------
            ListEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            string query3 = "SELECT * FROM Estado order by Descripcion";

            MySqlCommand cmd3 = new MySqlCommand(query3, Conne);
            MySqlDataAdapter da3 = new MySqlDataAdapter(cmd3);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            ListEstado.ValueMember = "IdEstado";
            ListEstado.DisplayMember = "Descripcion";
            ListEstado.DataSource = dt3;
            textNombre.Text = Nombre;
            textCC.Text = CeC;
            textApellido.Text = Apellido;
            textCelular.Text = Movil;
            textEmail.Text = Email;
            SelectEmpresa.Text = IdEmpresa.ToString();
            RolUsuario.Text = Roll;
            ListEstado.Text = ListEstado.ToString();
            textClave.Text = Clavee;
            PascDes = Encry.Desencriptar(Clavee);
            textClave.Text = PascDes;
            //Clavee = PascDes.ToString();
            ImagenLoad.Visible = false;
        }

        private void TablaEstados_MouseClick(object sender, MouseEventArgs e)
        {
            IdEstadoConfiguracion = TablaEstados.CurrentRow.Cells[0].Value.ToString();
            textDesEstado.Text = TablaEstados.CurrentRow.Cells[1].Value.ToString();
        }

        private void TablaRol_MouseClick(object sender, MouseEventArgs e)
        {
            IdRolConfiguracion = TablaRol.CurrentRow.Cells[0].Value.ToString();
            textDesRol.Text = TablaRol.CurrentRow.Cells[1].Value.ToString();
        }

        private void BtGuardar_Click(object sender, EventArgs e)
        {
            Encrypt64 Encry = new Encrypt64();
            if (textCC.Text == "")
            {
                MessageBox.Show("Error", "El campo de Cedula es obligatorio", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (MessageBox.Show("Esta Seguro que desea Modificar los datos seleccionados", "Modificar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DateTime Fecha = DateTime.Now;
                string FechaActual = Fecha.ToString("yyyy-MM-dd");
                Usuario JactUsuario = new Usuario();
                string Pasc = Encry.Encripta(textClave.Text);
                JactUsuario.Cedula = textCC.Text.Trim();
                JactUsuario.Nombre = textNombre.Text.Trim();
                JactUsuario.Apellido = textApellido.Text.Trim();
                JactUsuario.Rol = Convert.ToInt32(RolUsuario.SelectedValue.ToString());
                JactUsuario.IdEmpresa = SelectEmpresa.SelectedValue.ToString();
                JactUsuario.Movil = textCelular.Text.Trim();
                JactUsuario.Email = textEmail.Text.Trim();
                JactUsuario.Clave = Pasc;
                //JactUsuario.Huella = Encoding.UTF8.GetBytes(textHuella.Text.ToString());
                JactUsuario.IdEstado = Convert.ToInt32(ListEstado.SelectedValue.ToString());
                JactUsuario.Fecha_Actualizacion = FechaActual;

                int Resultado = ModelsBD.ModificarUsuario(JactUsuario);
                if (Resultado > 0)
                {
                    MessageBox.Show("Actualizado", "Se Han Actualizado Los Datos Correctamente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (ModelsBD.RefrescarActualizacion(IdUsuarioo, textCC.Text) > 0)
                    {
                        MenuAdmin menu1 = new MenuAdmin();
                        menu1.IdUsuariooo = ModelsBD.IdUsuarioo;
                        menu1.CeC = ModelsBD.CC;
                        menu1.Nombre = ModelsBD.Nombre;
                        menu1.Apellido = ModelsBD.Apellido;
                        menu1.Movil = ModelsBD.Movil;
                        menu1.Email = ModelsBD.Email;
                        menu1.IdEstado = ModelsBD.IdEstado;
                        menu1.IdEmpresa = ModelsBD.IdEmpresa;
                        menu1.Rool = ModelsBD.Rol_usu;
                        menu1.Clavv = ModelsBD.Clavee;
                    }
                    else
                    {
                        MessageBox.Show("El Usuario No se Encuentra Registrado");
                    }
                }
                else
                {
                    MessageBox.Show("No Se Pudo Actualizar Los Datos");
                }
            }
        }
    }
}
