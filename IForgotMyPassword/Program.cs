using IForgotMyPassword.Abstraction;
using IForgotMyPassword.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IMailService, MailService>();

//Cache
builder.Services.AddMemoryCache();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/Login/Index";
                        options.AccessDeniedPath = "/Login/Index";
                        options.SlidingExpiration = true;
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    });
//LoginPath => kullanýcý kimliði doðrulanmadýðýnda yönlendirileceði adrestir.
//LogoutPath => kullanýcý çýkýþ yaptýðýnda yönlendirileceði adrestir.
//AccessDeniedPath => yetkilendirme baþarýsýz olduðunda yönlendirileceði adrestir.
//ExpireTimeSpan => authentication ticket bilgisinin cookie üzerinde hangi zamana kadar saklanacaðýný belirler ve varsayýlan olarak 14 gündür.
//SlidingExpiration => belirtilen sürenin yarýsýna gelindiðinde sürenin yenilenip yenilenmeyeceðini belirler.

//Cookie
//Name => cookie ismidir, varsayýlan olarak AspNetCore.Cookies deðerini kullanýr.
//HttpOnly => cookie bilgisinin client-side script tarafýndan eriþilebilirliðini ayarlar.
//SameSite => cookie bilgisinin farklý siteler tarafýndan eriþilebilirliðini ayarlar. Strict ile cookie farklý site isteklerinde taþýnmaz. Lax ile bu bilgi taþýnýr ve None ile bir kýsýtlamaya tabi olmaz.
//SecurePolicy => cookie bilgisinin secure bilgisi ayarlanýr.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
