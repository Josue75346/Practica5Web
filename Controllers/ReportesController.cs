using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaFarmacia.Data;
using SistemaFarmacia.Services;

namespace SistemaFarmacia.Controllers
{
    public class ReportesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ======================================================
        // REPORTE: MEDICAMENTOS VENCIDOS (VISTA)
        // ======================================================
        public async Task<IActionResult> Vencidos()
        {
            var hoy = DateTime.Today;

            var lista = await _context.Medicamentos
                .Include(m => m.Categoria)
                .Where(m => m.FechaVencimiento < hoy)
                .ToListAsync();

            return View(lista);
        }

        // ======================================================
        // PDF: MEDICAMENTOS VENCIDOS
        // ======================================================
        public async Task<IActionResult> VencidosPdf()
        {
            var data = await _context.Medicamentos
                .Include(c => c.Categoria)
                .Where(m => m.FechaVencimiento < DateTime.Now)
                .ToListAsync();

            var pdf = PdfGenerator.GenerarReporte("Medicamentos Vencidos", data);

            return File(pdf, "application/pdf", "Vencidos.pdf");
        }

        // ======================================================
        // REPORTE: POR VENCER (30 días) - VISTA
        // ======================================================
        public async Task<IActionResult> PorVencer()
        {
            var hoy = DateTime.Today;
            var limite = hoy.AddDays(30);

            var lista = await _context.Medicamentos
                .Include(m => m.Categoria)
                .Where(m => m.FechaVencimiento >= hoy &&
                            m.FechaVencimiento <= limite)
                .ToListAsync();

            return View(lista);
        }

        // ======================================================
        // PDF: POR VENCER (30 días)
        // ======================================================
        public async Task<IActionResult> PorVencerPdf()
        {
            var fecha = DateTime.Now.AddDays(30);

            var data = await _context.Medicamentos
                .Include(c => c.Categoria)
                .Where(m => m.FechaVencimiento >= DateTime.Now &&
                            m.FechaVencimiento <= fecha)
                .ToListAsync();

            var pdf = PdfGenerator.GenerarReporte("Medicamentos por Vencer (30 días)", data);

            return File(pdf, "application/pdf", "PorVencer.pdf");
        }

        // ======================================================
        // REPORTE: BAJO STOCK (VISTA)
        // ======================================================
        public async Task<IActionResult> BajoStock()
        {
            var lista = await _context.Medicamentos
                .Include(m => m.Categoria)
                .Where(m => m.Stock < 10)
                .ToListAsync();

            return View(lista);
        }

        // ======================================================
        // PDF: BAJO STOCK
        // ======================================================
        public async Task<IActionResult> BajoStockPdf()
        {
            var data = await _context.Medicamentos
                .Include(c => c.Categoria)
                .Where(m => m.Stock < 10)
                .ToListAsync();

            var pdf = PdfGenerator.GenerarReporte("Medicamentos con Bajo Stock", data);

            return File(pdf, "application/pdf", "BajoStock.pdf");
        }

        // ======================================================
        // REPORTE: INVENTARIO COMPLETO (VISTA)
        // ======================================================
        public async Task<IActionResult> Inventario()
        {
            var lista = await _context.Medicamentos
                .Include(m => m.Categoria)
                .OrderBy(m => m.Nombre)
                .ToListAsync();

            return View(lista);
        }

        // ======================================================
        // PDF: INVENTARIO COMPLETO
        // ======================================================
        public async Task<IActionResult> InventarioPdf()
        {
            var data = await _context.Medicamentos
                .Include(c => c.Categoria)
                .OrderBy(m => m.Nombre)
                .ToListAsync();

            var pdf = PdfGenerator.GenerarReporte("Inventario Completo", data);

            return File(pdf, "application/pdf", "Inventario.pdf");
        }

        // ======================================================
        // FILTROS AVANZADOS (VISTA)
        // ======================================================
        public async Task<IActionResult> Filtros(
            string? nombre,
            int? categoriaId,
            DateTime? fechaDesde,
            DateTime? fechaHasta
        )
        {
            var query = _context.Medicamentos
                .Include(m => m.Categoria)
                .AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(m => m.Nombre.Contains(nombre));

            if (categoriaId.HasValue)
                query = query.Where(m => m.CategoriaId == categoriaId);

            if (fechaDesde.HasValue)
                query = query.Where(m => m.FechaVencimiento >= fechaDesde);

            if (fechaHasta.HasValue)
                query = query.Where(m => m.FechaVencimiento <= fechaHasta);

            ViewBag.Categorias = await _context.Categorias.ToListAsync();

            return View(await query.ToListAsync());
        }
    }
}