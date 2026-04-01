using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaFarmacia.Data;
using System.Linq;

namespace SistemaFarmacia.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class CatalogoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CatalogoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var medicamentos = _context.Medicamentos.ToList();
            return View(medicamentos);
        }
    }
}