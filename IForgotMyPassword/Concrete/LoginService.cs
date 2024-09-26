using IForgotMyPassword.Abstraction;

namespace IForgotMyPassword.Concrete
{
    public class LoginService : ILoginService
    {
        readonly IUserService _userService;
        readonly IAuthService _authService;
        readonly ICacheService _cacheService;
        readonly IMailService _mailService;

        public LoginService(IUserService userService, IAuthService authService, ICacheService cacheService, IMailService mailService)
        {
            _userService = userService;
            _authService = authService;
            _cacheService = cacheService;
            _mailService = mailService;
        }

        public async Task AddUserToClaimAsync(string userName, string password)
        {
            var user = _userService.GetUserByUsernameAndPassword(userName, password);
            await _authService.AddUserToClaimAsync(user.UserName, user.Role);
        }

        public bool ControlUsernameAndPassword(string username, string password)
        {
            var user = _userService.GetUserByUsernameAndPassword(username, password);
            if (user != null)
                return true;
            return false;
        }

        public async Task SendCodeToEmailAsync(string email)
        {
            Random random = new();
            string code = random.Next(100000, 999999).ToString();
            _cacheService.SetCache(email, code);

            string mailBody = $"Merhaba,<br><br>" +
                $"Şifre güncellemek için talepte bulunmuş olduğunuz kod aşağıdadır.<br><br>" +
                $"<strong>{code}</strong><br><br>" +
                $"İyi günler.";

            await _mailService.SendMailAsync(email, "Onay Kodu", mailBody);
        }

        public string GetCodeByEmail(string email)
        {
            return _cacheService.GetCache(email);
        }
    }
}
