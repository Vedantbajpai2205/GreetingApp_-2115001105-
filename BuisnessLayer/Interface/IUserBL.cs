using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        public bool Register(UserEntity user);
        public bool LoginUser(UserLogin login);
        public UserEntity GetUserByEmail(string email);
        public bool CheckEmailPassword(string email, string password, UserEntity result);
        
        public bool ForgetPassword(string email);

        public bool ResetPassword(string email, string newPassword);
    }
}
