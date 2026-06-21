using Ordbok.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpClient<OrdbokApiClient>(client =>
{
    client.BaseAddress = new Uri("https://api.ordbokapi.org/graphql");
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

// Autocomplete suggestions endpoint
app.MapGet("/api/suggest", async (
    string word,
    string[]? dictionaries,
    OrdbokApiClient client) =>
{
    if (string.IsNullOrWhiteSpace(word))
        return Results.Ok(Array.Empty<string>());

    var dicts = (dictionaries is { Length: > 0 })
        ? dictionaries
        : ["Bokmaalsordboka", "Nynorskordboka", "NorskOrdbok"];

    var suggestions = await client.GetSuggestionsAsync(word, dicts);
    return Results.Ok(suggestions);
});

app.Run();
