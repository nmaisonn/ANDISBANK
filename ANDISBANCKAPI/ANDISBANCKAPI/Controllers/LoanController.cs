using Microsoft.AspNetCore.Mvc;


using ANDISBANCKAPI;
using System.Text.Json;
using System.Collections.Generic;

namespace ANDIS_II.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class LoanController : ControllerBase
{
    public LoanController()
    {
    }

    [HttpGet("loan/type")]
    public string Get()
    {
        try
        {
            string loanTypes = "./loanTypes.json";

            if (System.IO.File.Exists(loanTypes))
            {
                string[] lines = System.IO.File.ReadAllLines(loanTypes);

                return string.Join("\n", lines);
            }
            else
            {
                string error = "El archivo no existe.";
                Console.WriteLine(error);
                return error;
            }
        }
        catch (Exception ex)
        {
            string error = "Ocurrió un error al leer el archivo JSON: " + ex.Message;
            Console.WriteLine(error);
            return error;
        }
    }

    /*[HttpPost("loan/request/{userId}")]
    public async Task<IActionResult> Post([FromRoute] int userId, [FromBody] LoanRequest loanRequest)
    {
        // if (resp)
        Console.WriteLine(loanRequest);
        return StatusCode(StatusCodes.Status201Created);
        // else
            // return StatusCode(StatusCodes.Status500InternalServerError);
    }*/

    /*
     [HttpGet("loan/{id}")]
    public IActionResult GetLoan(int id)
    {
        try
        {
            string loans = "./loans.json";

            List<Loan> loanList = new List<Loan>();

            if (System.IO.File.Exists(loans))
            {
                string jsonLoans = System.IO.File.ReadAllText(loans);

                loanList = JsonContent.DeserializeObject<List<Loan>>(jsonLoans);

                foreach (Loan loan in loanList)
                {
                    if (loan.Id.Equals(id))
                    {
                        return Ok(loan);
                    }
                }

                return NotFound() ;
            }
        }
        catch (Exception ex)
        {
            string error = "Ocurrió un error al leer el archivo JSON: " + ex.Message;
            Console.WriteLine(error);
            return NotFound();
        }
    }*/

    [HttpGet("loan/all/{userId}")]
    public IActionResult GetAllLoans(int userId)
    {
        try
        {
            string jsonFilePath = "./loans.json";

            if (System.IO.File.Exists(jsonFilePath))
            {
                string json = System.IO.File.ReadAllText(jsonFilePath);
                List<Loan> loanList = JsonSerializer.Deserialize<List<Loan>>(json);

                if (loanList != null)
                {
                    // Filtrar los préstamos por UserId
                    List<Loan> resultList = loanList.Where(loan => loan.UserId == userId).ToList();
                    return Ok(resultList);
                }
                else
                {
                    string error = "No se pudo deserializar el archivo JSON correctamente.";
                    Console.WriteLine(error);
                    return BadRequest();
                }
            }
            else
            {
                string error = "El archivo no existe.";
                Console.WriteLine(error);
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            string error = "Ocurrió un error al leer el archivo JSON: " + ex.Message;
            Console.WriteLine(error);
            return BadRequest(error);
        }
    }

}



