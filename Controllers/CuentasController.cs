using System.Globalization;
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
            var _culture = new CultureInfo("es-MX");
            var cuentas = arquosContext.VwCatPadrons
                .Where(pad => EF.Functions.Like(pad.Estatus, "Activo"))
                .ToList()
                .Select(pad => new
                {
                    pad.IdPadron,
                    pad.IdCuenta,
                    pad.RazonSocial,
                    pad.Sb,
                    pad.Sector,
                    pad.Localizacion,
                    pad.Total,
                    pad.Af,
                    pad.Mf,
                    pad.FechaFacturaAct,
                    periodo = new DateTime(Convert.ToInt32(pad.Af!.Value), Convert.ToInt32(pad.Mf!.Value), 1).ToString("MMMM yyyy", _culture)
                })
                .ToList();

            Console.WriteLine($"Total cuentas: {cuentas.Count}");

            return Ok(cuentas);
        }
    }
}
