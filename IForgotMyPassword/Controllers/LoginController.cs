using IForgotMyPassword.Abstraction;
using IForgotMyPassword.Concrete;
using IForgotMyPassword.Entities;
using IForgotMyPassword.Models;
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

        public IActionResult Index(string returnUrl)
        {
            //AccessDenied yani yetkisiz erişim yapıldığında erişmeye çalıştığın sayfanın url'i tutulur.(https://localhost:7185/Login/Index?ReturnUrl=%2FHome%2FMemberPage şeklinde görebilirsin.) Burada kullanıcı giriş yaptığında o url'e yönlensin diye o url'i çekiyoruz.
            return View(new UserLoginModel() { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserLoginModel userLoginModel)
        {
            if (ModelState.IsValid)
            {
                bool result = _loginService.ControlUsernameAndPassword(userLoginModel.Username, userLoginModel.Password);
                if (result)
                {
                    await _loginService.AddUserToClaimAsync(userLoginModel.Username, userLoginModel.Password);
                    if (userLoginModel.ReturnUrl != null)
                    {
                        return Redirect(userLoginModel.ReturnUrl);
                        //Eğer ReturnUrl doluysa ona yönlendir yok değilse normal bir şekilde devam et diyoruz.
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Kullanıcı adı ya da şifre yanlış girildi");
            }
            return View();
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
                if (cacheCode == code)
                    return Json($"Şifre güncelleme ekranına yönlendirildi ve şifre güncelleştirildi");
            }

            return Json(null);
        }
    }
}
