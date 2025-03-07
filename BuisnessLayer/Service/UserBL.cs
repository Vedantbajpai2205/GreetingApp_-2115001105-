using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRL;

        public UserBL(IUserRL userRL)
        {
            _userRL = userRL;
        }

        public bool Register(UserEntity user)
        {
            return _userRL.Register(user);
        }

        public string Login(string email, string password)
        {
            return _userRL.Login(email, password);
        }
        public bool ForgetPassword(string email) => _userRL.ForgetPassword(email);
        public bool ResetPassword(string email, string newPassword)
        {
            return _userRL.ResetPassword(email, newPassword);
        }
    }
}
