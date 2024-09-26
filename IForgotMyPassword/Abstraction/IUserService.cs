using IForgotMyPassword.Entities;

namespace IForgotMyPassword.Abstraction
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        User GetUserById(int id);
        User GetUserByUsernameAndPassword(string username, string password);
        User GetUserByUsernameOrEmail(string userNameOrEmail);
    }
}
