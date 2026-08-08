using Wedding.DTOs;

namespace Wedding.Services;

public interface IContext
{
    string Token { get; }
    string Language { get; }
    PartyDto? Party { get; }
    string GetMessage(string key);

    Task Init(string token, string language, bool markSeen);
}

public class Context(IConfiguration configuration, IGuestService guestService) : IContext
{
    public string Token => token;
    public string Language => language;
    public PartyDto? Party => party;

    private string token = string.Empty;
    private string language = "en";
    private PartyDto? party = null;

    private static readonly string[] Languages = ["en", "ro", "jp"];

    public string GetMessage(string key) => configuration
        .GetSection("Locale")
        .GetSection(language)
        .GetValue<string>(key) ?? key;
    
    public async Task Init(string token, string language, bool markSeen)
    {
        if (!Languages.Contains(language))
            throw new Exception("Invalid language provided");

        this.token = token;
        this.language = language;
        this.party = await guestService.GetPartyByToken(token, markSeen);
    }
}