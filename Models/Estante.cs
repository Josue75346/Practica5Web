using System.ComponentModel.DataAnnotations;

namespace SistemaFarmacia.Models
{
    public class Estante
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del estante es obligatorio")]
        [StringLength(20)]
        public string Nombre { get; set; }   // Ej: A1, B2, C3, Refrigerado

        [Required(ErrorMessage = "La ubicación es obligatoria")]
        [StringLength(100)]
        public string Ubicacion { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; }
    }
}