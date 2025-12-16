using tictactoe_blazor.Components;
using tictactoe_blazor.Components.Services;
using tictactoe_blazor.Components.Hubs;

var builder = WebApplication.CreateBuilder(args);

// ƒобавл€ем сервис игры как Singleton
builder.Services.AddSingleton<GameService>();
builder.Services.AddSignalR();  // хаб

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.WebHost.UseUrls("http://0.0.0.0:8080");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapHub<GameHub>("/gamehub");  // регистрируем хаб

app.Run();
