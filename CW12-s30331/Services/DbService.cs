using CW12_s30331.Data;
using CW12_s30331.DTOs.TripDTOs;
using CW12_s30331.Exceptions;
using CW12_s30331.Models;
using Microsoft.EntityFrameworkCore;

namespace CW12_s30331.Services;


public interface IDbService
{
    Task<TripWithPagesGetDto> GetTripsAsync(int page, int pageSize);
    Task AddClientToTripAsync(int idTrip, TripClientCreateDto clientDto);
    Task DeleteClientAsync(int idClient);
}

public class DbService(MasterContext data) : IDbService
{
    public async Task<TripWithPagesGetDto> GetTripsAsync(int page, int pageSize)
    {
        var query = data.Trips
            .OrderByDescending(t => t.DateFrom)
            .Select(t => new TripGetDto
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => new TripCountryGetDto
                {
                    Name = c.Name,
                }).ToList(),
                Clients = t.ClientTrips.Select(ct => new TripClientGetDto
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName,
                }).ToList()
            });
            

        var tripsCount = await query.CountAsync();
        var pages = (int) Math.Ceiling(tripsCount / (double)pageSize);
        
        var trips = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new TripWithPagesGetDto
        {
            PageNum = page,
            PageSize = pageSize,
            AllPages = pages,
            Trips = trips,
        };
    }

    public async Task AddClientToTripAsync(int idTrip, TripClientCreateDto clientDto)
    {
        var trip = await data.Trips.FindAsync(idTrip);
        if (trip == null)
        {
            throw new TripNotFoundException($"Trip with id {idTrip} was not found!");
        }

        if (trip.DateFrom <= DateTime.Now)
        {
            throw new TripAlreadyStartedException($"Trip with id {idTrip} has already started!");
        }
        
        var clientWithPeselExists = await data.Clients.AnyAsync(c => c.Pesel == clientDto.Pesel);

        if (clientWithPeselExists)
        {
            throw new ClientAlreadyExistsException($"Client with PESEL {clientDto.Pesel} already exists!");
        }
        
        // Według polecenia powinienem jeszcze sprawdzić czy PESEL jest już zapisany na wycieczkę, ale nie może być zapisany
        // jeśli on nie istnieje. Jeśli istnieje, to poprzednie exception go zcatchuje, więc nigdy nie dojdzie do tego momentu

        var newClient = new Client
        {
            FirstName = clientDto.FirstName,
            LastName = clientDto.LastName,
            Email = clientDto.Email,
            Telephone = clientDto.Telephone,
            Pesel = clientDto.Pesel,
        };
        await data.Clients.AddAsync(newClient);

        var clientTrip = new ClientTrip
        {
            IdClientNavigation = newClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = clientDto.PaymentDate,
        };
        await data.Clients.AddAsync(newClient);
        await data.ClientTrips.AddAsync(clientTrip);
        await data.SaveChangesAsync();
    }

    public async Task DeleteClientAsync(int idClient)
    {
        var client = await data.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == idClient);

        if (client == null)
        {
            throw new ClientNotFoundExcpetion($"Client with id {idClient} was not found!");
        }

        if (client.ClientTrips.Any())
        {
            throw new ClientWithTripsException($"Client with id {idClient} has trips and cannot be deleted!");
        }
        
        data.Clients.Remove(client);
        await data.SaveChangesAsync();
    }
}