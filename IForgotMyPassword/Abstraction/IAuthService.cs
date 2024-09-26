using IForgotMyPassword.Entities;

namespace IForgotMyPassword.Abstraction
{
    public interface IAuthService
    {
        Task AddUserToClaimAsync(string userName, string role);
    }
}
