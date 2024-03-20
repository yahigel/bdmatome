using BlazorApp.Components;
using Microsoft.EntityFrameworkCore;
using bdaAPI.Common;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<HttpClient>();

// MVC controllers services
builder.Services.AddControllers();

// OpenID Connect and JWT Token configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "Line";
})
.AddJwtBearer(options =>
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

// Database configuration
var connectionString = builder.Configuration.GetConnectionString("MyDatabase");
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(connectionString));

// Identity configuration
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    // ëºÇÃ Identity ÇÃê›íË
})
.AddEntityFrameworkStores<MyDbContext>()
.AddDefaultTokenProviders();

// IdentityServer configuration with in-memory stores, persisted grants, and ASP.NET Identity integration
builder.Services.AddIdentityServer(options =>
{
    // IdentityServer ÇÃîCà”ÇÃê›íË
})
.AddDeveloperSigningCredential() // ñ{î‘ä¬ã´Ç≈ÇÕìKêÿÇ»èÿñæèëÇ…íuÇ´ä∑Ç¶ÇÈÇ±Ç∆
.AddInMemoryPersistedGrants()
.AddInMemoryApiResources(Config.ApiResources)
.AddInMemoryClients(Config.Clients)
.AddInMemoryApiScopes(Config.ApiScopes)
.AddAspNetIdentity<IdentityUser>();

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

app.UseRouting();

app.UseAuthentication(); // Ensure these middleware are in the correct order
app.UseAuthorization();
app.UseIdentityServer(); // UseIdentityServer adds the token endpoint and other features of IdentityServer

app.MapControllers(); // Routing to controllers

app.Run();
