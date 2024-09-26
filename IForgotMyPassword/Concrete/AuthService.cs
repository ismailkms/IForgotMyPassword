using IForgotMyPassword.Abstraction;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IForgotMyPassword.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task AddUserToClaimAsync(string userName, string role)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Role, role ?? "")
                };
            var useridentity = new ClaimsIdentity(claims, "Login");
            ClaimsPrincipal principal = new ClaimsPrincipal(useridentity);
            await _httpContextAccessor.HttpContext.SignInAsync(principal);

        }
    }
}
