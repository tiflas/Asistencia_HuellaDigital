using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMonitoreo.Controller
{
    class Empresa
    {
        public int IdEmpresa { get; set; }
        public string RutEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public int Rol { get; set; }
        public int IdEstado { get; set; }
        public string FechaCreacion { get; set; }
        public string FechaActualizacion { get; set; }
    }
}
