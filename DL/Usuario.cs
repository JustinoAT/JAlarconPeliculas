using System;
using System.Collections.Generic;

namespace DL;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? Nombre { get; set; }

    public byte[]? Contrasena { get; set; }
}
