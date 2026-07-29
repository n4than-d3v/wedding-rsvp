namespace Wedding.Commands;

public class RsvpCommand
{
    public required string Token { get; set; }

    public required string Email { get; set; }

    public required int[] GuestsComing { get; set; }

    public string? Diet { get; set; }
}
