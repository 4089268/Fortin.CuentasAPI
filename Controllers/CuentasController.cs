using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fortin.CuentasAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace Fortin.CuentasAPI
{
    [Auth]
    [Route("api/[controller]")]
    [ApiController]
    public class CuentasController(ArquosV251Context arquosV251Context) : ControllerBase
    {

        private readonly ArquosV251Context arquosContext = arquosV251Context;

        [HttpGet]
        public IActionResult ObtenerCuentas()
        {
            var cuentas = arquosContext.VwCatPadrons
                .Where(pad => EF.Functions.Like(pad.Estatus, "Activo"))
                .ToList();

            Console.WriteLine($"Total cuentas: {cuentas.Count}");

            return Ok(cuentas);
        }
    }
}
