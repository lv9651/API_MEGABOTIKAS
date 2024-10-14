using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DataTransferController : ControllerBase
{
    private readonly DataTransferService _dataTransferService;

    public DataTransferController(DataTransferService dataTransferService)
    {
        _dataTransferService = dataTransferService;
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> TransferData()
    {
        try
        {
            await _dataTransferService.TransferDataAsync();
            return Ok("Datos transferidos exitosamente.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        }
    }
}