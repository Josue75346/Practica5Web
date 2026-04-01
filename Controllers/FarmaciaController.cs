using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SistemaFarmacia.Controllers
{
    [Authorize(Roles = "Administrador,Farmaceutico")]
    public class FarmaciaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}