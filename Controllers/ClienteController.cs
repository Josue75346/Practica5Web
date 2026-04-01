using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SistemaFarmacia.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class ClienteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}