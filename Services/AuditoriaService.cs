using Microsoft.AspNetCore.Http;
using SistemaFarmacia.Data;
using SistemaFarmacia.Models;

namespace SistemaFarmacia.Services
{
    public class AuditoriaService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _http;

        public AuditoriaService(ApplicationDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }

        public async Task RegistrarAsync(string accion, string controlador)
        {
            var user = _http.HttpContext.User.Identity.IsAuthenticated
                ? _http.HttpContext.User.Identity.Name
                : "Anónimo";

            var registro = new Auditoria
            {
                UsuarioId = user,
                Accion = accion,
                Controlador = controlador,
                Ip = _http.HttpContext.Connection.RemoteIpAddress.ToString(),
                Fecha = DateTime.Now
            };

            _context.Auditoria.Add(registro);
            await _context.SaveChangesAsync();
        }
    }
}