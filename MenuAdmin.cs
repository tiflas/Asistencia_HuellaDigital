using AppMonitoreo.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AppMonitoreo
{
    public partial class MenuAdmin : Form
    {
        public int IdUsuariooo;
        public string CeC;
        public string Nombre;
        public string Apellido;
        public string NombreEmpresa;
        public string Movil;
        public string Email;
        public string IdEstado;
        public string Rool;
        public int IdEmpresa;
        public string Clavv;
        public MenuAdmin()
        {
            InitializeComponent();
            ConfigureChart();
            LoadDataGrafic();
        }
        private void AbrirUsuarios(object formUsuarios)
        {
            // Liberar recursos y eliminar controles existentes en el PanelContenedor
            if (this.PanelContenedor.Controls.Count > 0)
            {
                // Iterar sobre los controles para liberar recursos si es necesario
                foreach (Control control in this.PanelContenedor.Controls)
                {
                    if (control is Chart chartControl)
                    {
                        // Liberar recursos asociados con el gráfico
                        chartControl.Dispose();
                    }
                }

                // Eliminar todos los controles del contenedor
                this.PanelContenedor.Controls.Clear();
            }

            // Preparar y mostrar el nuevo formulario
            Form fh = formUsuarios as Form;
            if (fh != null)
            {
                fh.TopLevel = false;
                fh.Dock = DockStyle.Fill;
                this.PanelContenedor.Controls.Add(fh);
                this.PanelContenedor.Tag = fh;
                fh.Show();
            }
        }

        private void AbrirEmpresas(object formEmpresa)
        {
            // Liberar recursos y eliminar controles existentes
            if (this.PanelContenedor.Controls.Count > 0)
            {
                // Iterar sobre los controles para liberar recursos si es necesario
                foreach (Control control in this.PanelContenedor.Controls)
                {
                    if (control is Chart chartControl)
                    {
                        // Liberar recursos asociados con el gráfico
                        chartControl.Dispose();
                    }
                }

                // Eliminar todos los controles del contenedor
                this.PanelContenedor.Controls.Clear();
            }

            // Preparar y mostrar el nuevo formulario
            Form fhe = formEmpresa as Form;
            fhe.TopLevel = false;
            fhe.Dock = DockStyle.Fill;
            this.PanelContenedor.Controls.Add(fhe);
            this.PanelContenedor.Tag = fhe;
            fhe.Show();
        }

        private void AbrirAsistencia(object formAsistencia)
        {
            // Liberar recursos y eliminar controles existentes en el PanelContenedor
            if (this.PanelContenedor.Controls.Count > 0)
            {
                // Iterar sobre los controles para liberar recursos si es necesario
                foreach (Control control in this.PanelContenedor.Controls)
                {
                    if (control is Chart chartControl)
                    {
                        // Liberar recursos asociados con el gráfico
                        chartControl.Dispose();
                    }
                }

                // Eliminar todos los controles del contenedor
                this.PanelContenedor.Controls.Clear();
            }

            // Preparar y mostrar el nuevo formulario
            Form fhe = formAsistencia as Form;
            if (fhe != null)
            {
                fhe.TopLevel = false;
                fhe.Dock = DockStyle.Fill;
                this.PanelContenedor.Controls.Add(fhe);
                this.PanelContenedor.Tag = fhe;
                fhe.Show();
            }
        }

        private void AbrirConfiguracion(object formConfiguracion)
        {
            // Liberar recursos y eliminar controles existentes en el PanelContenedor
            if (this.PanelContenedor.Controls.Count > 0)
            {
                // Iterar sobre los controles para liberar recursos si es necesario
                foreach (Control control in this.PanelContenedor.Controls)
                {
                    // Verificar si el control es un gráfico y liberar sus recursos
                    if (control is Chart chartControl)
                    {
                        chartControl.Dispose();
                    }
                    // Si el control es otro tipo que requiere liberación, añadir la lógica aquí
                    // Ejemplo: if (control is SomeOtherResourceType resourceControl) { resourceControl.Dispose(); }
                }

                // Eliminar todos los controles del contenedor
                this.PanelContenedor.Controls.Clear();
            }

            // Preparar y mostrar el nuevo formulario
            Form fhe = formConfiguracion as Form;
            if (fhe != null)
            {
                fhe.TopLevel = false;
                fhe.Dock = DockStyle.Fill;
                this.PanelContenedor.Controls.Add(fhe);
                this.PanelContenedor.Tag = fhe;
                fhe.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AbrirUsuarios(new ModuloUsuario());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AbrirEmpresas(new AgregarEmpresa());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AbrirAsistencia(new AdminAsistencia());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ConfiguracionUsuario menu1 = new ConfiguracionUsuario();
            menu1.IdUsuarioo = ModelsBD.IdUsuarioo;
            menu1.CeC = ModelsBD.CC;
            menu1.Nombre = ModelsBD.Nombre;
            menu1.Apellido = ModelsBD.Apellido;
            menu1.Movil = ModelsBD.Movil;
            menu1.Email = ModelsBD.Email;
            menu1.IdEstado = ModelsBD.IdEstado;
            menu1.IdEmpresa = ModelsBD.IdEmpresa;
            menu1.Roll = ModelsBD.Rol_usu;
            menu1.Clavee = ModelsBD.Clavee;
            AbrirConfiguracion(menu1);
        }
        
        private void ConfigureChart()
        {
            MySqlConnection Conne = Conn.ObteConnetion();
            string query = @"
    SELECT 
        em.Nombre, 
        COUNT(usuario.Id_Usuario) AS Numero_Usuarios 
    FROM 
        usuario 
    INNER JOIN 
        Empresas em ON em.Id_Empresa = usuario.Id_Empresa 
    GROUP BY 
        em.Id_Empresa, em.Nombre;";

            using (Conne)
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, Conne);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Configurar el gráfico
                chart1.Series.Clear(); // Limpiar series existentes

                // Crear y configurar una nueva serie de tipo Donut
                Series series = new Series("Número de Usuarios")
                {
                    ChartType = SeriesChartType.Doughnut, // Tipo de gráfico Donut
                    XValueType = ChartValueType.String,
                    YValueType = ChartValueType.Int32
                };

                // Agregar datos a la serie
                foreach (DataRow row in dataTable.Rows)
                {
                    string nombreEmpresa = row["Nombre"].ToString();
                    int numeroUsuarios = Convert.ToInt32(row["Numero_Usuarios"]);
                    series.Points.AddXY(nombreEmpresa, numeroUsuarios);
                }

                // Agregar la serie al gráfico
                chart1.Series.Add(series);

                // Configurar el área del gráfico
                chart1.ChartAreas[0].Area3DStyle.Enable3D = true; // Opcional: Para dar un efecto 3D al gráfico

                // Configurar los títulos y otras propiedades
                chart1.Titles.Clear();
                chart1.Titles.Add("Número de Usuarios por Empresa");

                // Opcional: Configurar el formato de las etiquetas
                series.Label = "#PERCENT{P0}"; // Muestra el porcentaje de cada sección
                series.LegendText = "#VALX";   // Muestra el nombre de la empresa en la leyenda
                chart1.Legends[0].Docking = Docking.Bottom; // Opcional: Configura la ubicación de la leyenda

                // Configurar el formato de los valores en el gráfico
                foreach (DataPoint point in series.Points)
                {
                    point.Label = $"{point.YValues[0]:0}"; // Muestra el número total de usuarios
                }

                // Configurar el formato de la leyenda
                chart1.Legends[0].Docking = Docking.Bottom;
                chart1.Legends[0].Alignment = StringAlignment.Center;
                chart1.Legends[0].Font = new Font("Arial", 10, FontStyle.Bold);
            }
        }
        private void LoadDataGrafic()
        {
            // Cadena de conexión a tu base de datos
            MySqlConnection Conne = Conn.ObteConnetion();
            string query = @"
    SELECT Fecha_Entrada, 
           CONCAT(LPAD(HOUR(Hora_Ingreso), 2, '0'), ':', LPAD(FLOOR(MINUTE(Hora_Ingreso) / 30) * 30, 2, '0')) AS Intervalo,
           COUNT(*) AS Conteo_Usuarios
    FROM RegistroMonitoreo
    WHERE Fecha_Entrada >= CURDATE() - INTERVAL 7 DAY
    GROUP BY Fecha_Entrada, FLOOR(HOUR(Hora_Ingreso)) * 60 + FLOOR(MINUTE(Hora_Ingreso) / 30) * 30
    ORDER BY Fecha_Entrada DESC, Intervalo DESC LIMIT 7;";

            using (Conne)
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, Conne);
                DataTable dataTable1 = new DataTable();
                adapter.Fill(dataTable1);

                // Verificar si hay datos
                if (dataTable1.Rows.Count > 0)
                {
                    // Configurar el gráfico
                    chart2.Series.Clear(); // Limpiar series existentes

                    // Crear y configurar una nueva serie de columnas
                    Series series = new Series("Conteo de Usuarios")
                    {
                        ChartType = SeriesChartType.Column,
                        XValueType = ChartValueType.String,
                        YValueType = ChartValueType.Int32
                    };

                    // Agregar datos a la serie
                    foreach (DataRow row in dataTable1.Rows)
                    {
                        string fecha = Convert.ToDateTime(row["Fecha_Entrada"]).ToString("dd/MM/yyyy"); // Formato de fecha
                        string intervalo = row["Intervalo"].ToString();
                        string xValue = $"{fecha} {intervalo}"; // Combinar fecha e intervalo
                        int conteo = Convert.ToInt32(row["Conteo_Usuarios"]);
                        series.Points.AddXY(xValue, conteo);
                    }

                    // Agregar la serie al gráfico
                    chart2.Series.Add(series);

                    // Configurar el área del gráfico
                    chart2.ChartAreas[0].AxisX.Title = "Fecha e Intervalo de Tiempo";
                    chart2.ChartAreas[0].AxisY.Title = "Número de Usuarios";
                    chart2.ChartAreas[0].AxisX.Interval = 1; // Opcional: Ajusta el intervalo del eje X

                    // Establecer el título del gráfico
                    chart2.Titles.Clear(); // Limpiar títulos existentes
                    chart2.Titles.Add("Hora con mas flujo de Ingreso Por Usuarios");
                }
                else
                {
                    // Manejar el caso en que no hay datos
                    chart2.Titles.Clear();
                    chart2.Titles.Add("No hay datos disponibles");
                }
            }
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 Inicio = new Form1();
            Inicio.Show();
            this.Hide();
        }
    }
}
