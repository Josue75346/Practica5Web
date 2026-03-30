using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaFarmacia.Data;

namespace SistemaFarmacia.Controllers
{
    // Si ambos roles pueden ver, dejamos ambos:
    [Authorize(Roles = "Administrador, Farmaceutico")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // ------------------------------------
            // 1. Datos base del inventario
            // ------------------------------------
            var medicamentos = await _context.Medicamentos
                .Include(m => m.Categoria) // NECESARIO PARA EL GRÁFICO
                .ToListAsync();

            var totalMedicamentos = medicamentos.Count;
            var vencidos = medicamentos.Count(m => m.EstaVencido);
            var porVencer30 = medicamentos.Count(m => m.PorVencer30Dias);
            var bajoStock = medicamentos.Count(m => m.BajoStock);

            var totalCategorias = await _context.Categorias.CountAsync();
            var totalEstantes = await _context.Estantes.CountAsync();

            // ------------------------------------
            // 2. Gráfico: Medicamentos por Categoría
            // ------------------------------------
            var categorias = await _context.Medicamentos
                .Include(m => m.Categoria)
                .GroupBy(m => m.Categoria.Nombre)
                .Select(g => new
                {
                    Categoria = g.Key,
                    Cantidad = g.Count()
                }).ToListAsync();

            // ------------------------------------
            // 3. Enviar datos a la vista
            // ------------------------------------
            ViewBag.TotalMedicamentos = totalMedicamentos;
            ViewBag.Vencidos = vencidos;
            ViewBag.PorVencer30 = porVencer30;
            ViewBag.BajoStock = bajoStock;

            ViewBag.TotalCategorias = totalCategorias;
            ViewBag.TotalEstantes = totalEstantes;

            ViewBag.Categorias = categorias;

            return View();
        }
    }
}