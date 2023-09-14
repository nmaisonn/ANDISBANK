using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ANDIS_II.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class LoanController : ControllerBase
{
    public LoanController(ILogger<LoanType> logger)
    {
    }

    [HttpGet("loan/type")]
    public IEnumerable<string> Get()
    {
        try
        {
            string loanTypes = "../loanTypes.json";

            if (File.Exists(loanTypes))
            {
                string[] lines = File.ReadAllLines(loanTypes);

                return lines
            }
            else
            {
                Console.WriteLine("El archivo no existe.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ocurrió un error al leer el archivo JSON: " + ex.Message);
        }
    }

    [HttpPost("loan/request/{userId}")]
    public async Task<IActionResult> RequestLoan([FromRoute] int userId, [FromBody] LoanRequest loanRequest)
    {
        // if (resp)
            return StatusCode(StatusCodes.Status201Created);
        // else
            // return StatusCode(StatusCodes.Status500InternalServerError);
    }
}



