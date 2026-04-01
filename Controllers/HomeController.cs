using Microsoft.AspNetCore.Mvc;

namespace SistemaFarmacia.Controllers
{
    public class HomeController : Controller
    {
        // Acción principal - Página de inicio
        public IActionResult Index()
        {
            return View();
        }

        // Acción para mostrar mensaje de sin permisos
        public IActionResult SinPermisos()
        {
            return View();
        }

        // Acción de Error genérico (opcional)
        public IActionResult Error()
        {
            return View();
        }
    }
}