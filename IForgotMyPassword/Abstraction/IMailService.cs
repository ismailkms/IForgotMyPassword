namespace IForgotMyPassword.Abstraction
{
    public interface IMailService
    {
        Task SendMailAsync(string to, string subject, string body);
    }
}
