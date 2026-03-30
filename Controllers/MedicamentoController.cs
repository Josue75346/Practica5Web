using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaFarmacia.Data;
using SistemaFarmacia.Models;
using SistemaFarmacia.Services;

namespace SistemaFarmacia.Controllers
{
    [Authorize(Roles = "Administrador,Farmaceutico")]
    [Authorize(Policy = "AdminFarmaceutico")]
    public class MedicamentoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AuditoriaService _auditoria;

        public MedicamentoController(ApplicationDbContext context, AuditoriaService auditoria)
        {
            _context = context;
            _auditoria = auditoria;
        }

        // LISTADO + FILTROS + ALERTAS
        public async Task<IActionResult> Index(string buscar, int? categoriaId, int? estanteId)
        {
            var query = _context.Medicamentos
                .Include(m => m.Categoria)
                .Include(m => m.Estante)
                .AsQueryable();

            if (!string.IsNullOrEmpty(buscar))
                query = query.Where(m => m.Nombre.Contains(buscar));

            if (categoriaId.HasValue)
                query = query.Where(m => m.CategoriaId == categoriaId);

            if (estanteId.HasValue)
                query = query.Where(m => m.EstanteId == estanteId);

            ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nombre");
            ViewBag.Estantes = new SelectList(_context.Estantes, "Id", "Nombre");

            return View(await query.ToListAsync());
        }

        // CREATE GET
        public IActionResult Create()
        {
            ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nombre");
            ViewBag.Estantes = new SelectList(_context.Estantes, "Id", "Nombre");
            return View();
        }

        // CREATE POST
        [HttpPost]
        public async Task<IActionResult> Create(Medicamento m)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nombre");
                ViewBag.Estantes = new SelectList(_context.Estantes, "Id", "Nombre");
                return View(m);
            }

            _context.Medicamentos.Add(m);
            await _context.SaveChangesAsync();

            // 🔥 REGISTRAR AUDITORÍA
            await _auditoria.RegistrarAsync("Creó un medicamento", "Medicamento");

            TempData["success"] = "Medicamento registrado correctamente";
            return RedirectToAction(nameof(Index));
        }

        // EDIT GET
        public async Task<IActionResult> Edit(int id)
        {
            var m = await _context.Medicamentos.FindAsync(id);
            if (m == null)
                return NotFound();

            ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nombre");
            ViewBag.Estantes = new SelectList(_context.Estantes, "Id", "Nombre");

            return View(m);
        }

        // EDIT POST
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Medicamento m)
        {
            if (id != m.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nombre");
                ViewBag.Estantes = new SelectList(_context.Estantes, "Id", "Nombre");
                return View(m);
            }

            _context.Medicamentos.Update(m);
            await _context.SaveChangesAsync();

            // 🔥 REGISTRAR AUDITORÍA
            await _auditoria.RegistrarAsync("Editó un medicamento", "Medicamento");

            TempData["success"] = "Medicamento actualizado correctamente";
            return RedirectToAction(nameof(Index));
        }

        // DELETE GET
        public async Task<IActionResult> Delete(int id)
        {
            var m = await _context.Medicamentos
                .Include(x => x.Categoria)
                .Include(x => x.Estante)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (m == null)
                return NotFound();

            return View(m);
        }

        // DELETE POST
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var m = await _context.Medicamentos.FindAsync(id);

            if (m == null)
                return NotFound();

            _context.Medicamentos.Remove(m);
            await _context.SaveChangesAsync();

            // 🔥 REGISTRAR AUDITORÍA
            await _auditoria.RegistrarAsync("Eliminó un medicamento", "Medicamento");

            TempData["success"] = "Medicamento eliminado correctamente";
            return RedirectToAction(nameof(Index));
        }
    }
}