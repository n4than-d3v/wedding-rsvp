using Microsoft.EntityFrameworkCore;
using Wedding.Commands;
using Wedding.Database;
using Wedding.Database.Models;
using Wedding.DTOs;

namespace Wedding.Services;

public interface IGuestService
{
    Task<PartyDto?> GetPartyByToken(string token);
    Task<PartyDto> Rsvp(RsvpCommand command);
}

public interface IAdminService
{
    Task<IReadOnlyList<PartyDto>> GetParties();
}

public class GuestService(WeddingContext database) : IGuestService, IAdminService
{
    public async Task<IReadOnlyList<PartyDto>> GetParties()
    {
        var parties = await database.Parties
            .Include(p => p.Guests)
            .ToListAsync();

        return [.. parties.Select(Map)];
    }

    public async Task<PartyDto?> GetPartyByToken(string token)
    {
        var party = await database.Parties
            .Include(p => p.Guests)
            .FirstOrDefaultAsync(p => p.Token == token);
        
        if (party == null) return null;

        party.Seen = DateTime.UtcNow;
        await database.SaveChangesAsync();

        return Map(party);
    }

    public async Task<PartyDto> Rsvp(RsvpCommand command)
    {
        var party = await database.Parties
            .Include(p => p.Guests)
            .FirstOrDefaultAsync(p => p.Token == command.Token)
            ?? throw new Exception("Party not found");

        party.Responded = DateTime.UtcNow;
        party.Email = command.Email;
        party.Diet = command.Diet;
        party.PlusOneName = command.PlusOneName;

        foreach (var guest in party.Guests)
        {
            guest.Coming = command.GuestsComing.Contains(guest.Id);
        }

        await database.SaveChangesAsync();

        return Map(party);
    }
 
    private static PartyDto Map(Party party) => new()
    {
        Id = party.Id,
        Token = party.Token,
        Name = party.Name,
        Seen = party.Seen,
        Responded = party.Responded,
        Email = party.Email,
        Diet = party.Diet,
        PlusOneName = party.PlusOneName,
        Arrival = party.Arrival,
        Housed = party.Housed,
        Guests = [.. party.Guests.Select(g => new GuestDto
        {
            Id = g.Id,
            Name = g.Name,
            Coming = g.Coming
        })]
    };
}