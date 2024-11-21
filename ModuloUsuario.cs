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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppMonitoreo
{
    public partial class ModuloUsuario : Form
    {
        private DPFP.Template Template;
        public string Passs;
        public string PascDes;
        public string HuellaA;
        public ModuloUsuario()
        {
            InitializeComponent();
            textCedula.TextChanged += new EventHandler(textCedula_TextChanged);
            textNombre.TextChanged += new EventHandler(textNombre_TextChanged);
            textApellido.TextChanged += new EventHandler(textApellido_TextChanged);
            textEmail.TextChanged += new EventHandler(textEmail_TextChanged);
            textCelular.TextChanged += new EventHandler(textCelular_TextChanged);
        }
        public void Imagen_load()
        {
            Thread.Sleep(3000);
        }
        private void TablaUsuarios_MouseClick(object sender, MouseEventArgs e)
        {
            Encrypt64 Encry = new Encrypt64();
            textCedula.Text = TablaUsuarios.CurrentRow.Cells[0].Value.ToString();
            textNombre.Text = TablaUsuarios.CurrentRow.Cells[1].Value.ToString();
            textApellido.Text = TablaUsuarios.CurrentRow.Cells[2].Value.ToString();
            HuellaA = TablaUsuarios.CurrentRow.Cells[3].Value.ToString();
            textEmail.Text = TablaUsuarios.CurrentRow.Cells[4].Value.ToString();
            textCelular.Text = TablaUsuarios.CurrentRow.Cells[5].Value.ToString();
            SelectEmpresa.Text = TablaUsuarios.CurrentRow.Cells[6].Value.ToString();
            RolUsuario.Text = TablaUsuarios.CurrentRow.Cells[7].Value.ToString();
            ListEstado.Text = TablaUsuarios.CurrentRow.Cells[8].Value.ToString();
            //textClave.Text = TablaUsuarios.CurrentRow.Cells[8].Value.ToString();
            Passs = TablaUsuarios.CurrentRow.Cells[9].Value.ToString();
            PascDes = Encry.Desencriptar(Passs);
            textClave.Text = PascDes;
        }

        private async void ModuloUsuario_Load(object sender, EventArgs e)
        {
            Task oTask = new Task(Imagen_load);
            ImagenLoad.Visible = true;
            oTask.Start();
            await oTask;
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

            ModelsBD ModelsConexion = new ModelsBD();
            ModelsConexion.CargarUsuario(TablaUsuarios);
            ImagenLoad.Visible = false;
        }

        private void btModiUsu_Click(object sender, EventArgs e)
        {
            Encrypt64 Encry = new Encrypt64();
            if (textCedula.Text == "")
            {
                MessageBox.Show("El campo de Cedula es obligatorio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (MessageBox.Show("Esta Seguro que desea Modificar los datos seleccionados", "Modificar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DateTime Fecha = DateTime.Now;
                string FechaActual = Fecha.ToString("yyyy-MM-dd");
                Usuario JactUsuario = new Usuario();
                byte[] StremeHuella = null;
                JactUsuario.Cedula = textCedula.Text.Trim();
                string Pasc = Encry.Encripta(textClave.Text.Trim());
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
                if (Template != null)
                {
                    StremeHuella = Template.Bytes;
                    string codigoTexto = Convert.ToBase64String(StremeHuella);
                    JactUsuario.Huella = codigoTexto;
                    int Resultado = ModelsBD.ModificarUsuario(JactUsuario);
                    if (Resultado > 0)
                    {
                        MessageBox.Show("Se Han Actualizado Los Datos Correctamente", "Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ModelsBD ModelsConexion = new ModelsBD();
                        ModelsConexion.CargarUsuario(TablaUsuarios);
                        textCedula.Text = "";
                        textNombre.Text = "";
                        textApellido.Text = "";
                        textCelular.Text = "";
                        textEmail.Text = "";
                        textClave.Text = "";
                        Template = null;
                    }
                    else
                    {
                        MessageBox.Show("No se lograron actualizar los datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    JactUsuario.Huella = HuellaA;
                    int Resultado = ModelsBD.ModificarUsuario(JactUsuario);
                    if (Resultado > 0)
                    {
                        MessageBox.Show("Se Han Actualizado Los Datos Correctamente", "Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ModelsBD ModelsConexion = new ModelsBD();
                        ModelsConexion.CargarUsuario(TablaUsuarios);
                        textCedula.Text = "";
                        textNombre.Text = "";
                        textApellido.Text = "";
                        textCelular.Text = "";
                        textEmail.Text = "";
                        textClave.Text = "";
                        Template = null;
                    }
                    else if (Resultado == -1)
                    {
                        MessageBox.Show("No se lograron actualizar los datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void BtGuardar_Click(object sender, EventArgs e)
        {
            MySqlConnection Conne = Conn.ObteConnetion();
            Encrypt64 Encry = new Encrypt64();
            bool registroExiste = await VerificarExistenciaRegistroAsync();
            if (textCedula.Text == "" || textNombre.Text == "" || textApellido.Text == "")
            {
                MessageBox.Show("Hay Campos vacios obligatorios - Cedula,Nombre,Apellido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (registroExiste)
            {
                // El registro ya existe, mostrar un mensaje al usuario o realizar alguna acción
                MessageBox.Show("La Cedula que has Ingresado ya se encuentra Registrada Porfavor Verificar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                Usuario Jusuario = new Usuario();
                DateTime Fecha = DateTime.Now;
                string FechaActual = Fecha.ToString("yyyy-MM-dd");
                byte[] StremeHuella = null;
                Jusuario.Cedula = textCedula.Text.Trim();
                string Pasc = Encry.Encripta(textClave.Text.Trim());
                Jusuario.Nombre = textNombre.Text.Trim();
                Jusuario.Apellido = textApellido.Text.Trim();
                Jusuario.Rol = Convert.ToInt32(RolUsuario.SelectedValue.ToString());
                Jusuario.IdEmpresa = SelectEmpresa.SelectedValue.ToString();
                Jusuario.Movil = textCelular.Text.Trim();
                Jusuario.Email = textEmail.Text.Trim();
                Jusuario.Clave = Pasc;
                Jusuario.Fecha_Creacion = FechaActual;
                Jusuario.IdEstado = Convert.ToInt32(ListEstado.SelectedValue.ToString());
                if (Template != null)
                {
                    StremeHuella = Template.Bytes;
                    string codigoTexto = Convert.ToBase64String(StremeHuella);
                    Jusuario.Huella = codigoTexto;
                    int Resultado = ModelsBD.InsertUsu(Jusuario);
                    if (Resultado > 0)
                    {
                        MessageBox.Show("Se ha Registrado Correctamente", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ModelsBD ModelsConexion = new ModelsBD();
                        ModelsConexion.CargarUsuario(TablaUsuarios);
                        textCedula.Text = "";
                        textNombre.Text = "";
                        textApellido.Text = "";
                        textCedula.Text = "";
                        textEmail.Text = "";
                        textClave.Text = "";
                        textCelular.Text = "";
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
                        ModelsBD ModelsConexion = new ModelsBD();
                        ModelsConexion.CargarUsuario(TablaUsuarios);
                        textCedula.Text = "";
                        textNombre.Text = "";
                        textApellido.Text = "";
                        textCelular.Text = "";
                        textEmail.Text = "";
                        textClave.Text = "";
                        Template = null;
                    }
                    else if (Resultado == -1)
                    {
                        MessageBox.Show("El usuario que intenta registrar ya existe en base de datos");
                    }
                }
            }
        }
        private async Task<bool> VerificarExistenciaRegistroAsync()
        {
            string CC = textCedula.Text;
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

        private void BtExcel_Click(object sender, EventArgs e)
        {
            new ExportarExcel().ExportarAExcel(TablaUsuarios);
        }

        private void ImporExce_Click(object sender, EventArgs e)
        {
            // Crear y configurar el OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos Excel (*.xlsx)|*.xlsx";
            openFileDialog.Title = "Seleccione plantilla de Excel Especifica para Cargar";

            // Mostrar el diálogo y verificar si el usuario seleccionó un archivo
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Obtener la ruta del archivo seleccionado
                string filePath = openFileDialog.FileName;
                ModelsBD ModelsConexion = new ModelsBD();
                try
                {
                    // Cargar el archivo Excel
                    SLDocument Sl = new SLDocument(filePath);
                    SLWorksheetStatistics Propiedades = Sl.GetWorksheetStatistics();
                    int UltimaFila = Propiedades.EndRowIndex;

                    // Conectar a la base de datos
                    using (MySqlConnection Conne = Conn.ObteConnetion())
                    {
                        string error = "";
                        DateTime localDateTime = DateTime.Now;
                        string bogotaTimeZoneId = "SA Pacific Standard Time";
                        TimeZoneInfo bogotaTimeZone = TimeZoneInfo.FindSystemTimeZoneById(bogotaTimeZoneId);
                        DateTime bogotaDateTime = TimeZoneInfo.ConvertTime(localDateTime, bogotaTimeZone);
                        string FechaActual = bogotaDateTime.ToString("yyyy-MM-dd");
                        string Dato1 = FechaActual;
                        // Iterar sobre las filas del archivo Excel
                        for (int x = 2; x <= UltimaFila; x++)
                        {
                            string Cedu = Sl.GetCellValueAsString("A" + x);
                            if (ExisteUsuario(Cedu))
                            {
                                error += "Ya Existe Cedula con Numero " + Cedu + "\n";
                            }
                            else
                            {
                                string Query = "INSERT INTO usuario (Cedula, Huella, Nombre, Apellido, Id_rol, Id_Empresa, Movil, Email, Clave, IdEstado, Fecha_Creacion, Fecha_Actualizacion) VALUES(@Cedula,'',@Nombre,@Apellido,3,1,@Movil,@Email,'',1,'" + Dato1 + "','')";

                                try
                                {
                                    using (MySqlCommand Comando = new MySqlCommand(Query, Conne))
                                    {
                                        Comando.Parameters.AddWithValue("@Cedula", Sl.GetCellValueAsString("A" + x));
                                        Comando.Parameters.AddWithValue("@Nombre", Sl.GetCellValueAsString("B" + x));
                                        Comando.Parameters.AddWithValue("@Apellido", Sl.GetCellValueAsString("C" + x));
                                        //Comando.Parameters.AddWithValue("@Id_rol", Sl.GetCellValueAsString("D" + x));
                                        /*int idRol;
                                        if (!int.TryParse(Sl.GetCellValueAsString("D" + x), out idRol))
                                        {
                                            idRol = 0;
                                        }
                                        Comando.Parameters.AddWithValue("@Id_rol", idRol);
                                        /*int Idempresa;
                                        if (!int.TryParse(Sl.GetCellValueAsString("E" + x), out Idempresa))
                                        {
                                            Idempresa = 0;
                                        }*/
                                        //Comando.Parameters.AddWithValue("@Id_Empresa", Sl.GetCellValueAsString("E" + x));
                                        Comando.Parameters.AddWithValue("@Movil", Sl.GetCellValueAsString("D" + x));
                                        Comando.Parameters.AddWithValue("@Email", Sl.GetCellValueAsString("E" + x));
                                        Comando.ExecuteNonQuery();
                                    }
                                }
                                catch (MySqlException Ex)
                                {
                                    MessageBox.Show($"Error al Insertar en la base de datos: {Ex.Message}");
                                    ModelsConexion.CargarUsuario(TablaUsuarios);
                                }
                            }
                        }
                        MessageBox.Show("¡Plantilla de Excel Cargada!\n" + error, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ModelsConexion.CargarUsuario(TablaUsuarios);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al leer el archivo Excel: {ex.Message}");
                }
            }
        }
        private bool ExisteUsuario(string Cedu)
        {
            MySqlConnection Conne = Conn.ObteConnetion();
            string QuerySQL = "SELECT Cedula FROM usuario WHERE Cedula='" + Cedu + "'";
            MySqlCommand Comando = new MySqlCommand(QuerySQL, Conne);
            int Num = Convert.ToInt32(Comando.ExecuteScalar());
            if (Num > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Ruta del archivo en el directorio de salida del proyecto
            string rutaOrigen = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plantilla.xlsx");

            // Ruta donde se guardará el archivo descargado
            string rutaDestino = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Plantilla.xlsx");

            try
            {
                File.Copy(rutaOrigen, rutaDestino, true);
                MessageBox.Show($"La plantilla se ha descargado en {rutaDestino}", "Descarga completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al descargar la plantilla: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtHuella_Click(object sender, EventArgs e)
        {
            CapturarHuella Capturar = new CapturarHuella();
            Capturar.OnTemplate += this.OnTemplate;
            Capturar.ShowDialog();
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

        private void textCedula_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Este Campo solo admite Numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void textCedula_TextChanged(object sender, EventArgs e)
        {
            if (textCedula.Text.Length > 20)
            {
                // Si el texto excede el límite, recórtalo y muestra un mensaje
                textCedula.Text = textCedula.Text.Substring(0, 20);
                textCedula.SelectionStart = textCedula.Text.Length;  // Mover el cursor al final
                MessageBox.Show($"Te has excedido el máximo de {20} caracteres permitidos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textNombre_TextChanged(object sender, EventArgs e)
        {
            if (textNombre.Text.Length > 30)
            {
                // Si el texto excede el límite, recórtalo y muestra un mensaje
                textNombre.Text = textNombre.Text.Substring(0, 30);
                textNombre.SelectionStart = textNombre.Text.Length;  // Mover el cursor al final
                MessageBox.Show($"Te has excedido el máximo de {30} caracteres permitidos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textApellido_TextChanged(object sender, EventArgs e)
        {
            if (textApellido.Text.Length > 30)
            {
                // Si el texto excede el límite, recórtalo y muestra un mensaje
                textApellido.Text = textApellido.Text.Substring(0, 30);
                textApellido.SelectionStart = textApellido.Text.Length;  // Mover el cursor al final
                MessageBox.Show($"Te has excedido el máximo de {30} caracteres permitidos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textEmail_TextChanged(object sender, EventArgs e)
        {
            if (textEmail.Text.Length > 40)
            {
                // Si el texto excede el límite, recórtalo y muestra un mensaje
                textEmail.Text = textEmail.Text.Substring(0, 40);
                textEmail.SelectionStart = textEmail.Text.Length;  // Mover el cursor al final
                MessageBox.Show($"Te has excedido el máximo de {40} caracteres permitidos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textCelular_TextChanged(object sender, EventArgs e)
        {
            if (textCelular.Text.Length > 20)
            {
                // Si el texto excede el límite, recórtalo y muestra un mensaje
                textCelular.Text = textCelular.Text.Substring(0, 20);
                textCelular.SelectionStart = textCelular.Text.Length;  // Mover el cursor al final
                MessageBox.Show($"Te has excedido el máximo de {20} caracteres permitidos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtBuscar_Click(object sender, EventArgs e)
        {
            if (textCedula.Text == "")
            {
                textCedula.Text = " ";
                ModelsBD ModelsConexion = new ModelsBD();
                ModelsConexion.BuscarUsuario(TablaUsuarios, textCedula.Text, textNombre.Text);
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
                ModelsConexion.BuscarUsuario(TablaUsuarios, textCedula.Text, textNombre.Text);
                textCedula.Text = "";
                textNombre.Text = "";
                textApellido.Text = "";
                textCelular.Text = "";
                textEmail.Text = "";
            }
        }
    }
}
