using SistemaFarmacia.Data;
using SistemaFarmacia.Models;
using Microsoft.AspNetCore.Http;

namespace SistemaFarmacia.Services
{
    public class BitacoraService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _http;

        public BitacoraService(ApplicationDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }

        public async Task RegistrarAsync(string accion)
        {
            var user = _http.HttpContext.User.Identity.IsAuthenticated
                ? _http.HttpContext.User.Identity.Name
                : "Anónimo";

            var registro = new Bitacora
            {
                UsuarioId = user,
                Accion = accion,
                Fecha = DateTime.Now
            };

            _context.Bitacora.Add(registro);
            await _context.SaveChangesAsync();
        }
    }
}