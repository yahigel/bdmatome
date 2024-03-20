using BlazorApp.Components;
using Microsoft.EntityFrameworkCore;
using bdaAPI.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<HttpClient>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MVC controllers services
builder.Services.AddControllers(); // ‚±‚Ìs‚ð’Ç‰Á

// OpenID Connect
builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes);

// LINE login configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "Line";
})
.AddCookie() // Cookie-based authentication
.AddLine(options =>
{
    options.ClientId = "2003996418";
    options.ClientSecret = "af1ca219bda830894a12795187237083";
    // Additional options as needed
});

var connectionString = builder.Configuration.GetConnectionString("MyDatabase");
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseCors("AllowSpecificOrigin"); // Apply CORS policy

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseAuthentication();
app.UseAuthorization();
app.UseIdentityServer();

app.MapControllers(); // Make sure this line is already there, which is necessary for routing to controllers

app.Run();
