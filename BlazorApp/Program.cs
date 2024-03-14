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

//OpenIDConnect
builder.Services.AddIdentityServer()                //IdentityServer4 ミドルウェアをサービスコレクションに追加。
    .AddDeveloperSigningCredential()                //開発時に使用される署名証明書をIdentityServerに追加。
                                                    //このメソッドはテストや開発段階で使用され、本番環境ではより安全な証明書に置き換える必要があります。
                                                    //署名証明書は、トークンの正当性を確認するのに使用されます。
    .AddInMemoryApiResources(Config.ApiResources)   //アプリケーションのHTTPリクエストパイプラインにIdentityServerミドルウェアを追加。
                                                    //これにより、IdentityServerが認証と認可を処理できるようになります。
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes);


// LINEログインの設定
builder.Services.AddAuthentication(options =>
{
    // 既存の認証方式を保持しつつ、デフォルトの認証方式として設定
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "Line";
})
.AddCookie() // Cookieベースの認証を追加
.AddLine(options =>
{
    options.ClientId = "2003996418";
    options.ClientSecret = "af1ca219bda830894a12795187237083";
    // 必要に応じて他のオプションを設定
});

// Add Swagger and EF Core services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
app.UseCors("AllowSpecificOrigin"); // CORSポリシーを適用

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseAuthentication();
app.UseAuthorization();
app.UseIdentityServer();

app.MapControllers();

app.Run();
