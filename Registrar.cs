using AppMonitoreo.Controller;
using AppMonitoreo.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppMonitoreo
{
    public partial class Registrar : Form
    {
        private DPFP.Template Template;
        public Registrar()
        {
            InitializeComponent();
        }

        private async void buttGuardar_Click(object sender, EventArgs e)
        {
            MySqlConnection Conne = Conn.ObteConnetion();
            Encrypt64 Encry = new Encrypt64();
            string password = textBClave.Text;
            string confirmPassword = textBClaveConfi.Text;
            if (textBCC.Text == "" || textBNombre.Text == "" || password == "" || confirmPassword == "")
            {
                MessageBox.Show("Hay campos vacios porfavor verificar");
            }
            else if (password == confirmPassword)
            {
                bool registroExiste = await VerificarExistenciaRegistroAsync();
                if (registroExiste)
                {
                    // El registro ya existe, mostrar un mensaje al usuario o realizar alguna acción
                    MessageBox.Show("La cedula que has Ingresado ya se encuentra Registrada Porfavor Verificar");
                }
                else
                {
                    Usuario Jusuario = new Usuario();
                    DateTime Fecha = DateTime.Now;
                    string FechaActual = Fecha.ToString("yyyy-MM-dd");
                    byte[] StremeHuella = null;
                    string Pasc = Encry.Encripta(textBClave.Text.Trim());
                    Jusuario.Cedula = textBCC.Text.Trim();
                    Jusuario.Nombre = textBNombre.Text.Trim();
                    Jusuario.Apellido = textBApellido.Text.Trim();
                    Jusuario.Rol = 3;
                    Jusuario.IdEmpresa = listaempre.SelectedValue.ToString();
                    Jusuario.Movil = textBCelular.Text.Trim();
                    Jusuario.Email = textBEmail.Text.Trim();
                    Jusuario.Clave = Pasc;
                    Jusuario.Fecha_Creacion = FechaActual;
                    Jusuario.IdEstado = 1;
                    if (Template != null)
                    {
                        StremeHuella = Template.Bytes;
                        string codigoTexto = Convert.ToBase64String(StremeHuella);
                        Jusuario.Huella = codigoTexto;
                        int Resultado = ModelsBD.InsertUsu(Jusuario);
                        if (Resultado > 0)
                        {
                            MessageBox.Show("Se ha Registrado Correctamente", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //model.cargarproducto(tablaproducto);
                            textBCC.Text = "";
                            textBNombre.Text = "";
                            textBApellido.Text = "";
                            textBCelular.Text = "";
                            textBEmail.Text = "";
                            textBClave.Text = "";
                            textBClaveConfi.Text = "";
                            Template = null;
                        }
                        else if (Resultado == -1)
                        {
                            MessageBox.Show("El usuario que intenta registrar ya existe en base de datos");
                        }
                    }
                    else
                    {
                        int Resultado = ModelsBD.InsertUsu(Jusuario);
                        if (Resultado > 0)
                        {
                            MessageBox.Show("Se ha Registrado Correctamente", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //model.cargarproducto(tablaproducto);
                            textBCC.Text = "";
                            textBNombre.Text = "";
                            textBApellido.Text = "";
                            textBCelular.Text = "";
                            textBEmail.Text = "";
                            textBClave.Text = "";
                            textBClaveConfi.Text = "";
                            Template = null;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Las Contraseñas no coinciden", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void BtSalirRegis_Click(object sender, EventArgs e)
        {
            Form1 InicioSesion = new Form1();
            InicioSesion.Show();
            this.Hide();
        }

        private void Registrar_Load(object sender, EventArgs e)
        {
            MySqlConnection Conne = Conn.ObteConnetion();
            listaempre.DropDownStyle = ComboBoxStyle.DropDownList;
            string query = "SELECT Id_Empresa,Nombre FROM Empresas WHERE IdEstado = 1 order by Nombre";

            MySqlCommand cmd = new MySqlCommand(query, Conne);
            MySqlDataAdapter da1 = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da1.Fill(dt);
            listaempre.ValueMember = "Id_Empresa";
            listaempre.DisplayMember = "Nombre";
            listaempre.DataSource = dt;
        }
        private async Task<bool> VerificarExistenciaRegistroAsync()
        {
            string CC = textBCC.Text;
            bool registroExiste = false;

            // Configura la cadena de conexión a tu base de datos MySQL Server
            MySqlConnection Conne = Conn.ObteConnetion();

            var query = "SELECT COUNT(*) FROM usuario WHERE Cedula = @Cedula";
            MySqlCommand command = new MySqlCommand(query, Conne);
            command.Parameters.AddWithValue("@Cedula", CC);

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
        private void OnTemplate(DPFP.Template template)
        {
            this.Invoke(new Function(delegate ()
            {
                Template = template;
                //btnAgregar.Enabled = (Template != null);
                if (Template != null)
                {
                    MessageBox.Show("La plantilla de huellas dactilares está lista para la verificación de huellas dactilares.", "Registro de huellas dactilares");
                    txtHuella.Text = "Huella capturada correctamente";
                }
                else
                {
                    MessageBox.Show("La plantilla de huellas dactilares no es válida. Repita el registro de huellas digitales.", "Registro de huellas digitales");
                }
            }));
        }

        private void BtHuella_Click(object sender, EventArgs e)
        {
            CapturarHuella Capturar = new CapturarHuella();
            Capturar.OnTemplate += this.OnTemplate;
            Capturar.ShowDialog();
        }
        private void textBCC_Enter(object sender, EventArgs e)
        {
            if (textBCC.Text == "C.C")
            {
                textBCC.Text = "";
                textBCC.ForeColor = Color.LightGray;
            }
        }

        private void textBCC_Leave(object sender, EventArgs e)
        {
            if (textBCC.Text == "")
            {
                textBCC.Text = "C.C";
                textBCC.ForeColor = Color.DimGray;
            }
        }

        private void textBCC_Enter_1(object sender, EventArgs e)
        {
            if (textBCC.Text == "C.C")
            {
                textBCC.Text = "";
                textBCC.ForeColor = Color.LightGray;
            }
        }

        private void textBNombre_Leave(object sender, EventArgs e)
        {
            if (textBNombre.Text == "")
            {
                textBNombre.Text = "NOMBRE";
                textBNombre.ForeColor = Color.DimGray;
            }
        }

        private void textBNombre_Enter(object sender, EventArgs e)
        {
            if (textBNombre.Text == "NOMBRE")
            {
                textBNombre.Text = "";
                textBNombre.ForeColor = Color.LightGray;
            }
        }

        private void textBApellido_Leave(object sender, EventArgs e)
        {
            if (textBApellido.Text == "")
            {
                textBApellido.Text = "APELLIDOS";
                textBApellido.ForeColor = Color.DimGray;
            }
        }

        private void textBApellido_Enter(object sender, EventArgs e)
        {
            if (textBApellido.Text == "APELLIDOS")
            {
                textBApellido.Text = "";
                textBApellido.ForeColor = Color.LightGray;
            }
        }

        private void textBCelular_Leave(object sender, EventArgs e)
        {
            if (textBCelular.Text == "")
            {
                textBCelular.Text = "CELULAR";
                textBCelular.ForeColor = Color.DimGray;
            }
        }

        private void textBEmail_Leave(object sender, EventArgs e)
        {
            if (textBEmail.Text == "")
            {
                textBEmail.Text = "CORREO";
                textBEmail.ForeColor = Color.DimGray;
            }
        }

        private void textBCelular_Enter(object sender, EventArgs e)
        {
            if (textBCelular.Text == "CELULAR")
            {
                textBCelular.Text = "";
                textBCelular.ForeColor = Color.LightGray;
            }
        }

        private void textBEmail_Enter(object sender, EventArgs e)
        {
            if (textBEmail.Text == "CORREO")
            {
                textBEmail.Text = "";
                textBEmail.ForeColor = Color.LightGray;
            }
        }

        private void textBClave_Leave(object sender, EventArgs e)
        {
            if (textBClave.Text == "")
            {
                textBClave.Text = "CLAVE";
                textBClave.ForeColor = Color.DimGray;
            }
        }

        private void textBClave_Enter(object sender, EventArgs e)
        {
            if (textBClave.Text == "CLAVE")
            {
                textBClave.Text = "";
                textBClave.ForeColor = Color.LightGray;
            }
        }

        private void textBClaveConfi_Leave(object sender, EventArgs e)
        {
            if (textBClaveConfi.Text == "")
            {
                textBClaveConfi.Text = "CONFIRMAR CLAVE";
                textBClaveConfi.ForeColor = Color.DimGray;
            }
        }

        private void textBClaveConfi_Enter(object sender, EventArgs e)
        {
            if (textBClaveConfi.Text == "CONFIRMAR CLAVE")
            {
                textBClaveConfi.Text = "";
                textBClaveConfi.ForeColor = Color.LightGray;
            }
        }
    }
}
