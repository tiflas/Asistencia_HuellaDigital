using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMonitoreo.Controller
{
    public class Huella
    {
        public int IdUsuario { get; set; }
        public string Cedula { get; set; }
        //public byte[]? HuellaDigi { get; set; }
        public string HuellaDigi { get; set; }
        public int IdEmpresa { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }     
    }
}
