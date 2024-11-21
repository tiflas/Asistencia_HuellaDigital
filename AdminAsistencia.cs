using AppMonitoreo.Controller;
using AppMonitoreo.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppMonitoreo
{
    public partial class AdminAsistencia : Form
    {
        void AbrirFechaEmpre()
        {
            BtEmpreFecha.Show();
            BtBuscarFecha.Hide();
            FechaInicio.Show();
            FechaCierre.Show();
            label6.Show();
            label7.Show();
            BtBuscar.Hide();
        }
        void AbrirFecha()
        {
            BtBuscarFecha.Show();
            FechaInicio.Show();
            FechaCierre.Show();
            label6.Show();
            label7.Show();
            BtBuscar.Hide();
            BtEmpreFecha.Hide();
        }

        void OcultarFecha()
        {
            BtBuscarFecha.Hide();
            FechaInicio.Hide();
            FechaCierre.Hide();
            label6.Hide();
            label7.Hide();
            BtBuscar.Show();
            BtEmpreFecha.Hide();
        }
        public AdminAsistencia()
        {
            InitializeComponent();
        }
        public void Imagen_load()
        {
            Thread.Sleep(3000);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            FrmVerificar Veerificar = new FrmVerificar();
            Veerificar.Show();
        }

        private void BtEmpreFecha_Click(object sender, EventArgs e)
        {
            DateTime FechaInicioo = new DateTime(FechaInicio.Value.Year, FechaInicio.Value.Month, FechaInicio.Value.Day);
            DateTime FechaCierree = new DateTime(FechaCierre.Value.Year, FechaCierre.Value.Month, FechaCierre.Value.Day);
            string Dato1 = (FechaInicioo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            string Dato2 = (FechaCierree.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            int Id_empresa = Convert.ToInt32(SelectEmpresa.SelectedValue.ToString());
            ModelsBD ModelsConexion = new ModelsBD();
            ModelsConexion.BuscarAsistenciaFechaEmpresa(TablaAsistencia, Dato1, Dato2, Id_empresa);
            textCedula.Text = "";
            textNombre.Text = "";
            textApellido.Text = "";
            textCelular.Text = "";
            textEmail.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OcultarFecha();
            if (textCedula.Text == "")
            {
                textCedula.Text = " ";
                ModelsBD ModelsConexion = new ModelsBD();
                ModelsConexion.BuscarUsuario(TablaAsistencia, textCedula.Text, textNombre.Text);
                textCedula.Text = "";
                textNombre.Text = "";
                textApellido.Text = "";
                textCelular.Text = "";
                textEmail.Text = "";

            }
            else
            {
                textNombre.Text = " ";
                ModelsBD ModelsConexion = new ModelsBD();
                ModelsConexion.BuscarUsuario(TablaAsistencia, textCedula.Text, textNombre.Text);
                textCedula.Text = "";
                textNombre.Text = "";
                textApellido.Text = "";
                textCelular.Text = "";
                textEmail.Text = "";
                //TablaUsuarios.DataSource = ModelsBD.BuscarUsuario(textCedula.Text);
            }
        }

        private void BtExcel_Click(object sender, EventArgs e)
        {
            new ExportarExcel().ExportarAExcel(TablaAsistencia);
        }

        private async void BtEntrada_Click(object sender, EventArgs e)
        {
            bool registroExiste = await VerificarCedula();
            if (textCedula.Text == "")
            {
                MessageBox.Show("El campo de Cedula es obligatorio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (registroExiste)
            {
                DateTime localDateTime = DateTime.Now;
                // Define el identificador de la zona horaria de Bogotá, Colombia
                string bogotaTimeZoneId = "SA Pacific Standard Time"; // Zona horaria para Bogotá, Colombia
                TimeZoneInfo bogotaTimeZone = TimeZoneInfo.FindSystemTimeZoneById(bogotaTimeZoneId);
                // Convierte la hora local a la hora en la zona horaria de Bogotá
                DateTime bogotaDateTime = TimeZoneInfo.ConvertTime(localDateTime, bogotaTimeZone);
                string HoraIngresoUsu = bogotaDateTime.ToString("HH:mm:ss");
                string FechaActual = bogotaDateTime.ToString("yyyy-MM-dd");
                string cedulaa = textCedula.Text;
                string nombre = textNombre.Text;
                RegistroMonitoreo RegistroUsuario = new RegistroMonitoreo();
                RegistroUsuario.Id_usuario = textCedula.Text;
                RegistroUsuario.Id_empresa = Convert.ToInt32(SelectEmpresa.SelectedValue.ToString());
                RegistroUsuario.FechaEntrada = FechaActual;
                RegistroUsuario.TiempoIngreso = HoraIngresoUsu;
                long CountRegistro = ModelsBD.VerificarRegistro(RegistroUsuario);
                if (CountRegistro <= 0)
                {
                    int Resultado = ModelsBD.GuardarRegistro(RegistroUsuario);
                    if (Resultado > 0)
                    {
                        MessageBox.Show("Se ha Guardado correctamente el Registro De Ingreso de Cedula:"+ cedulaa+ "\nNombre:"+ nombre + "\nHora Ingreso:"+ HoraIngresoUsu + "", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ModelsBD ModelsConexion = new ModelsBD();
                        ModelsConexion.CargarAsistencia(TablaAsistencia);
                        textCedula.Text = "";
                        textNombre.Text = "";
                        textApellido.Text = "";
                        textCelular.Text = "";
                        textEmail.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Ya se encuentra Registrado El Ingreso con la Cedula: " + cedulaa + "\nNombre: " + nombre + "", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }  
            }
            else
            {
                MessageBox.Show("La Cedula que ingresaste no se Encuentra Registrada en el sistema");
            }
        }

        private void BtBuscar_Click(object sender, EventArgs e)
        {
            if (textCedula.Text == "")
            {
                textCedula.Text = " ";
                ModelsBD ModelsConexion = new ModelsBD();
                ModelsConexion.BuscarUsuarioAsistencia(TablaAsistencia, textCedula.Text, textNombre.Text);
                textCedula.Text = "";
                textNombre.Text = "";
                textApellido.Text = "";
                textCelular.Text = "";
                textEmail.Text = "";
            }
            else
            {
                textNombre.Text = " ";
                ModelsBD ModelsConexion = new ModelsBD();
                ModelsConexion.BuscarUsuarioAsistencia(TablaAsistencia, textCedula.Text, textNombre.Text);
                textCedula.Text = "";
                textNombre.Text = "";
                textApellido.Text = "";
                textCelular.Text = "";
                textEmail.Text = "";
            }
        }

        private void BtBuscarFecha_Click(object sender, EventArgs e)
        {
            DateTime FechaInicioo = new DateTime(FechaInicio.Value.Year, FechaInicio.Value.Month, FechaInicio.Value.Day);
            DateTime FechaCierree = new DateTime(FechaCierre.Value.Year, FechaCierre.Value.Month, FechaCierre.Value.Day);
            string Dato1 = (FechaInicioo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            string Dato2 = (FechaCierree.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            ModelsBD ModelsConexion = new ModelsBD();
            ModelsConexion.BuscarAsistenciaFecha(TablaAsistencia, Dato1, Dato2);
            textCedula.Text = "";
            textNombre.Text = "";
            textApellido.Text = "";
            textCelular.Text = "";
            textEmail.Text = "";
        }
        private async Task<bool> VerificarCedula()
        {
            string CC = textCedula.Text;
            bool registroExiste = false;

            // Configura la cadena de conexión a tu base de datos MySQL Server
            MySqlConnection Conne = Conn.ObteConnetion();

            var query = "SELECT COUNT(*) FROM usuario WHERE Cedula = @Cedu";
            MySqlCommand command = new MySqlCommand(query, Conne);
            command.Parameters.AddWithValue("@Cedu", CC);

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

        private void RadioUsuario_CheckedChanged(object sender, EventArgs e)
        {
            OcultarFecha();
        }

        private void RadioFecha_CheckedChanged(object sender, EventArgs e)
        {
            AbrirFecha();
        }

        private void RadioEmpreFecha_CheckedChanged(object sender, EventArgs e)
        {
            AbrirFechaEmpre();
        }

        private async void AdminAsistencia_Load(object sender, EventArgs e)
        {
            Task oTask = new Task(Imagen_load);
            ImagenLoad.Visible = true;
            oTask.Start();
            await oTask;
            BtBuscarFecha.Hide();
            BtEmpreFecha.Hide();
            FechaInicio.Hide();
            FechaCierre.Hide();
            label6.Hide();
            label7.Hide();
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
            ImagenLoad.Visible = false;
        }

        private void TablaAsistencia_MouseClick(object sender, MouseEventArgs e)
        {
            textCedula.Text = TablaAsistencia.CurrentRow.Cells[0].Value.ToString();
            textNombre.Text = TablaAsistencia.CurrentRow.Cells[1].Value.ToString();
            textApellido.Text = TablaAsistencia.CurrentRow.Cells[2].Value.ToString();
            textEmail.Text = TablaAsistencia.CurrentRow.Cells[3].Value.ToString();
            textCelular.Text = TablaAsistencia.CurrentRow.Cells[4].Value.ToString();
            SelectEmpresa.Text = TablaAsistencia.CurrentRow.Cells[5].Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textCedula.Text == "")
            {
                MessageBox.Show("El campo de Cedula es obligatorio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                DateTime localDateTime = DateTime.Now;
                // Define el identificador de la zona horaria de Bogotá, Colombia
                string bogotaTimeZoneId = "SA Pacific Standard Time"; // Zona horaria para Bogotá, Colombia
                TimeZoneInfo bogotaTimeZone = TimeZoneInfo.FindSystemTimeZoneById(bogotaTimeZoneId);
                // Convierte la hora local a la hora en la zona horaria de Bogotá
                DateTime bogotaDateTime = TimeZoneInfo.ConvertTime(localDateTime, bogotaTimeZone);
                string HoraIngresoUsu = bogotaDateTime.ToString("HH:mm:ss");
                string FechaActual = bogotaDateTime.ToString("yyyy-MM-dd");
                RegistroMonitoreo RegistroUsuario = new RegistroMonitoreo();
                RegistroUsuario.Id_usuario = textCedula.Text;
                RegistroUsuario.Id_empresa = Convert.ToInt32(SelectEmpresa.SelectedValue.ToString());
                RegistroUsuario.FechaEntrada = FechaActual;
                RegistroUsuario.TiempoIngreso = HoraIngresoUsu;
                string cedulaa = textCedula.Text;
                string nombre = textNombre.Text;
                long CountRegistroTotal = ModelsBD.VerificarRegistroSalida(RegistroUsuario);
                if (CountRegistroTotal <= 0)
                {
                    string HoraIngresoHoy = ModelsBD.VerificarHoraIngreso(RegistroUsuario);
                    DateTime AprobarHora = DateTime.ParseExact(HoraIngresoHoy, "HH:mm:ss", null);
                    DateTime HoraIngresoVerificar = DateTime.ParseExact(HoraIngresoUsu, "HH:mm:ss", null);
                    // Adelantar una hora
                    DateTime VerificarHora = AprobarHora.AddHours(1);
                    if(HoraIngresoVerificar >= VerificarHora)
                    {
                        RegistroUsuario.Id_usuario = textCedula.Text;
                        RegistroUsuario.Id_empresa = Convert.ToInt32(SelectEmpresa.SelectedValue.ToString());
                        RegistroUsuario.FechaEntrada = FechaActual;
                        RegistroUsuario.TiempoSalida = HoraIngresoUsu;
                        int Resultado = ModelsBD.GuardarRegistroSalida(RegistroUsuario);
                        if (Resultado > 0)
                        {
                            MessageBox.Show("Se ah Guardado correctamente el Registro De Salida Cedula:" + cedulaa + "\nNombre:" + nombre + "\nHora Salida:" + HoraIngresoUsu + "", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ModelsBD ModelsConexion = new ModelsBD();
                            ModelsConexion.CargarAsistencia(TablaAsistencia);
                            textCedula.Text = "";
                            textNombre.Text = "";
                            textApellido.Text = "";
                            textCelular.Text = "";
                            textEmail.Text = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Solo puedes Guardar la salida luego de una hora de Ingreso", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("Ya se encuentra Registrada la salida con el usuario Cedula:" + cedulaa + "\nNombre:" + nombre + "", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
    }
}
