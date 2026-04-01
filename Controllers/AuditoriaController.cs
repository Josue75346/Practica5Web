using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaFarmacia.Data;
using SistemaFarmacia.Models;

namespace SistemaFarmacia.Controllers
{
    [Authorize(Policy = "SoloAdmin")]
    public class AuditoriaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuditoriaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var datos = _context.Auditoria
                .OrderByDescending(a => a.Fecha)
                .ToList();

            return View(datos);
        }
    }
}