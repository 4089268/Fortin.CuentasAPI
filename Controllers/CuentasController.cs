using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fortin.CuentasAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

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
            var cuentas = new List<dynamic>();
            var _culture = new CultureInfo("es-MX");
            var query = @"SELECT
                            p.id_padron,
                            p.id_cuenta,
                            p.razon_social,
                            p.sb,
                            p.sector,
                            p._localizacion as localizacion,
                            r.Redondeado as total,
                            p.af,
                            p.mf,
                            p._fecha_factura_act as fecha_factura_act,
                            CONCAT(p.af, '-', RIGHT(Concat(REPLICATE('0',2), mf),2) ) AS periodo,
                            p._estatus
                        FROM [Padron].[vw_Cat_Padron] p
                        CROSS APPLY [Global].[uif_Redondear](P.total) r
                        WHERE p._estatus like 'Activo'";

            using (var connection = new SqlConnection(arquosContext.Database.GetConnectionString()))
            {
                connection.Open();
                var sqlCommand = new SqlCommand(query, connection)
                {
                    CommandType = System.Data.CommandType.Text,
                };

                using (var reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cuentas.Add(new
                            {
                                IdPadron = reader["id_padron"],
                                IdCuenta = reader["id_cuenta"],
                                RazonSocial = reader["razon_social"],
                                Sb = reader["sb"],
                                Sector = reader["sector"],
                                Localizacion = reader["localizacion"],
                                Total = reader["total"],
                                Af = reader["af"],
                                Mf = reader["mf"],
                                FechaFacturaAct = reader["fecha_factura_act"],
                                Periodo = reader["periodo"],
                                Estatus = reader["_estatus"]
                            }
                        );
                    }
                }
                connection.Close();
            }

            Console.WriteLine($"Total cuentas: {cuentas.Count}");
            return Ok(cuentas);
        }

        [HttpGet("ef")]
        public IActionResult ObtenerCuentasOld()
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
