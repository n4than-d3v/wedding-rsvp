namespace Wedding.DTOs;

public class PartyDto
{
    public int Id { get; set; }

    public string Token { get; set; }

    public string Name { get; set; }
    
    public DateTime? Seen { get; set; }

    public DateTime? Responded { get; set; }

    public string? Email { get; set; }

    public string? Diet { get; set; }

    public List<GuestDto> Guests { get; set; }
}