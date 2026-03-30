using System.ComponentModel.DataAnnotations;

namespace SistemaFarmacia.Models
{
    public class Medicamento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [Range(0.1, 999999, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Required]
        [Range(0, 999999, ErrorMessage = "El stock debe ser igual o mayor a 0")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Debe ingresar la fecha de vencimiento")]
        public DateTime FechaVencimiento { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        [Required]
        public int EstanteId { get; set; }

        [StringLength(200)]
        public string? Descripcion { get; set; }

        public bool Estado { get; set; } = true;

        // RELACIONES
        public Categoria Categoria { get; set; }
        public Estante Estante { get; set; }

        // PROPIEDADES CALCULADAS
        public bool EstaVencido => FechaVencimiento < DateTime.Now;

        public bool PorVencer30Dias =>
            FechaVencimiento >= DateTime.Now &&
            FechaVencimiento <= DateTime.Now.AddDays(30);

        public bool BajoStock => Stock <= 5;
    }
}