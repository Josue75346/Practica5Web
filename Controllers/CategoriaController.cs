using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SistemaFarmacia.Data;
using SistemaFarmacia.Models;

namespace SistemaFarmacia.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Authorize(Policy = "SoloAdmin")]
    public class CategoriaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTAR
        public IActionResult Index()
        {
            var lista = _context.Categorias.ToList();
            return View(lista);
        }

        // CREAR GET
        public IActionResult Create()
        {
            return View();
        }

        // CREAR POST
        [HttpPost]
        public IActionResult Create(Categoria categoria)
        {
            if (!ModelState.IsValid)
                return View(categoria);

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            TempData["mensaje"] = "✔ Categoría creada correctamente";
            return RedirectToAction("Index");
        }

        // EDITAR GET
        public IActionResult Edit(int id)
        {
            var categoria = _context.Categorias.Find(id);

            if (categoria == null)
                return NotFound();

            return View(categoria);
        }

        // EDITAR POST
        [HttpPost]
        public IActionResult Edit(Categoria categoria)
        {
            if (!ModelState.IsValid)
                return View(categoria);

            _context.Categorias.Update(categoria);
            _context.SaveChanges();

            TempData["mensaje"] = "✔ Categoría actualizada correctamente";

            return RedirectToAction("Index");
        }

        // ELIMINAR GET
        public IActionResult Delete(int id)
        {
            var categoria = _context.Categorias.Find(id);

            if (categoria == null)
                return NotFound();

            return View(categoria);
        }

        // ELIMINAR POST
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            var categoria = _context.Categorias.Find(id);

            if (categoria == null)
                return NotFound();

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            TempData["mensaje"] = "✔ Categoría eliminada correctamente";

            return RedirectToAction("Index");
        }
    }
}