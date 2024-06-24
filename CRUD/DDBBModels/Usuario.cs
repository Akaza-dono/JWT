using System;
using System.Collections.Generic;

namespace CRUD.DDBBModels
{
    public partial class Usuario
    {
        public int IdUsuario { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Correo { get; set; }
        public bool IsActive { get; set; }
        public string? Password { get; set; }
    }
}
