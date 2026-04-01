using System.ComponentModel.DataAnnotations;

namespace SistemaFarmacia.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50)]
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; }
    }
}