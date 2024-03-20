using BlazorApp.Components;
using Microsoft.EntityFrameworkCore;
using bdaAPI.Common;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<HttpClient>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MVC controllers services
builder.Services.AddControllers();

// OpenID Connect and JWT Token configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Bearer";
    options.DefaultChallengeScheme = "Line";
})
.AddJwtBearer("Bearer", options =>
{
    options.Authority = "https://localhost:5001"; // The URL of the authorization server
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false // Skip validating the audience
    };
})
.AddCookie() // Cookie-based authentication remains for other purposes
.AddLine(options => // Existing LINE login configuration remains unchanged
{
    options.ClientId = "2003996418";
    options.ClientSecret = "af1ca219bda830894a12795187237083";
    // Additional options as needed
});

var connectionString = builder.Configuration.GetConnectionString("MyDatabase");
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(connectionString));

// IdentityServer configuration with in-memory stores and persisted grants
builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryPersistedGrants() // Add this line to fix the error
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes);

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
app.UseCors("AllowSpecificOrigin"); // Apply CORS policy remains unchanged

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseAuthentication(); // Make sure these middleware are in the correct order
app.UseAuthorization();
app.UseIdentityServer(); // UseIdentityServer adds the token endpoint and other features of IdentityServer

app.MapControllers(); // Make sure this line is already there, which is necessary for routing to controllers

app.Run();
