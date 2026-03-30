using System;

namespace SistemaFarmacia.Models
{
        public class Auditoria
        {
            public int Id { get; set; }
            public string UsuarioId { get; set; }
            public string Accion { get; set; }
            public string Controlador { get; set; }
            public string Ip { get; set; }
            public DateTime Fecha { get; set; }
        }
    }
