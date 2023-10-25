using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }
        public string Nombre { get; set; }
        public byte[] Contrasena { get; set; }

        public List<object> Usuarios { get; set; }
    }
}
