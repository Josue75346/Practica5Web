using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaFarmacia.Data;
using Microsoft.EntityFrameworkCore;

namespace SistemaFarmacia.Controllers
{
    [Authorize(Policy = "SoloAdmin")]
    public class BitacoraController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BitacoraController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var datos = await _context.Bitacora
                .OrderByDescending(b => b.Fecha)
                .ToListAsync();

            return View(datos);
        }
    }
}