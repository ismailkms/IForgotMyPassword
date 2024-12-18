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
//LoginPath => kullanıcı kimliği doğrulanmadığında yönlendirileceği adrestir.
//LogoutPath => kullanıcı çıkış yaptığında yönlendirileceği adrestir.
//AccessDeniedPath => yetkilendirme başarısız olduğunda yönlendirileceği adrestir.
//ExpireTimeSpan => authentication ticket bilgisinin cookie üzerinde hangi zamana kadar saklanacağını belirler ve varsayılan olarak 14 gündür.
//SlidingExpiration => belirtilen sürenin yarısına gelindiğinde sürenin yenilenip yenilenmeyeceğini belirler.

//Cookie
//Name => cookie ismidir, varsayılan olarak AspNetCore.Cookies değerini kullanır.
//HttpOnly => cookie bilgisinin client-side script tarafından erişilebilirliğini ayarlar.
//SameSite => cookie bilgisinin farklı siteler tarafından erişilebilirliğini ayarlar. Strict ile cookie farklı site isteklerinde taşınmaz. Lax ile bu bilgi taşınır ve None ile bir kısıtlamaya tabi olmaz.
//SecurePolicy => cookie bilgisinin secure bilgisi ayarlanır.

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
