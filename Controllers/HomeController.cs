using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fortin.CuentasAPI.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("/")]
        public IActionResult Home()
        {
            return Ok(new
            {
                Title = "Fortin Cuentas API"
            });
        }
    }
}
