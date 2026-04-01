namespace SistemaFarmacia.Models
{
    public class Bitacora
    {
        public int Id { get; set; }

        public string UsuarioId { get; set; }
        public ApplicationUser Usuario { get; set; }

        public string Accion { get; set; }
        public DateTime Fecha { get; set; }
    }
}
