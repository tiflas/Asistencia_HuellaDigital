using AppMonitoreo.Controller;
using AppMonitoreo.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppMonitoreo
{
    public partial class AgregarEmpresa : Form
    {
        public AgregarEmpresa()
        {
            InitializeComponent();
            textRut.TextChanged += new EventHandler(textRut_TextChanged);
            textNombreEmpresa.TextChanged += new EventHandler(textNombreEmpresa_TextChanged);
            textTelefonoEmpresa.TextChanged += new EventHandler(textTelefonoEmpresa_TextChanged);
            textDireccionEmpresa.TextChanged += new EventHandler(textDireccionEmpresa_TextChanged);
        }
        public void Imagen_load()
        {
            Thread.Sleep(3000);
        }
        private void BtBuscar_Click(object sender, EventArgs e)
        {
            if (textRut.Text == "")
            {
                textRut.Text = " ";
                ModelsBD ModelsConexion = new ModelsBD();
                ModelsConexion.BuscarEmpresa(TablaEmpresa, textRut.Text, textNombreEmpresa.Text);
                textRut.Text = "";
                textNombreEmpresa.Text = "";
                textTelefonoEmpresa.Text = "";
                textDireccionEmpresa.Text = "";
                textIdEmpresa.Text = "";
            }
            else
            {
                textNombreEmpresa.Text = " ";
                ModelsBD ModelsConexion = new ModelsBD();
                ModelsConexion.BuscarEmpresa(TablaEmpresa, textRut.Text, textNombreEmpresa.Text);
                textRut.Text = "";
                textNombreEmpresa.Text = "";
                textTelefonoEmpresa.Text = "";
                textDireccionEmpresa.Text = "";
                textIdEmpresa.Text = "";
            }
        }

        private void btModiUsu_Click(object sender, EventArgs e)
        {
            if (textRut.Text == "")
            {
                MessageBox.Show("Error", "El campo de Rut es obligatorio", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (MessageBox.Show("Esta Seguro que desea Modificar los datos seleccionados", "Modificar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DateTime Fecha = DateTime.Now;
                string FechaActual = Fecha.ToString("yyyy-MM-dd");

                Empresa JactEmpresa = new Empresa();
                JactEmpresa.IdEmpresa = Convert.ToInt32(textIdEmpresa.Text.Trim());
                JactEmpresa.RutEmpresa = textRut.Text.Trim();
                JactEmpresa.NombreEmpresa = textNombreEmpresa.Text.Trim();
                JactEmpresa.Telefono = textTelefonoEmpresa.Text.Trim();
                JactEmpresa.Direccion = textDireccionEmpresa.Text.Trim();
                JactEmpresa.Rol = Convert.ToInt32(IdRol.SelectedValue.ToString());
                JactEmpresa.IdEstado = Convert.ToInt32(IdEstadoEmpre.SelectedValue.ToString());
                JactEmpresa.FechaActualizacion = FechaActual;

                int Resultado = ModelsBD.ModificarEmpresa(JactEmpresa);
                if (Resultado > 0)
                {
                    MessageBox.Show("Actualizado", "Se Han Actualizado Los Datos Correctamente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ModelsBD ModelsConexion = new ModelsBD();
                    ModelsConexion.CargarEmpresa(TablaEmpresa);
                    textIdEmpresa.Text = "";
                    textRut.Text = "";
                    textNombreEmpresa.Text = "";
                    textDireccionEmpresa.Text = "";
                    textTelefonoEmpresa.Text = "";
                }
                else
                {
                    MessageBox.Show("No Se Pudo Actualizar Los Datos");
                }
            }
        }

        private async void BtGuardar_Click(object sender, EventArgs e)
        {
            MySqlConnection Conne = Conn.ObteConnetion();
            bool registroExiste = await VerificarExistenciaRegistroAsync();
            if (textRut.Text == "")
            {
                // El registro ya existe, mostrar un mensaje al usuario o realizar alguna acción
                MessageBox.Show("El Campo RUT de Empresa se encuentra vacio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (registroExiste)
            {
                MessageBox.Show("El Rut de la Empresa que has Ingresado ya se encuentra Registrado Porfavor Verificar");
            }
            else
            {
                Empresa JEmpresa = new Empresa();
                DateTime Fecha = DateTime.Now;
                string FechaActual = Fecha.ToString("yyyy-MM-dd");
                JEmpresa.RutEmpresa = textRut.Text.Trim();
                JEmpresa.NombreEmpresa = textNombreEmpresa.Text.Trim();
                JEmpresa.Telefono = textTelefonoEmpresa.Text.Trim();
                JEmpresa.Rol = Convert.ToInt32(IdRol.SelectedValue.ToString());
                JEmpresa.IdEstado = Convert.ToInt32(IdEstadoEmpre.SelectedValue.ToString());
                JEmpresa.Direccion = textDireccionEmpresa.Text.Trim();
                JEmpresa.FechaCreacion = FechaActual;
                int Resultado = ModelsBD.InsertEmpresa(JEmpresa);
                if (Resultado > 0)
                {
                    MessageBox.Show("Se ha Registrado Correctamente", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ModelsBD ModelsConexion = new ModelsBD();
                    ModelsConexion.CargarEmpresa(TablaEmpresa);
                    textIdEmpresa.Text = "";
                    textNombreEmpresa.Text = "";
                    textRut.Text = "";
                    textDireccionEmpresa.Text = "";
                    textTelefonoEmpresa.Text = "";
                }
                else if (Resultado == -1)
                {
                    MessageBox.Show("Error", "La Empresa que Intenta Registrar ya existe en base de datos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        private async Task<bool> VerificarExistenciaRegistroAsync()
        {
            string CC = textRut.Text;
            bool registroExiste = false;

            // Configura la cadena de conexión a tu base de datos MySQL Server
            MySqlConnection Conne = Conn.ObteConnetion();

            var query = "SELECT COUNT(*) FROM Empresas WHERE Rut = @Rut";
            MySqlCommand command = new MySqlCommand(query, Conne);
            command.Parameters.AddWithValue("@Rut", CC);

            // Ejecutar el comando y obtener el resultado escalar de tipo int
            int count = Convert.ToInt32(await command.ExecuteScalarAsync());
            //var count = await Conne.ExecuteScalarAsync<int>(query, new { email = emaill });
            if (count > 0)
            {
                // El registro ya existe en la base de datos
                registroExiste = true;
            }
            Conne.Close();
            return registroExiste;
        }

        private void BtExcel_Click(object sender, EventArgs e)
        {
            new ExportarExcel().ExportarAExcel(TablaEmpresa);
        }

        private async void AgregarEmpresa_Load(object sender, EventArgs e)
        {
            Task oTask = new Task(Imagen_load);
            ImagenLoad.Visible = true;
            oTask.Start();
            await oTask;
            MySqlConnection Conne = Conn.ObteConnetion();
            IdRol.DropDownStyle = ComboBoxStyle.DropDownList;
            string query2 = "SELECT IdRol,Descripcion FROM Rol order by Descripcion";

            MySqlCommand cmd2 = new MySqlCommand(query2, Conne);
            MySqlDataAdapter da2 = new MySqlDataAdapter(cmd2);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            IdRol.ValueMember = "IdRol";
            IdRol.DisplayMember = "Descripcion";
            IdRol.DataSource = dt2;

            //----------------------------------------------------
            IdEstadoEmpre.DropDownStyle = ComboBoxStyle.DropDownList;
            string query3 = "SELECT IdEstado,Descripcion FROM Estado order by Descripcion";

            MySqlCommand cmd3 = new MySqlCommand(query3, Conne);
            MySqlDataAdapter da3 = new MySqlDataAdapter(cmd3);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            IdEstadoEmpre.ValueMember = "IdEstado";
            IdEstadoEmpre.DisplayMember = "Descripcion";
            IdEstadoEmpre.DataSource = dt3;

            ModelsBD ModelsConexion = new ModelsBD();
            ModelsConexion.CargarEmpresa(TablaEmpresa);
            ImagenLoad.Visible = false;
        }

        private void TablaEmpresa_MouseClick(object sender, MouseEventArgs e)
        {
            textIdEmpresa.Text = TablaEmpresa.CurrentRow.Cells[0].Value.ToString();
            textRut.Text = TablaEmpresa.CurrentRow.Cells[1].Value.ToString();
            textNombreEmpresa.Text = TablaEmpresa.CurrentRow.Cells[2].Value.ToString();
            textTelefonoEmpresa.Text = TablaEmpresa.CurrentRow.Cells[3].Value.ToString();
            textDireccionEmpresa.Text = TablaEmpresa.CurrentRow.Cells[4].Value.ToString();
            IdEstadoEmpre.Text = TablaEmpresa.CurrentRow.Cells[5].Value.ToString();
            IdRol.Text = TablaEmpresa.CurrentRow.Cells[6].Value.ToString();
        }

        private void textRut_TextChanged(object sender, EventArgs e)
        {
            if (textRut.Text.Length > 25)
            {
                // Si el texto excede el límite, recórtalo y muestra un mensaje
                textRut.Text = textRut.Text.Substring(0, 25);
                textRut.SelectionStart = textRut.Text.Length;  // Mover el cursor al final
                MessageBox.Show($"Te has excedido el máximo de {25} caracteres permitidos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textNombreEmpresa_TextChanged(object sender, EventArgs e)
        {
            if (textNombreEmpresa.Text.Length > 30)
            {
                // Si el texto excede el límite, recórtalo y muestra un mensaje
                textNombreEmpresa.Text = textNombreEmpresa.Text.Substring(0, 30);
                textNombreEmpresa.SelectionStart = textNombreEmpresa.Text.Length;  // Mover el cursor al final
                MessageBox.Show($"Te has excedido el máximo de {30} caracteres permitidos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textTelefonoEmpresa_TextChanged(object sender, EventArgs e)
        {
            if (textTelefonoEmpresa.Text.Length > 20)
            {
                // Si el texto excede el límite, recórtalo y muestra un mensaje
                textTelefonoEmpresa.Text = textTelefonoEmpresa.Text.Substring(0, 20);
                textTelefonoEmpresa.SelectionStart = textTelefonoEmpresa.Text.Length;  // Mover el cursor al final
                MessageBox.Show($"Te has excedido el máximo de {20} caracteres permitidos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textDireccionEmpresa_TextChanged(object sender, EventArgs e)
        {
            if (textDireccionEmpresa.Text.Length > 30)
            {
                // Si el texto excede el límite, recórtalo y muestra un mensaje
                textDireccionEmpresa.Text = textDireccionEmpresa.Text.Substring(0, 30);
                textDireccionEmpresa.SelectionStart = textDireccionEmpresa.Text.Length;  // Mover el cursor al final
                MessageBox.Show($"Te has excedido el máximo de {30} caracteres permitidos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
