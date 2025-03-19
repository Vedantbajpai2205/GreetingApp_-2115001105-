using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using StackExchange.Redis;
using RepositoryLayer.Interface;
using NLog;
using System.Text.Json;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRL;
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 10000;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IDatabase _cache;

        public UserBL(IUserRL userRL, IConnectionMultiplexer redis)
        {
            _userRL = userRL;
            _cache = redis.GetDatabase();
        }
        public async Task<IEnumerable<UserEntity>> GetUsersAsync()
        {
            string cacheKey = "users_list";

            // Try getting data from Redis cache
            var cachedData = await _cache.StringGetAsync(cacheKey);
            if (!cachedData.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<IEnumerable<UserEntity>>(cachedData);
            }
            var products = await _userRL.GetUsersAsync();
            await _cache.StringSetAsync(cacheKey, JsonSerializer.Serialize(products), TimeSpan.FromMinutes(10));

            return products;
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
        public bool SendResetEmail(string email, string token)
        {
            try
            {
                var fromEmail = "your-email@example.com";
                var fromPassword = "your-email-password"; // Use environment variables for security
                var smtpClient = new SmtpClient("smtp.your-email-provider.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromEmail, fromPassword),
                    EnableSsl = true,
                };

                var resetUrl = $"https://your-frontend-app.com/reset-password?token={token}";
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = "Password Reset Request",
                    Body = $"Click <a href='{resetUrl}'>here</a> to reset your password.",
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(email);
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                
                _logger.Error($"Error in SendResetEmail: {ex.Message}");
                return false;
            }
        }
    }
}
