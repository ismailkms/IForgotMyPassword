using IForgotMyPassword.Abstraction;
using IForgotMyPassword.Entities;

namespace IForgotMyPassword.Concrete
{
    public class UserService : IUserService
    {
        
        public List<User> GetAllUsers()
        {
            return new()
            {
                new() { Id = 1, UserName = "user1", Password = "123", Email = "user1@gmail.com", Role = "Admin" },
                new() { Id = 2, UserName = "user2", Password = "123", Email = "user2@gmail.com", Role = "Member" },
                new() { Id = 3, UserName = "user3", Password = "123", Email = "user3@gmail.com", Role = "Admin" },
                new() { Id = 4, UserName = "user4", Password = "123", Email = "user4@gmail.com", Role = null }
            };
        }

        public User GetUserById(int id)
        {
            User? user = GetAllUsers().Find(u => u.Id == id);
            return user;
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            User? user = GetAllUsers().Find(u => u.UserName == username && u.Password == password);
            return user;
        }

        public User GetUserByUsernameOrEmail(string userNameOrEmail)
        {
            User? user = GetAllUsers().Find(u => u.UserName == userNameOrEmail);
            if (user == null) 
                user = GetAllUsers().Find(u => u.Email == userNameOrEmail);
            return user;
        }
    }
}
