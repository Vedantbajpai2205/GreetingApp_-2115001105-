using System.Security.Cryptography;
using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRL;
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 10000;

        public UserBL(IUserRL userRL)
        {
            _userRL = userRL;
        }

        public bool Register(UserEntity user)
        {
            return _userRL.Register(user);
        }

        public bool LoginUser(UserLogin login)
        {
            var user = GetUserByEmail(login.Email);
            if (user == null)
            {
                return false;
            }
            return CheckEmailPassword(login.Email, login.Password, user);
        }


        public UserEntity GetUserByEmail(string email)
        {
            return _userRL.GetUserByEmail(email);
        }
        public bool CheckEmailPassword(string email, string password, UserEntity result)
        {
            if (result == null)
            {
                return false;
            }
            return result.Email == email && VerifyPassword(password, result.Password);
        }
        public bool ForgetPassword(string email) => _userRL.ForgetPassword(email);
        public bool ResetPassword(string email, string newPassword)
        {
            return _userRL.ResetPassword(email, newPassword);
        }
        public bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(storedPassword);
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);
            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, Iterations))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);
                bool result = ComparePassword(hash, hashBytes);
                return result;
            }
        }
        public bool ComparePassword(byte[] hash, byte[] hashbytes)
        {
            for (int i = 0; i < HashSize; i++)
            {
                if (hashbytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
