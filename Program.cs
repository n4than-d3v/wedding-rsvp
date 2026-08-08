using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Wedding.Components;
using Wedding.Database;
using Wedding.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents();

builder.Services.AddDbContext<WeddingContext>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetConnectionString("PostgreSQL")));

builder.Services.AddScoped<IGuestService, GuestService>();
builder.Services.AddScoped<IAdminService, GuestService>();
builder.Services.AddScoped<IContext, Context>();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();

app.UseForwardedHeaders();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WeddingContext>();
    context.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>();
app.MapGet("/api/{token}", async (IGuestService guestService, string token) =>
{
    var party = await guestService.GetPartyByToken(token, false);
    if (party == null)
    {
        return Results.Ok(new
        {
            status = "N/A",
            invited = 0,
            coming = 0
        });
    }

    return Results.Ok(new
    {
        status = party.Responded == null ? (party.Seen == null ? "Not seen" : "Seen") : party.AnyComing ? "Coming" : "Not coming",
        invited = party.Guests.Count,
        coming = party.Guests.Count(g => g.Coming ?? false)
    });
});

app.Run();
