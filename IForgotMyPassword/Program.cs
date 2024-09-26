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
//LoginPath => kullan�c� kimli�i do�rulanmad���nda y�nlendirilece�i adrestir.
//LogoutPath => kullan�c� ��k�� yapt���nda y�nlendirilece�i adrestir.
//AccessDeniedPath => yetkilendirme ba�ar�s�z oldu�unda y�nlendirilece�i adrestir.
//ExpireTimeSpan => authentication ticket bilgisinin cookie �zerinde hangi zamana kadar saklanaca��n� belirler ve varsay�lan olarak 14 g�nd�r.
//SlidingExpiration => belirtilen s�renin yar�s�na gelindi�inde s�renin yenilenip yenilenmeyece�ini belirler.

//Cookie
//Name => cookie ismidir, varsay�lan olarak AspNetCore.Cookies de�erini kullan�r.
//HttpOnly => cookie bilgisinin client-side script taraf�ndan eri�ilebilirli�ini ayarlar.
//SameSite => cookie bilgisinin farkl� siteler taraf�ndan eri�ilebilirli�ini ayarlar. Strict ile cookie farkl� site isteklerinde ta��nmaz. Lax ile bu bilgi ta��n�r ve None ile bir k�s�tlamaya tabi olmaz.
//SecurePolicy => cookie bilgisinin secure bilgisi ayarlan�r.

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
