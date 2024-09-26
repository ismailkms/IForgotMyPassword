namespace IForgotMyPassword.Abstraction
{
    public interface ILoginService
    {
        bool ControlUsernameAndPassword(string username, string password);
        Task AddUserToClaimAsync(string userName, string password);
        Task SendCodeToEmailAsync(string email);
        string GetCodeByEmail(string email);
    }
}
