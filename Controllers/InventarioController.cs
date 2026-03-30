using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaFarmacia.Data;

namespace SistemaFarmacia.Controllers
{
    [Authorize(Roles = "Administrador,Farmaceutico")]
    public class InventarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var hoy = DateTime.Now;

            var vencidos = await _context.Medicamentos
                .Where(m => m.FechaVencimiento < hoy)
                .OrderBy(m => m.FechaVencimiento)
                .Include(m => m.Categoria)
                .Include(m => m.Estante)
                .ToListAsync();

            var porVencer30 = await _context.Medicamentos
                .Where(m => m.FechaVencimiento >= hoy &&
                            m.FechaVencimiento <= hoy.AddDays(30))
                .OrderBy(m => m.FechaVencimiento)
                .Include(m => m.Categoria)
                .Include(m => m.Estante)
                .ToListAsync();

            var porVencer60 = await _context.Medicamentos
                .Where(m => m.FechaVencimiento > hoy.AddDays(30) &&
                            m.FechaVencimiento <= hoy.AddDays(60))
                .OrderBy(m => m.FechaVencimiento)
                .Include(m => m.Categoria)
                .Include(m => m.Estante)
                .ToListAsync();

            var porVencer90 = await _context.Medicamentos
                .Where(m => m.FechaVencimiento > hoy.AddDays(60) &&
                            m.FechaVencimiento <= hoy.AddDays(90))
                .OrderBy(m => m.FechaVencimiento)
                .Include(m => m.Categoria)
                .Include(m => m.Estante)
                .ToListAsync();

            var bajoStock = await _context.Medicamentos
                .Where(m => m.Stock < 5)
                .OrderBy(m => m.Stock)
                .Include(m => m.Categoria)
                .Include(m => m.Estante)
                .ToListAsync();

            ViewBag.Vencidos = vencidos;
            ViewBag.PorVencer30 = porVencer30;
            ViewBag.PorVencer60 = porVencer60;
            ViewBag.PorVencer90 = porVencer90;
            ViewBag.BajoStock = bajoStock;

            return View();
        }
    }
}