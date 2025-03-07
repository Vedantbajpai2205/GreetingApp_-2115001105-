using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        public bool Register(UserEntity user);
        string Login(string email, string password);
        public bool ForgetPassword(string email);

        public bool ResetPassword(string email, string newPassword);
    }
}
