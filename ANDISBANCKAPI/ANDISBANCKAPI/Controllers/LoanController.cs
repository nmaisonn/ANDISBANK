using Microsoft.AspNetCore.Mvc;
using ANDISBANCKAPI.Entidades;
using ANDISBANCKAPI;
using System.Text.Json;
using System.Collections.Generic;

namespace ANDIS_II.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class LoanController : ControllerBase
{
    private Loan[] _loans = new Loan[]
    {
        new Loan { id = 1,Descripcion = "Prestamo 1", Tipo = new LoanType{Name = "Prestamos de autos"},Monto = 1000, Tasa = 0.1, Plazo = DateTime.Now, Fecha = DateTime.Now, ClienteId = 1, Estado = 0 },
        new Loan { id = 2,Descripcion = "Prestamo 2", Tipo = new LoanType{ Name = "Prestamos de casas"},Monto = 2000, Tasa = 0.2, Plazo = DateTime.Now, Fecha = DateTime.Now, ClienteId = 2, Estado = 1 },
        new Loan { id = 3,Descripcion = "Prestamo 3", Tipo = new LoanType{ Name = "Prestamos bancarios"},Monto = 3000, Tasa = 0.3, Plazo = DateTime.Now, Fecha = DateTime.Now, ClienteId = 3, Estado = 2 },
        new Loan { id = 4,Descripcion = "Prestamo 4", Tipo = new LoanType{ Name = "Prestamos de autos" },Monto = 4000, Tasa = 0.4, Plazo = DateTime.Now, Fecha = DateTime.Now, ClienteId = 4, Estado = 3 },
    };
    public LoanController(ILogger<LoanController> logger)
    {
    }

    [HttpGet("loan/type")]
    public string GetLoanTypes()
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
            string error = "OcurriÃ³ un error al leer el archivo JSON: " + ex.Message;
            Console.WriteLine(error);
            return error;
        }
    }

    [HttpGet("api/v1/loan/paid/{userId}")]
    public Loan[] GetPaidLoans(int userId)
    {
        // Filter and return paid loans for the specified user
        Loan[] paidLoans = _loans.Where(x => x.ClienteId == userId && x.Estado == 1).ToArray();
        return paidLoans;
    }

    [HttpGet("api/v1/loan/active/{userId}")]
    public Loan[] GetActiveLoans(int userId)
    {
        Loan[] paidLoans = _loans.Where(x => x.ClienteId == userId && x.Estado == 0).ToArray();
        return paidLoans;
    }

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
                    List<Loan> resultList = loanList.Where(loan => loan.ClienteId == userId).ToList();
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


    [HttpPut("api/v1/loan/completed/{id}")]
    public IActionResult UpdateLoanStatus(int id)
    {
        Loan loanToUpdate = _loans.FirstOrDefault(x => x.id == id);
        if (loanToUpdate == null)
        {
            return NotFound();
        }

        //Actualizo el estado
        loanToUpdate.Estado = 1;

        //Persistencia 
        int i = Array.FindIndex(_loans, x => x.id == id);
        _loans[i] = loanToUpdate;

        return Ok("Estado del préstamo actualizado con éxito");
    }
}



