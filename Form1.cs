using AppMonitoreo.Controller;
using AppMonitoreo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Security.Policy;

namespace AppMonitoreo
{
    public partial class Form1 : Form
    {
        string Cname = Dns.GetHostName();
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Visible = false;
            //this.Load += new EventHandler(Form1_Load);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmVerificar Veerificar = new FrmVerificar();
            Veerificar.Show();
        }

        private void Usuariotxtb_Leave(object sender, EventArgs e)
        {
            if (Usuariotxtb.Text == "")
            {
                Usuariotxtb.Text = "USUARIO";
                Usuariotxtb.ForeColor = Color.DimGray;
            }
        }

        private void Usuariotxtb_Enter(object sender, EventArgs e)
        {
            if (Usuariotxtb.Text == "USUARIO")
            {
                Usuariotxtb.Text = "";
                Usuariotxtb.ForeColor = Color.LightGray;
            }
        }

        private void Clavetxtb_Leave(object sender, EventArgs e)
        {
            if (Clavetxtb.Text == "")
            {
                Clavetxtb.Text = "CLAVE";
                Clavetxtb.ForeColor = Color.DimGray;
            }
        }

        private void Clavetxtb_Enter(object sender, EventArgs e)
        {
            if (Clavetxtb.Text == "CLAVE")
            {
                Clavetxtb.Text = "";
                Clavetxtb.ForeColor = Color.LightGray;
            }
        }

        private void BtonIngresar_Click(object sender, EventArgs e)
        {
            if (Usuariotxtb.Text == "" || Clavetxtb.Text == "")
            {
                MessageBox.Show("Hay campos vacios");
            }
            else
            {
                Encrypt64 Decry = new Encrypt64();
                string password = Clavetxtb.Text;
                string Pasc = Decry.Encripta(password);
                if (ModelsBD.Auntentificar(Usuariotxtb.Text, Pasc) > 0)
                {
                    if (ModelsBD.Rol_usu == "Administrador")
                    {
                        MenuAdmin menu1 = new MenuAdmin();
                        menu1.Show();
                        menu1.IdUsuariooo = ModelsBD.IdUsuarioo;
                        menu1.Nombrelabel.Text = ModelsBD.Nombre;
                        menu1.CeC = ModelsBD.CC;
                        menu1.Nombre = ModelsBD.Nombre;
                        menu1.Apellido = ModelsBD.Apellido;
                        menu1.Movil = ModelsBD.Movil;
                        menu1.Email = ModelsBD.Email;
                        menu1.IdEstado = ModelsBD.IdEstado;
                        menu1.IdEmpresa = ModelsBD.IdEmpresa;
                        menu1.Clavv = ModelsBD.Clavee;
                        this.Hide();
                    }
                    else
                    {
                        RegistroUsuario menu2 = new RegistroUsuario();
                        menu2.Show();
                        /*menu2.CClabel.Text = ModelsBD.CC;
                        menu2.Nombrelabel.Text = ModelsBD.Nombre;
                        menu2.Apellidolabel.Text = ModelsBD.Apellido;
                        menu2.Empresalabel.Text = ModelsBD.NombreEmpresa;
                        menu2.Movillabel.Text = ModelsBD.Movil;
                        menu2.Emaillabel.Text = ModelsBD.Email;
                        menu2.Estadolabel.Text = ModelsBD.IdEstado;
                        menu2.IdEmpresa = ModelsBD.IdEmpresa;*/
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("El Usuario No se Encuentra Registrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registrar RegistrarForm = new Registrar();
            RegistrarForm.Show();
            this.Hide();
        }

        private void Clavetxtb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Clavetxtb.Text == "CLAVE")
            {
                Clavetxtb.Text = "CLAVE";
            }
            else
            {
                Clavetxtb.PasswordChar = '•';
            }
        }

        private void Clavetxtb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Evitar el sonido de la tecla Enter
                e.SuppressKeyPress = true;

                // Llamar al evento del botón como si se hubiera hecho clic en él
                BtonIngresar.PerformClick();
            }
        }
        private void ObtenerDireccionIP()
        {
            bool registroExiste = VerifiUsuCOM();
            if (!registroExiste)
            {
                // Obtén el nombre del host local
                string hostName = Dns.GetHostName();
                //IPAddress[] addresses = Dns.GetHostAddresses(hostName);
                MySqlConnection Conne = Conn.ObteConnetion();
                // Define la consulta SQL para insertar un nuevo registro
                //-------------------------------
                DateTime localDateTime = DateTime.Now;
                // Define el identificador de la zona horaria de Bogotá, Colombia
                string bogotaTimeZoneId = "SA Pacific Standard Time"; // Zona horaria para Bogotá, Colombia
                TimeZoneInfo bogotaTimeZone = TimeZoneInfo.FindSystemTimeZoneById(bogotaTimeZoneId);
                // Convierte la hora local a la hora en la zona horaria de Bogotá
                DateTime bogotaDateTime = TimeZoneInfo.ConvertTime(localDateTime, bogotaTimeZone);
                string HoraIngresoUsu = bogotaDateTime.ToString("HH:mm:ss");
                string FechaActual = bogotaDateTime.ToString("yyyy-MM-dd");
                var query = "INSERT INTO IpComp (EquipoCompu,Fecha_Ejecucion,Hora) VALUES (@DescriCOM,@Fecha_Ejecucion,@HoraEjecu)";
                MySqlCommand command = new MySqlCommand(query, Conne);
                command.Parameters.AddWithValue("@DescriCOM", hostName);
                command.Parameters.AddWithValue("@Fecha_Ejecucion", FechaActual);
                command.Parameters.AddWithValue("@HoraEjecu", HoraIngresoUsu);
                command.ExecuteNonQuery();
                Conne.Close();//
            }
            PerformVerification();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ObtenerDireccionIP();
            VerifiUsuCOM();
            UsuariosID();
        }

        private bool VerifiUsuCOM()
        {
            string ComNa = Cname;
            bool registroExiste = false;

            MySqlConnection Conne = Conn.ObteConnetion();

            var query = "SELECT COUNT(*) FROM IpComp WHERE EquipoCompu = @CoM";
            MySqlCommand command = new MySqlCommand(query, Conne);
            command.Parameters.AddWithValue("@CoM", ComNa);
            int count = Convert.ToInt32(command.ExecuteScalar());
            if (count > 0)
            {
                // El registro ya existe en la base de datos
                registroExiste = true;
            }
            Conne.Close();
            return registroExiste;
        }

        private void PerformVerification()
        {
            string hostName = Dns.GetHostName();


            MySqlConnection Conne = Conn.ObteConnetion();
            string query = "SELECT EquipoCompu FROM IpComp LIMIT 3;"; // Asegúrate de adaptar la consulta a tu base de datos.


            bool result = CheckVariableInQueryResult(Conne, query, hostName);

            // Muestra u oculta la imagen basada en el resultado
            pictureBox1.Visible = !result;

        }
        private bool CheckVariableInQueryResult(MySqlConnection connection, string query, string variable)
        {
            try
            {
                // Usamos la conexión existente
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string equipoCompu = reader["EquipoCompu"].ToString();
                            if (equipoCompu == variable)
                            {
                                return true; // La variable está en la base de datos
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error: {ex.Message}");
            }
            finally
            {
                // Asegúrate de cerrar la conexión
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return false; // La variable no está en la base de datos
        }
        private static void UsuariosID()
        {
            var userCompanyList = new List<(string Cedula, int idEmpresa)>();

            // Obtener la conexión
            using (var Conne = Conn.ObteConnetion())
            {
                string query = "SELECT Cedula, Id_Empresa FROM usuario ORDER BY id_usuario";
                using (var command = new MySqlCommand(query, Conne))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string Cedula = reader.GetString(0);
                        int idEmpresa = reader.GetInt32(1);
                        userCompanyList.Add((Cedula, idEmpresa));
                    }
                }
            }
            //Ahora que tenemos la lista, podemos proceder con la actualización
            UpdateCompanyIds(userCompanyList);
        }
        private static void UpdateCompanyIds(List<(string Cedula, int idEmpresa)> userCompanyList)
        {
            using (var Conne = Conn.ObteConnetion())
            {
                using (var transaction = Conne.BeginTransaction())
                {
                    try
                    {
                        string updateQuery = "UPDATE RegistroMonitoreo SET Id_Empresa = @Id_Empresa WHERE CC_usuario = @CC_usuario";

                        foreach (var (Cedula, idEmpresa) in userCompanyList)
                        {
                            using (var command = new MySqlCommand(updateQuery, Conne, transaction))
                            {
                                command.Parameters.AddWithValue("@Id_Empresa", idEmpresa);
                                command.Parameters.AddWithValue("@CC_usuario", Cedula);

                                int rowsAffected = command.ExecuteNonQuery();
                                Console.WriteLine($"Updated rows: {rowsAffected} for Usuario: {Cedula}, Empresa: {idEmpresa}");
                            }
                        }

                        transaction.Commit();  // Confirmar la transacción si todo ha ido bien
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();  // Deshacer cambios en caso de error
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }
    }
}
