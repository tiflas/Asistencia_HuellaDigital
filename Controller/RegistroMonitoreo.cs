using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMonitoreo.Controller
{
    class RegistroMonitoreo
    {
        public string Id_usuario { get; set; }
        public int Id_empresa { get; set; }
        public string NomEmpresa { get; set; }
        public string FechaEntrada { get; set; }
        public string TiempoIngreso { get; set; }
        public string TiempoSalida { get; set; }
    }
}
