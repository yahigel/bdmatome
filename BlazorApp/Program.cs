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
builder.Services.AddIdentityServer()                //IdentityServer4 �~�h���E�F�A���T�[�r�X�R���N�V�����ɒǉ��B
    .AddDeveloperSigningCredential()                //�J�����Ɏg�p����鏐���ؖ�����IdentityServer�ɒǉ��B
                                                    //���̃��\�b�h�̓e�X�g��J���i�K�Ŏg�p����A�{�Ԋ��ł͂����S�ȏؖ����ɒu��������K�v������܂��B
                                                    //�����ؖ����́A�g�[�N���̐��������m�F����̂Ɏg�p����܂��B
    .AddInMemoryApiResources(Config.ApiResources)   //�A�v���P�[�V������HTTP���N�G�X�g�p�C�v���C����IdentityServer�~�h���E�F�A��ǉ��B
                                                    //����ɂ��AIdentityServer���F�؂ƔF�������ł���悤�ɂȂ�܂��B
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes);


// LINE���O�C���̐ݒ�
builder.Services.AddAuthentication(options =>
{
    // �����̔F�ؕ�����ێ����A�f�t�H���g�̔F�ؕ����Ƃ��Đݒ�
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "Line";
})
.AddCookie() // Cookie�x�[�X�̔F�؂�ǉ�
.AddLine(options =>
{
    options.ClientId = "2003996418";
    options.ClientSecret = "af1ca219bda830894a12795187237083";
    // �K�v�ɉ����đ��̃I�v�V������ݒ�
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
app.UseCors("AllowSpecificOrigin"); // CORS�|���V�[��K�p

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseAuthentication();
app.UseAuthorization();
app.UseIdentityServer();

app.MapControllers();

app.Run();
