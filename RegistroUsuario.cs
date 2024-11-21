using AppMonitoreo.Controller;
using AppMonitoreo.Models;
using MySql.Data.MySqlClient;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppMonitoreo
{
    public partial class RegistroUsuario : Form
    {
        public int IdEmpresa;
        public RegistroUsuario()
        {
            InitializeComponent();
        }

        private void BtBuscar_Click(object sender, EventArgs e)
        {

        }
        private void RegistroUsuario_Load(object sender, EventArgs e)
        {
            
        }

        private void BtIngreso_Click(object sender, EventArgs e)
        {
            DateTime Fecha = DateTime.Now;
            DateTime HoraIngreso = DateTime.Now;
            DateTime localDateTime = DateTime.Now;
            // Define el identificador de la zona horaria de Bogotá, Colombia
            string bogotaTimeZoneId = "SA Pacific Standard Time"; // Zona horaria para Bogotá, Colombia
                                                                  // Obtén la información de la zona horaria
            TimeZoneInfo bogotaTimeZone = TimeZoneInfo.FindSystemTimeZoneById(bogotaTimeZoneId);
            // Convierte la hora local a la hora en la zona horaria de Bogotá
            DateTime bogotaDateTime = TimeZoneInfo.ConvertTime(localDateTime, bogotaTimeZone);
            string HoraIngresoUsu = bogotaDateTime.ToString("HH:mm:ss");
            string FechaActual = bogotaDateTime.ToString("yyyy-MM-dd");

            RegistroMonitoreo RegistroUsuario = new RegistroMonitoreo();
            RegistroUsuario.Id_usuario = CClabel.Text;
            RegistroUsuario.Id_empresa = IdEmpresa;
            RegistroUsuario.FechaEntrada = FechaActual;
            RegistroUsuario.TiempoIngreso = HoraIngresoUsu;
            long CountRegistro = ModelsBD.VerificarRegistro(RegistroUsuario);
            if (CountRegistro <= 0)
            {
                int Resultado = ModelsBD.GuardarRegistro(RegistroUsuario);
                if (Resultado > 0)
                {
                    MessageBox.Show("Se ah Guardado correctamente el Registro De Ingreso", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Form1 Inicio = new Form1();
                    Inicio.Show();
                    this.Hide();
                }
            }
            else
            {
                long CountRegistroTotal = ModelsBD.VerificarRegistroSalida(RegistroUsuario);
                if (CountRegistroTotal <= 0)
                {
                    RegistroUsuario.Id_usuario = CClabel.Text;
                    RegistroUsuario.Id_empresa = IdEmpresa;
                    RegistroUsuario.FechaEntrada = FechaActual;
                    RegistroUsuario.TiempoSalida = HoraIngresoUsu;
                    int Resultado = ModelsBD.GuardarRegistroSalida(RegistroUsuario);
                    if (Resultado > 0)
                    {
                        MessageBox.Show("Se ah Guardado correctamente el Registro De Salida", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Form1 Inicio = new Form1();
                        Inicio.Show();
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Ya se encuentra Registrada la salida con el usuario que Intenta Ingresar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Form1 Inicio = new Form1();
                    Inicio.Show();
                    this.Hide();
                }
            }
        }
    }
}
