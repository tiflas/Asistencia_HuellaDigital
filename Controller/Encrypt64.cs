using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMonitoreo.Controller
{
    internal class Encrypt64
    {
        public string Encripta(string Mesagge)
        {
            string result = string.Empty;
            byte[] encrypted = System.Text.Encoding.Unicode.GetBytes(Mesagge);
            result = Convert.ToBase64String(encrypted);
            return result;
        }
        public string Desencriptar(string MessageENC)
        {
            string result = string.Empty;
            byte[] decrypted = Convert.FromBase64String(MessageENC);
            result = System.Text.Encoding.Unicode.GetString(decrypted);
            return result;
        }
    }
}
