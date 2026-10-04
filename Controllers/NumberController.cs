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
   

   [HttpPut("{id}/{number}")]
public async Task<IActionResult> UpdateNumber([FromRoute] int id, [FromRoute] int number)
{
    try
    {
        int nuevoResultado = number + number;
        var record = new NumberRecordSet(number, nuevoResultado);

        var actualizado = await _numberService.UpdateAsync(id, record);

        if (actualizado == null)
        {
            return NotFound(new { message = $"No se encontró el registro con ID {id}." });
        }

        return Ok(actualizado);
     
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = "Error interno.", detalle = ex.Message });
    }
}

    [HttpGet("db/get")]
    public async Task<IActionResult> GetList()
    {
        try
        {
            IEnumerable<NumberRecordGet> numbers = await _numberService.GetListAsync();
            return Ok(numbers);
        }
      catch (Exception ex)
    {
        return StatusCode(500, new { message = "Error al obtener la lista.", detalle = ex.Message });
    }
    }

    [HttpGet("db/get/{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var number = await _numberService.GetByIdAsync(id);
            if (number == null)
            {
                return NotFound(new { message = $"No se encontró el registro con ID {id}." });
            }
            return Ok(number);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener el registro.", detalle = ex.Message });
        }
    }
}
