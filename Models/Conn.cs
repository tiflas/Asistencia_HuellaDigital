using AppMonitoreo.Controller;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace AppMonitoreo.Models
{
    class Conn
    {
        public static MySqlConnection ObteConnetion()
        {
            Encrypt64 Encry = new Encrypt64();
            /*var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Conn)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("AppMonitoreo.appsetting.json"); // Archivo -> Propiedades -> Recuerso Incrustado
            var StreamReader = new StreamReader(stream).ReadToEnd();
            RootObject Cone = JsonConvert.DeserializeObject<RootObject>(StreamReader);*/
            //MySqlConnection Connection = new MySqlConnection(Encry.Desencriptar(Cone.Serv));

            string Servv = "";
            string Sercrip = Encry.Encripta(Servv);

            MySqlConnection Connection = new MySqlConnection(Encry.Desencriptar(Sercrip));
            try
            {
                Connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al conectar a la base de datos: " + ex.Message);
            }
            return Connection;
        }
    }
}
