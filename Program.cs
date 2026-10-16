using EventApp.Components;
using EventApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Razor component services with Interactive Server mode
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<ProductCatalog>();
builder.Services.AddScoped<CartService>(); // one cart per browser circuit, never shared

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();