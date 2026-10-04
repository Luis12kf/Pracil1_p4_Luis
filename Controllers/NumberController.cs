using Microsoft.AspNetCore.Mvc;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Data.Sqlite;
using Parcil1_P4Luis.Services;
using Parcil1_P4Luis.Models;
using SQLitePCL;

namespace Parcil1_P4Luis.Controllers;
[ApiController]
[Route("api/[controller]")]
public class NumberController : ControllerBase
{
    private readonly NumberService _numberService;

    public NumberController(NumberService numberService)
    {
        _numberService = numberService;
    }

    [HttpGet("{number}")]
    public async Task<IActionResult> Index([FromRoute] int number)
{
    int resultado = number + number;
    var record = new NumberRecordSet(number, resultado);

    try
    {
        dynamic service = _numberService;
        bool success = await service.SaveAsync(record);
        if (!success)
        {
            return StatusCode(500, new { message = "Error al guardar en la base de datos." });
        }

        return Ok(new { numero = number, resultado = resultado });
    }
    catch (Exception ex)
    {
        // Captura cualquier falla real (conexión BD, tabla inexistente, error de Dapper, etc.)
        return StatusCode(500, new { message = "Error interno del servidor.", detalle = ex.Message });
    }
}
   

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNumber(int id, [FromBody] NumberRecordSet number)
    {
        try
        {
            dynamic service = _numberService;
            var updatedNumber = await service.UpdateAsync(id, number);
            if (updatedNumber != null)
            {
                return Ok(updatedNumber);
            }
            return NotFound(new { message = "Número no encontrado." });
        }
        catch (RuntimeBinderException)
        {
            return StatusCode(500, new { message = "La operación de actualización no está disponible." });
        }
    }

    [HttpGet]
    [HttpGet("db/get")]
    public async Task<IActionResult> GetList()
    {
        try
        {
            dynamic service = _numberService;
            var numbers = await service.GetListAsync();
            return Ok(numbers);
        }
        catch (RuntimeBinderException)
        {
            return Ok(Array.Empty<NumberRecordSet>());
        }
    }

}
