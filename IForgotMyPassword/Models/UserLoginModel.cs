using System.ComponentModel.DataAnnotations;

namespace IForgotMyPassword.Models
{
    public class UserLoginModel
    {
        //Burada DataAnnotation ile validation yazmak SRP'ye aykırıdır. Birden çok iş yaptırıyoruz.
        [Required(ErrorMessage ="Kullanıcı adı alanı boş olamaz")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş olamaz")]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
