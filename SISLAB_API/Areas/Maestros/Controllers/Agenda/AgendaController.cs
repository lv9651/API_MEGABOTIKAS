using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SISLAB_API.Areas.Maestros.Models;
using SISLAB_API.Areas.Maestros.Services;
using System.Web;
using MySql.Data.MySqlClient;




namespace SISLAB_API.Areas.Maestros.Controllers // Cambia esto al espacio de nombres real
{

    [Route("api/[controller]")]
[ApiController]
public class AgendaController : ControllerBase
{
    private readonly AgendaService _agendaService;

    public AgendaController(AgendaService agendaService)
    {
        _agendaService = agendaService;
    }

    [HttpPost]
    public IActionResult RegisterAgendaItem([FromBody] AgendaItem agendaItem)
    {
        if (agendaItem == null)
            return BadRequest("Invalid agenda item.");

        _agendaService.AddAgendaItem(agendaItem);
        return Ok("ok");
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteAgendaItem(int id)
    {
        _agendaService.DeleteAgendaItem(id);
        return Ok("Agenda item deleted successfully.");
    }

    [HttpGet("date/{date}")]
    public IActionResult GetAgendaItems(DateTime date)
    {
        var items = _agendaService.GetAgendaItems(date);
        return Ok(items);
    }
}

}