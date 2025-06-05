using CW12_s30331.DTOs.TripDTOs;
using CW12_s30331.Exceptions;
using CW12_s30331.Models;
using CW12_s30331.Services;
using Microsoft.AspNetCore.Mvc;

namespace CW12_s30331.Controllers;


[Route("api/[controller]")]
[ApiController]
public class TripsController(IDbService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0)
            page = 1;
        if (pageSize <= 0)
            pageSize = 10;
        
        var trips = await service.GetTripsAsync(page, pageSize);
        return Ok(trips);
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AddClientToTrip([FromRoute] int idTrip, [FromBody] TripClientCreateDto clientDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await service.AddClientToTripAsync(idTrip, clientDto);

            return StatusCode(201, "Added client to trip successfully!");
        }
        catch (TripNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (TripAlreadyStartedException e)
        {
            return Conflict(e.Message);
        }
        catch (ClientAlreadyExistsException e)
        {
            return Conflict(e.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}