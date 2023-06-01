﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Usuarios
    {
        public int IdUsuario { get; set; }
        public string Documento { get; set;}
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public Rol oRol { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro {  get; set; }
    }
}
