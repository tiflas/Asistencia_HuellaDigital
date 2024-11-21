using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppMonitoreo.Models;
using MySql.Data.MySqlClient;
using AppMonitoreo.Controller;
using System.IO;

namespace AppMonitoreo
{
    public partial class FrmVerificar : CaptureForm
    {
        private DPFP.Template Template;
        private DPFP.Verification.Verification Verificator;

        public void Verify(DPFP.Template template)
        {
            Template = template;
            ShowDialog();
        }
        protected override void Init()
        {
            base.Init();
            base.Text = "Verificación de Huella Digital";
            Verificator = new DPFP.Verification.Verification();     // Create a fingerprint template verificator
            UpdateStatus(0);
        }

        private void UpdateStatus(int FAR)
        {
            // Show "False accept rate" value
            SetStatus(String.Format("False Accept Rate (FAR) = {0}", FAR));
        }

        protected override void Process(DPFP.Sample Sample)
        {
            base.Process(Sample);

            DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);

            if (features != null)
            {
                DPFP.Verification.Verification.Result result = new DPFP.Verification.Verification.Result();

                foreach (var Emp in ModelsBD.VerificarHuella())
                {
                    try
                    {
                        byte[] huellaBytes;

                        // Decodificar Base64 a bytes
                        huellaBytes = Convert.FromBase64String(Emp.HuellaDigi);

                        using (var stream = new MemoryStream(huellaBytes))
                        {
                            DPFP.Template template = new DPFP.Template(stream);

                            // Manejo de errores en la verificación
                            try
                            {
                                Verificator.Verify(features, template, ref result);
                                UpdateStatus(result.FARAchieved);

                                if (result.Verified)
                                {
                                    MakeReport($"La huella dactilar fue VERIFICADA.\r\nC.C: {Emp.Cedula}\r\nNombre : {Emp.Nombre}\r\nApellidos : {Emp.Apellido}");

                                    DateTime localDateTime = DateTime.Now;
                                    string bogotaTimeZoneId = "SA Pacific Standard Time";
                                    TimeZoneInfo bogotaTimeZone = TimeZoneInfo.FindSystemTimeZoneById(bogotaTimeZoneId);
                                    DateTime bogotaDateTime = TimeZoneInfo.ConvertTime(localDateTime, bogotaTimeZone);
                                    string HoraIngresoUsu = bogotaDateTime.ToString("HH:mm:ss");
                                    string FechaActual = bogotaDateTime.ToString("yyyy-MM-dd");

                                    RegistroMonitoreo RegistroUsuario = new RegistroMonitoreo
                                    {
                                        Id_usuario = Emp.Cedula,
                                        Id_empresa = Emp.IdEmpresa,
                                        FechaEntrada = FechaActual,
                                        TiempoIngreso = HoraIngresoUsu
                                    };

                                    long CountRegistro = ModelsBD.VerificarRegistro(RegistroUsuario);
                                    if (CountRegistro <= 0)
                                    {
                                        int Resultado = ModelsBD.GuardarRegistro(RegistroUsuario);
                                        if (Resultado > 0)
                                        {
                                            MessageBox.Show("Se ha guardado correctamente el registro de ingreso", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                    else
                                    {
                                        long CountRegistroTotal = ModelsBD.VerificarRegistroSalida(RegistroUsuario);
                                        if (CountRegistroTotal <= 0)
                                        {
                                            string HoraIngresoHoy = ModelsBD.VerificarHoraIngreso(RegistroUsuario);
                                            DateTime AprobarHora = DateTime.ParseExact(HoraIngresoHoy, "HH:mm:ss", null); // convertimos la variable string en date time
                                            DateTime HoraIngresoVerificar = DateTime.ParseExact(HoraIngresoUsu, "HH:mm:ss", null);
                                            // Adelantar una hora
                                            DateTime VerificarHora = AprobarHora.AddHours(1);
                                            if (HoraIngresoVerificar >= VerificarHora)// Se verifica que la hora actual sea mayor a la hora guardada de ingreso del usuario
                                            {
                                                RegistroUsuario.TiempoSalida = HoraIngresoUsu;
                                                int Resultado = ModelsBD.GuardarRegistroSalida(RegistroUsuario);
                                                if (Resultado > 0)
                                                {
                                                    MessageBox.Show("Se ha guardado correctamente el registro de salida", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Solo puedes Guardar la salida luego de una hora de Ingreso", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }     
                                        }
                                        else
                                        {
                                            MessageBox.Show("Ya se encuentra registrada la salida con el usuario que intenta ingresar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        }
                                    }
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error durante la verificación: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al cargar la huella digital desde el almacenamiento: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("No se pudieron extraer las características de la muestra", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public FrmVerificar()
        {
            MySqlConnection Conne = Conn.ObteConnetion();
            InitializeComponent();
        }
    }
}
