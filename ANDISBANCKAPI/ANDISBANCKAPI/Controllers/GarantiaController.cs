using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Serilog;
  

namespace ANDISBANCKAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GarantiasController : ControllerBase
    {

        private static string[] garantias = new[]
        {
            "inmueble","vehiculo","terreno",
        };
        private IOutputCacheStore _cache;
        public GarantiasController(IOutputCacheStore cache)
        {
            _cache = cache;
        }

        [HttpGet(Name = "GetGarantias")]
        [OutputCache(PolicyName = "AlbañilesPolicy")]
        public IEnumerable<string> GetGarantias()
        {
            Thread.Sleep(10000);
            Log.Information("GetGarantias ");
            return garantias;
        }

    }
}