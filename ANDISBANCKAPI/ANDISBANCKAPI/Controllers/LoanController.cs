using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.IO;
using Newtonsoft.Json;
using ANDISBANCKAPI;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    [HttpPost("loan/request/{userId}")]
    public async Task<IActionResult> Post([FromRoute] int userId, [FromBody] LoanRequest loanRequest)
    {
        // if (resp)
        Console.WriteLine(loanRequest);
        return StatusCode(StatusCodes.Status201Created);
        // else
            // return StatusCode(StatusCodes.Status500InternalServerError);
    }
}



