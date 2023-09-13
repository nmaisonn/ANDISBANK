using Microsoft.AspNetCore.Mvc;

namespace ANDIS_II.Controllers;

[ApiController]
[Route("[controller]")]
public class LoanController : ControllerBase
{
    public LoanController(ILogger<WeatherForecastController> logger)
    {
    }

    [HttpGet(Name = "api/v1/loan/type")]
    public LoanType[] Get()
    {
        LoanType[] arr = new LoanType[]
        {
            new LoanType { Name = "Prestamos de autos" },
            new LoanType { Name = "Prestamos de casas" },
            new LoanType { Name = "Prestamos bancarios" }
        };

        return arr;
    }
}


class LoanType
{
    public string Name { get; set; }
}