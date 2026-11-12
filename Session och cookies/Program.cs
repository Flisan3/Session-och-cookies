
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(); // Krävs för att Session ska kunna lagra data i serverns minne builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>

{

    // Session rensas efter 20 minuters inaktivitet options.IdleTimeout = TimeSpan.FromMinutes(20);

});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

// Aktiverar Session app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

/*
1. När Session går ut försvinner varukorgen eftersom informationen
lagras tillfälligt på servern och inte sparas permanent.

2. Session passar bra för en varukorg eftersom informationen bara
behöver sparas tillfälligt medan användaren använder webbplatsen.

3. Nej, Session är inte en bra plats för att lagra alla produkter.
En databas passar bättre eftersom produkterna behöver sparas
permanent och kunna användas av flera användare.
*/