using AppMonitoreo.Controller;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace AppMonitoreo.Models
{
    class ModelsBD
    {
        public static int IdUsuarioo;
        public static string CC;
        //public static byte[]? HuellaDigi;
        public static string HuellaDigi;
        public static string Nombre;
        public static string Apellido;
        public static string Rol_usu;
        public static int IdEmpresa;
        public static string NombreEmpresa;
        public static string Movil;
        public static string Email;
        public static string Clavee;
        public static string IdEstado;
        public static int InsertUsu(Usuario jUsuario)
        {
            int Retorno = 0;
            MySqlConnection Conne = Conn.ObteConnetion();
            MySqlCommand Command = new MySqlCommand(string.Format("INSERT INTO usuario (Cedula, Huella, Nombre, Apellido, Id_rol, Id_Empresa, Movil, Email, Clave, IdEstado,Fecha_Creacion,Fecha_Actualizacion) values('{0}', '{1}', '{2}', '{3}', '{4}','{5}','{6}','{7}','{8}','{9}','{10}','')", jUsuario.Cedula, jUsuario.Huella, jUsuario.Nombre, jUsuario.Apellido, jUsuario.Rol,jUsuario.IdEmpresa,jUsuario.Movil,jUsuario.Email,jUsuario.Clave,jUsuario.IdEstado,jUsuario.Fecha_Creacion), Conne);
            Retorno = Command.ExecuteNonQuery();
            Conne.Close();
            return Retorno;
        }
        public static int ModificarUsuario(Usuario jusuario)
        {
            int Resultado = 0;
            MySqlConnection Conne = Conn.ObteConnetion();
            MySqlCommand Comando = new MySqlCommand(string.Format("UPDATE usuario SET Huella ='{0}',Nombre='{1}',Apellido='{2}',Id_rol='{3}',Id_Empresa='{4}',Movil='{5}',Email='{6}',Clave='{7}',IdEstado='{8}',Fecha_Actualizacion='{9}' WHERE Cedula='{10}'",
                jusuario.Huella, jusuario.Nombre, jusuario.Apellido, jusuario.Rol, jusuario.IdEmpresa, jusuario.Movil,jusuario.Email,jusuario.Clave,jusuario.IdEstado,jusuario.Fecha_Actualizacion,jusuario.Cedula), Conne);
            Resultado = Comando.ExecuteNonQuery();
            Conne.Close();
            return Resultado;
        }
        public static int GuardarRegistro(RegistroMonitoreo jMonitoreo)
        {
            int Retorno = 0;
            MySqlConnection Conne = Conn.ObteConnetion();
            MySqlCommand Command = new MySqlCommand(string.Format("INSERT INTO RegistroMonitoreo (CC_usuario, Id_Empresa, Fecha_Entrada, Hora_Ingreso,Hora_Salida) values('{0}', '{1}', '{2}', '{3}','')", jMonitoreo.Id_usuario, jMonitoreo.Id_empresa, jMonitoreo.FechaEntrada, jMonitoreo.TiempoIngreso), Conne);
            Retorno = Command.ExecuteNonQuery();
            Conne.Close();
            return Retorno;
        }

        public static long VerificarRegistro(RegistroMonitoreo jMonitoreo)
        {
            long Retorno = 0 ;
            MySqlConnection Conne = Conn.ObteConnetion();
            MySqlCommand Command = new MySqlCommand(string.Format("SELECT COUNT(*) FROM RegistroMonitoreo WHERE CC_usuario = '{0}' AND Fecha_Entrada  = '{1}'", jMonitoreo.Id_usuario,jMonitoreo.FechaEntrada), Conne);
            Retorno = (long)Command.ExecuteScalar();
            Conne.Close();
            return Retorno;
        }
        public static long VerificarRegistroSalida(RegistroMonitoreo jMonitoreo)
        {
            long Retorno = 0;
            MySqlConnection Conne = Conn.ObteConnetion();
            MySqlCommand Command = new MySqlCommand(string.Format("SELECT COUNT(*) FROM RegistroMonitoreo WHERE CC_usuario = '{0}' AND Fecha_Entrada  = '{1}' AND Hora_Ingreso <> '' AND Hora_Salida <> ''", jMonitoreo.Id_usuario, jMonitoreo.FechaEntrada), Conne);
            Retorno = (long)Command.ExecuteScalar();
            Conne.Close();
            return Retorno;
        }
        public static string VerificarHoraIngreso(RegistroMonitoreo jMonitoreo)
        {
            string horaIngreso = null;

            using (MySqlConnection Conne = Conn.ObteConnetion())
            {
                string query = "SELECT Hora_Ingreso FROM RegistroMonitoreo WHERE CC_usuario = @CCUsuario AND Fecha_Entrada = @FechaEntrada";

                using (MySqlCommand Command = new MySqlCommand(query, Conne))
                {
                    Command.Parameters.AddWithValue("@CCUsuario", jMonitoreo.Id_usuario);
                    Command.Parameters.AddWithValue("@FechaEntrada", jMonitoreo.FechaEntrada);

                    object result = Command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        // Convertir el resultado a string (esto debe coincidir con el formato en la base de datos)
                        horaIngreso = result.ToString();
                    }
                }
            }

            return horaIngreso;
        }
        public static long VeriFicarEQUIPO(ObtenerIP Jequi)
        {
            long Retorno = 0;
            MySqlConnection Conne = Conn.ObteConnetion();
            MySqlCommand Command = new MySqlCommand(string.Format("SELECT COUNT(IpComp) FROM IpComp WHERE EquipoCompu = '{0}'", Jequi.ComEQUIPO), Conne);
            Retorno = (long)Command.ExecuteScalar();
            Conne.Close();
            return Retorno;
        }
        public static int GuardarRegistroSalida(RegistroMonitoreo jMonitoreo)
        {
            int Retorno = 0;
            MySqlConnection Conne = Conn.ObteConnetion();
            MySqlCommand Command = new MySqlCommand(string.Format("UPDATE RegistroMonitoreo SET Hora_Salida = '{0}' where CC_usuario ='{1}' AND Fecha_Entrada ='{2}'", jMonitoreo.TiempoSalida, jMonitoreo.Id_usuario, jMonitoreo.FechaEntrada), Conne);
            Retorno = Command.ExecuteNonQuery();
            Conne.Close();
            return Retorno;
        }
        public static int Auntentificar(string cc, string pass)
        {
            int Result = -1;
            MySqlConnection Conne = Conn.ObteConnetion();
            MySqlCommand Command = new MySqlCommand(string.Format("SELECT usuario.id_usuario,usuario.Cedula,usuario.Huella,usuario.Nombre,usuario.Apellido,Rol.Descripcion as Rol, em.Nombre as Empresa,usuario.Movil,usuario.Email,usuario.Clave,estad.Descripcion as Estado,em.Id_Empresa FROM usuario INNER JOIN Empresas em on em.Id_Empresa = usuario.Id_Empresa INNER JOIN Estado estad on estad.IdEstado = usuario.IdEstado INNER JOIN Rol ON Rol.IdRol = usuario.Id_rol WHERE Cedula = '{0}' AND Clave = '{1}'", cc, pass), Conne);
            MySqlDataReader Reader = Command.ExecuteReader();
            while (Reader.Read())
            {
                Result = 1;
                IdUsuarioo = Reader.GetInt32(0);
                CC = Reader.GetString(1);
                //HuellaDigi = (byte[])Reader.GetValue(2);
                HuellaDigi = Reader.GetString(2);
                Nombre = Reader.GetString(3);
                Apellido = Reader.GetString(4);
                Rol_usu = Reader.GetString(5);
                NombreEmpresa = Reader.GetString(6);
                Movil = Reader.GetString(7);
                Email = Reader.GetString(8);
                Clavee = Reader.GetString(9);
                IdEstado = Reader.GetString(10);
                IdEmpresa = Reader.GetInt32(11);
            }
            Conne.Close();
            return Result;
        }
        public static List<Huella> VerificarHuella()
        {
            var resultados = new List<Huella>();
            using (MySqlConnection conne = Conn.ObteConnetion())
            {
                using (MySqlCommand command = new MySqlCommand("SELECT id_usuario, Cedula, Huella, Id_Empresa, Nombre, Apellido FROM usuario WHERE Huella <> ''", conne))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var huella = new Huella
                            {
                                IdUsuario = reader.GetInt32(0),
                                Cedula = reader.GetString(1),
                                HuellaDigi = reader.GetString(2),
                                //HuellaDigi = (byte[])reader.GetValue(2),
                                IdEmpresa = reader.GetInt32(3),
                                Nombre = reader.GetString(4),
                                Apellido = reader.GetString(5)
                            };
                            resultados.Add(huella);
                        }
                    }
                }
                conne.Close();
            }
            return resultados;
        }
        public void CargarUsuario(DataGridView Dgv)
        {
            try
            {
                MySqlConnection Conne = Conn.ObteConnetion();
                MySqlDataAdapter Da = new MySqlDataAdapter("SELECT usuario.Cedula,usuario.Nombre,usuario.Apellido,usuario.Huella,usuario.Email,usuario.Movil,em.Nombre as Empresa,Rol.Descripcion Rol,Estado.Descripcion Estado,usuario.Clave,usuario.Fecha_Creacion,usuario.Fecha_Actualizacion FROM usuario INNER JOIN Empresas em on em.Id_Empresa = usuario.Id_Empresa INNER JOIN Rol on Rol.IdRol = usuario.Id_rol INNER JOIN Estado on Estado.IdEstado = usuario.IdEstado ORDER BY usuario.Nombre", Conne);
                DataTable Dt = new DataTable();
                Da.Fill(Dt);
                Dgv.DataSource = Dt;
                Conne.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo llenar el datagridview: " + ex.ToString());

            }
        }

        public static int InsertEmpresa(Empresa jEmpresa)
        {
            int Retorno = 0;
            MySqlConnection Conne = Conn.ObteConnetion();
            MySqlCommand Command = new MySqlCommand(string.Format("INSERT INTO Empresas (Rut, Nombre, Telefono, Direccion, IdEstado, IdRol, Fecha_Creacion,Fecha_Actualizacion) values('{0}', '{1}', '{2}', '{3}', '{4}','{5}','{6}','')", jEmpresa.RutEmpresa, jEmpresa.NombreEmpresa, jEmpresa.Telefono, jEmpresa.Direccion, jEmpresa.IdEstado, jEmpresa.Rol, jEmpresa.FechaCreacion), Conne);
            Retorno = Command.ExecuteNonQuery();
            Conne.Close();
            return Retorno;
        }

        public static int ModificarEmpresa(Empresa jEmpresa)
        {
            int Resultado = 0;
            MySqlConnection Conne = Conn.ObteConnetion();
            MySqlCommand Comando = new MySqlCommand(string.Format("UPDATE Empresas SET Rut ='{0}',Nombre='{1}',Telefono='{2}',Direccion='{3}',IdEstado='{4}',IdRol='{5}',Fecha_Actualizacion='{6}' WHERE Id_Empresa='{7}'",
                jEmpresa.RutEmpresa,jEmpresa.NombreEmpresa,jEmpresa.Telefono,jEmpresa.Direccion,jEmpresa.IdEstado,jEmpresa.Rol,jEmpresa.FechaActualizacion, jEmpresa.IdEmpresa), Conne);
            Resultado = Comando.ExecuteNonQuery();
            Conne.Close();
            return Resultado;
        }

        public void CargarEmpresa(DataGridView Dgv)
        {
            try
            {
                MySqlConnection Conne = Conn.ObteConnetion();
                MySqlDataAdapter Em = new MySqlDataAdapter("SELECT Empresas.Id_Empresa as ID,Empresas.Rut,Empresas.Nombre,Empresas.Telefono,Empresas.Direccion,Estado.Descripcion Estado,Rol.Descripcion Rol,Empresas.Fecha_Creacion,Empresas.Fecha_Actualizacion FROM Empresas INNER JOIN Rol on Rol.IdRol = Empresas.IdRol INNER JOIN Estado on Estado.IdEstado = Empresas.IdEstado ORDER BY Empresas.Nombre", Conne);
                DataTable Dt = new DataTable();
                Em.Fill(Dt);
                Dgv.DataSource = Dt;
                Conne.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo llenar el datagridview: " + ex.ToString());
            }
        }
        public int UsuarioRegistrado(string IdCedula)
        {
            int contador = 0;
            try
            {
                MySqlConnection Conne = Conn.ObteConnetion();
                MySqlCommand Cmd = new MySqlCommand("SELECT * FROM usuario WHERE Cedula ='" + IdCedula + "'", Conne);
                MySqlDataReader Dr = Cmd.ExecuteReader();
                while (Dr.Read())
                {
                    contador++;
                }
                Dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al Cargar Usuario Registrado: " + ex.ToString());
            }
            return contador;


        }
        public void BuscarUsuario(DataGridView Dgv, string Jusuarioced,string JNombre)
        {
            try
            {
                MySqlConnection Conne = Conn.ObteConnetion();
                MySqlDataAdapter Da = new MySqlDataAdapter("SELECT usuario.Cedula,usuario.Nombre,usuario.Apellido,usuario.Huella,usuario.Email,usuario.Movil,em.Nombre as Empresa,Rol.Descripcion Rol,Estado.Descripcion Estado,usuario.Clave,usuario.Fecha_Creacion,usuario.Fecha_Actualizacion FROM usuario INNER JOIN Empresas em on em.Id_Empresa = usuario.Id_Empresa INNER JOIN Rol on Rol.IdRol = usuario.Id_rol INNER JOIN Estado on Estado.IdEstado = usuario.IdEstado WHERE usuario.Cedula LIKE '" + Jusuarioced + "%' OR usuario.Nombre LIKE '"+ JNombre + "%' ORDER BY usuario.Nombre", Conne);
                DataTable Dt = new DataTable();
                Da.Fill(Dt);
                Dgv.DataSource = Dt;
                Conne.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo llenar el Datagridview: " + ex.ToString());
            }
        }

        public void BuscarEmpresa(DataGridView Dgv, string JEmpresaAct,string JNombre)
        {
            try
            {
                MySqlConnection Conne = Conn.ObteConnetion();
                MySqlDataAdapter Da = new MySqlDataAdapter("SELECT Empresas.Id_Empresa as ID,Empresas.Rut,Empresas.Nombre,Empresas.Telefono,Empresas.Direccion,Estado.Descripcion Estado,Rol.Descripcion Rol,Empresas.Fecha_Creacion,Empresas.Fecha_Actualizacion FROM Empresas INNER JOIN Rol on Rol.IdRol = Empresas.IdRol INNER JOIN Estado on Estado.IdEstado = Empresas.IdEstado WHERE Empresas.Rut LIKE '" + JEmpresaAct + "%' OR Empresas.Nombre LIKE '" + JNombre + "%' ORDER BY Empresas.Nombre", Conne);
                DataTable Dt = new DataTable();
                Da.Fill(Dt);
                Dgv.DataSource = Dt;
                Conne.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo llenar el Datagridview: " + ex.ToString());
            }
        }

        public void CargarAsistencia(DataGridView Dgv)
        {
            try
            {
                MySqlConnection Conne = Conn.ObteConnetion();
                MySqlDataAdapter Da = new MySqlDataAdapter("SELECT usu.Cedula,usu.Nombre,usu.Apellido,usu.Email,usu.Movil,em.Nombre 'Empresa',RegistroMonitoreo.Fecha_Entrada,RegistroMonitoreo.Hora_Ingreso,RegistroMonitoreo.Hora_Salida FROM RegistroMonitoreo INNER JOIN usuario usu on usu.Cedula = RegistroMonitoreo.CC_usuario INNER JOIN Empresas em on em.Id_Empresa = usu.Id_Empresa ORDER BY usu.Nombre", Conne);
                DataTable Dt = new DataTable();
                Da.Fill(Dt);
                Dgv.DataSource = Dt;
                Conne.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo llenar el datagridview: " + ex.ToString());

            }
        }

        public void BuscarUsuarioAsistencia(DataGridView Dgv, string Jusuarioced, string JNombre)
        {
            try
            {
                MySqlConnection Conne = Conn.ObteConnetion();
                MySqlDataAdapter Da = new MySqlDataAdapter("SELECT usu.Cedula,usu.Nombre,usu.Apellido,usu.Email,usu.Movil,em.Nombre 'Empresa',RegistroMonitoreo.Fecha_Entrada,RegistroMonitoreo.Hora_Ingreso,RegistroMonitoreo.Hora_Salida FROM RegistroMonitoreo INNER JOIN usuario usu on usu.Cedula = RegistroMonitoreo.CC_usuario INNER JOIN Empresas em on em.Id_Empresa = usu.Id_Empresa WHERE usu.Cedula LIKE '" + Jusuarioced + "%' OR usu.Nombre LIKE '" + JNombre + "%' ORDER BY usu.Nombre", Conne);
                DataTable Dt = new DataTable();
                Da.Fill(Dt);
                Dgv.DataSource = Dt;
                Conne.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo llenar el Datagridview: " + ex.ToString());
            }
        }

        public void BuscarAsistenciaFecha(DataGridView Dgv, string FechaInicio, string FechaCierre)
        {
            try
            {
                MySqlConnection Conne = Conn.ObteConnetion();
                MySqlDataAdapter Da = new MySqlDataAdapter("SELECT usu.Cedula,usu.Nombre,usu.Apellido,usu.Email,usu.Movil,em.Nombre 'Empresa',RegistroMonitoreo.Fecha_Entrada,RegistroMonitoreo.Hora_Ingreso,RegistroMonitoreo.Hora_Salida FROM RegistroMonitoreo INNER JOIN usuario usu on usu.Cedula = RegistroMonitoreo.CC_usuario INNER JOIN Empresas em on em.Id_Empresa = usu.Id_Empresa WHERE Fecha_Entrada BETWEEN '" + FechaInicio + "' AND '" + FechaCierre + "' ORDER BY usu.Nombre", Conne);
                DataTable Dt = new DataTable();
                Da.Fill(Dt);
                Dgv.DataSource = Dt;
                Conne.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo llenar el Datagridview: " + ex.ToString());
            }
        }
        public void BuscarAsistenciaFechaEmpresa(DataGridView Dgv, string FechaInicio, string FechaCierre,int IdEmpre)
        {
            try
            {
                MySqlConnection Conne = Conn.ObteConnetion();
                MySqlDataAdapter Da = new MySqlDataAdapter("SELECT usu.Cedula,usu.Nombre,usu.Apellido,usu.Email,usu.Movil,em.Nombre 'Empresa',RegistroMonitoreo.Fecha_Entrada,RegistroMonitoreo.Hora_Ingreso,RegistroMonitoreo.Hora_Salida FROM RegistroMonitoreo INNER JOIN usuario usu on usu.Cedula = RegistroMonitoreo.CC_usuario INNER JOIN Empresas em on em.Id_Empresa = usu.Id_Empresa WHERE Fecha_Entrada BETWEEN '" + FechaInicio + "' AND '" + FechaCierre + "' AND RegistroMonitoreo.Id_Empresa = '"+ IdEmpre + "' ORDER BY usu.Nombre", Conne);
                DataTable Dt = new DataTable();
                Da.Fill(Dt);
                Dgv.DataSource = Dt;
                Conne.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo llenar el Datagridview: " + ex.ToString());
            }
        }

        public static int RefrescarActualizacion(int idusu, string cc)
        {
            int Result = -1;
            MySqlConnection Conne = Conn.ObteConnetion();
            MySqlCommand Command = new MySqlCommand(string.Format("SELECT usuario.id_usuario,usuario.Cedula,usuario.Huella,usuario.Nombre,usuario.Apellido,Rol.Descripcion as Rol, em.Nombre as Empresa,usuario.Movil,usuario.Email,usuario.Clave,estad.Descripcion as Estado,em.Id_Empresa FROM usuario INNER JOIN Empresas em on em.Id_Empresa = usuario.Id_Empresa INNER JOIN Estado estad on estad.IdEstado = usuario.IdEstado INNER JOIN Rol ON Rol.IdRol = usuario.Id_rol WHERE id_usuario = '{0}' AND Cedula = '{1}'", idusu, cc), Conne);
            MySqlDataReader Reader = Command.ExecuteReader();
            while (Reader.Read())
            {
                Result = 1;
                IdUsuarioo = Reader.GetInt32(0);
                CC = Reader.GetString(1);
                //HuellaDigi = (byte[])Reader.GetValue(2);
                HuellaDigi = Reader.GetString(2);
                Nombre = Reader.GetString(3);
                Apellido = Reader.GetString(4);
                Rol_usu = Reader.GetString(5);
                NombreEmpresa = Reader.GetString(6);
                Movil = Reader.GetString(7);
                Email = Reader.GetString(8);
                Clavee = Reader.GetString(9);
                IdEstado = Reader.GetString(10);
                IdEmpresa = Reader.GetInt32(11);
            }
            Conne.Close();
            return Result;
        }
        public static int GuardarEstado(Estadoo JEstado)
        {
            int Retorno = 0;
            MySqlConnection Conne = Conn.ObteConnetion();
            MySqlCommand Command = new MySqlCommand(string.Format("INSERT INTO Estado (Descripcion) values ('{0}')", JEstado.DescripEstado), Conne);
            Retorno = Command.ExecuteNonQuery();
            Conne.Close();
            return Retorno;
        }
        public static int ModificarEstado(Estadoo JEstado)
        {
            int Resultado = 0;
            MySqlConnection Conne = Conn.ObteConnetion();
            MySqlCommand Comando = new MySqlCommand(string.Format("UPDATE Estado SET Descripcion ='{0}' WHERE IdEstado='{1}'",
                JEstado.DescripEstado, JEstado.IdEstado), Conne);
            Resultado = Comando.ExecuteNonQuery();
            Conne.Close();
            return Resultado;
        }
        public static int EliminarEstado(Estadoo JEstado)
        {
            int Resultado = 0;
            MySqlConnection Conne = Conn.ObteConnetion();
            MySqlCommand Comando = new MySqlCommand(string.Format("DELETE FROM Estado WHERE IdEstado='{0}'", JEstado.IdEstado), Conne);
            Resultado = Comando.ExecuteNonQuery();
            Conne.Close();
            return Resultado;
        }
        public void CargarEstado(DataGridView Dgv)
        {
            try
            {
                MySqlConnection Conne = Conn.ObteConnetion();
                MySqlDataAdapter Em = new MySqlDataAdapter("SELECT * FROM Estado ORDER BY Descripcion", Conne);
                DataTable Dt = new DataTable();
                Em.Fill(Dt);
                Dgv.DataSource = Dt;
                Conne.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo llenar el datagridview: " + ex.ToString());
            }
        }

        public static int GuardarRol(TipoRol JRol)
        {
            int Retorno = 0;
            MySqlConnection Conne = Conn.ObteConnetion();
            MySqlCommand Command = new MySqlCommand(string.Format("INSERT INTO Rol (Descripcion) values ('{0}')", JRol.DescripRol), Conne);
            Retorno = Command.ExecuteNonQuery();
            Conne.Close();
            return Retorno;
        }
        public static int ModificarRol(TipoRol JRol)
        {
            int Resultado = 0;
            MySqlConnection Conne = Conn.ObteConnetion();
            MySqlCommand Comando = new MySqlCommand(string.Format("UPDATE Rol SET Descripcion ='{0}' WHERE IdRol='{1}'",
                JRol.DescripRol, JRol.Id_Rol), Conne);
            Resultado = Comando.ExecuteNonQuery();
            Conne.Close();
            return Resultado;
        }
        public static int EliminarRol(TipoRol JRol)
        {
            int Resultado = 0;
            MySqlConnection Conne = Conn.ObteConnetion();
            MySqlCommand Comando = new MySqlCommand(string.Format("DELETE FROM Rol WHERE IdRol='{0}'", JRol.Id_Rol), Conne);
            Resultado = Comando.ExecuteNonQuery();
            Conne.Close();
            return Resultado;
        }
        public void CargarRol(DataGridView Dgv)
        {
            try
            {
                MySqlConnection Conne = Conn.ObteConnetion();
                MySqlDataAdapter Em = new MySqlDataAdapter("SELECT * FROM Rol ORDER BY Descripcion", Conne);
                DataTable Dt = new DataTable();
                Em.Fill(Dt);
                Dgv.DataSource = Dt;
                Conne.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo llenar el datagridview: " + ex.ToString());
            }
        }
    }
}
