using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaFarmacia.Data;
using SistemaFarmacia.Models;
using SistemaFarmacia.Services;

namespace SistemaFarmacia.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly BitacoraService _bitacora;

        // ============================
        //     CONSTRUCTOR
        // ============================
        public UsersController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            BitacoraService bitacora)
        {
            _context = context;
            _userManager = userManager;
            _bitacora = bitacora;
        }

        // ============================
        //     LISTADO DE USUARIOS
        // ============================
        public async Task<IActionResult> Index()
        {
            var lista = await _context.Users.ToListAsync();
            return View(lista);
        }

        // ============================
        //     EDITAR GET
        // ============================
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // ============================
        //     EDITAR POST
        // ============================
        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser u)
        {
            if (!ModelState.IsValid)
                return View(u);

            var dbUser = await _context.Users.FindAsync(u.Id);
            if (dbUser == null)
                return NotFound();

            dbUser.Nombre = u.Nombre;
            dbUser.Email = u.Email;
            dbUser.UserName = u.Email;

            _context.Users.Update(dbUser);
            await _context.SaveChangesAsync();

            // ⭐ BITÁCORA
            await _bitacora.RegistrarAsync($"Editó al usuario {u.Nombre}");

            TempData["mensaje"] = "Usuario actualizado correctamente";
            return RedirectToAction("Index");
        }

        // ============================
        //     BLOQUEAR USUARIO
        // ============================
        public async Task<IActionResult> Bloquear(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(10);
            await _context.SaveChangesAsync();

            // ⭐ BITÁCORA
            await _bitacora.RegistrarAsync($"Bloqueó al usuario {user.Nombre}");

            TempData["mensaje"] = "Usuario bloqueado";
            return RedirectToAction("Index");
        }

        // ============================
        //     DESBLOQUEAR USUARIO
        // ============================
        public async Task<IActionResult> Desbloquear(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            user.LockoutEnd = null;
            await _context.SaveChangesAsync();

            // ⭐ BITÁCORA
            await _bitacora.RegistrarAsync($"Desbloqueó al usuario {user.Nombre}");

            TempData["mensaje"] = "Usuario desbloqueado";
            return RedirectToAction("Index");
        }

        // ============================
        //     ELIMINAR USUARIO GET
        // ============================
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // ============================
        //     ELIMINAR USUARIO POST
        // ============================
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var u = await _context.Users.FindAsync(id);

            if (u == null)
                return NotFound();

            _context.Users.Remove(u);
            await _context.SaveChangesAsync();

            // ⭐ BITÁCORA
            await _bitacora.RegistrarAsync($"Eliminó al usuario {u.Nombre}");

            TempData["mensaje"] = "Usuario eliminado";
            return RedirectToAction("Index");
        }
    }
}