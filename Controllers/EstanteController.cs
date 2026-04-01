using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaFarmacia.Data;
using SistemaFarmacia.Models;

namespace SistemaFarmacia.Controllers
{
    [Authorize(Roles = "Administrador,Farmaceutico")]
    [Authorize(Policy = "SoloAdmin")]

    public class EstanteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstanteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTA
        public async Task<IActionResult> Index()
        {
            return View(await _context.Estantes.ToListAsync());
        }

        // CREAR GET
        public IActionResult Create()
        {
            return View();
        }

        // CREAR POST
        [HttpPost]
        public async Task<IActionResult> Create(Estante estante)
        {
            if (!ModelState.IsValid)
                return View(estante);

            _context.Estantes.Add(estante);
            await _context.SaveChangesAsync();

            TempData["success"] = "Estante registrado correctamente";
            return RedirectToAction(nameof(Index));
        }

        // EDIT GET
        public async Task<IActionResult> Edit(int id)
        {
            var estante = await _context.Estantes.FindAsync(id);
            if (estante == null)
                return NotFound();

            return View(estante);
        }

        // EDIT POST
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Estante estante)
        {
            if (id != estante.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(estante);

            _context.Update(estante);
            await _context.SaveChangesAsync();

            TempData["success"] = "Estante actualizado correctamente";
            return RedirectToAction(nameof(Index));
        }

        // DELETE GET
        public async Task<IActionResult> Delete(int id)
        {
            var estante = await _context.Estantes.FindAsync(id);
            if (estante == null)
                return NotFound();

            return View(estante);
        }

        // DELETE POST
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estante = await _context.Estantes.FindAsync(id);
            if (estante == null)
                return NotFound();

            _context.Estantes.Remove(estante);
            await _context.SaveChangesAsync();

            TempData["success"] = "Estante eliminado correctamente";
            return RedirectToAction(nameof(Index));
        }
    }
}