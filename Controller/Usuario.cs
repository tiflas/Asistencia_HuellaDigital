using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMonitoreo.Controller
{
    class Usuario
    {
        public int Id_Usuario { get; set; }
        public string Cedula { get; set; }
        public string Huella { get; set; }
        //public byte[]? Huella { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Rol { get; set; }
        public string IdEmpresa { get; set; }
        public string Movil { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
        public int IdEstado { get; set; }
        public string Fecha_Creacion { get; set; }
        public string Fecha_Actualizacion { get; set; }
    }
}
