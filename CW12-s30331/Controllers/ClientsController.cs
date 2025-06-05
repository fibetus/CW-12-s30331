using CW12_s30331.Exceptions;
using CW12_s30331.Services;
using Microsoft.AspNetCore.Mvc;

namespace CW12_s30331.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController(IDbService service) : ControllerBase
{
    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        try
        {
            await service.DeleteClientAsync(idClient);
            return StatusCode(204, $"Client {idClient} deleted");
        }
        catch (ClientNotFoundExcpetion e)
        {
            return NotFound(e.Message);
        }
        catch (ClientWithTripsException e)
        {
            return Conflict(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}