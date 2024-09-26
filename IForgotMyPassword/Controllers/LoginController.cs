using IForgotMyPassword.Abstraction;
using IForgotMyPassword.Concrete;
using IForgotMyPassword.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace IForgotMyPassword.Controllers
{
    public class LoginController : Controller
    {
        readonly ILoginService _loginService;
        readonly IUserService _userService;

        public LoginController(ILoginService loginService, IUserService userService)
        {
            _loginService = loginService;
            _userService = userService;
        }

        public IActionResult Index(bool control = true)
        {
            if (!control)
                ViewBag.Control = "Kullanıcı adı veya şifreniz yanlış.";
            return View();
        }

        public async Task<IActionResult> Login(string userName, string password)
        {
            bool result = _loginService.ControlUsernameAndPassword(userName, password);
            if (result)
            {
                await _loginService.AddUserToClaimAsync(userName, password);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Login", new { control = false });
        }

        public IActionResult EmailForm()
        {
            return View();
        }

        public async Task<JsonResult> SendCodeToEmail(string userNameOrEmail)
        {
            var user = _userService.GetUserByUsernameOrEmail(userNameOrEmail);

            if (user != null)
            {
                await _loginService.SendCodeToEmailAsync(user.Email);
                return Json(user.Id);
            }
            return Json(null);
        }

        public IActionResult CodePage(int id)
        {
            return View("CodePage", id);
        }

        public JsonResult CodeControl(int id, string code)
        {
            var user = _userService.GetUserById(id);
            if (user != null)
            {
                string cacheCode = _loginService.GetCodeByEmail(user.Email);
                if(cacheCode == code)
                    return Json($"Şifre güncelleme ekranına yönlendirildi ve şifre güncelleştirildi");
            }

            return Json(null);
        }
    }
}
