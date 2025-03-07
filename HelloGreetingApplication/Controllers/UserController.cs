using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;
using RepositoryLayer.Entity;

namespace HelloGreetingApplication.Controllers
{
    /// <summary>
    /// User Controller to handle User Authentication APIs
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IUserBL _userBL;

        public UserController(IUserBL userBL)
        {
            _userBL = userBL;
            _logger.Info("User Controller initialized successfully");
        }

        /// <summary>
        /// API to Register a New User
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public IActionResult Register(UserModel userModel)
        {
            var response = new ResponseModel<string>();
            try
            {
                UserEntity userEntity = new UserEntity()
                {
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    Email = userModel.Email,
                    Password = userModel.Password // Hashing will be done inside Business Layer
                };

                bool result = _userBL.Register(userEntity); // Now passing UserEntity instead of UserModel
                if (result)
                {
                    response.Success = true;
                    response.Message = "User Registered Successfully";
                    response.Data = userEntity.Email;
                    _logger.Info($"User Registered: {userModel.Email}");
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "User Already Exists";
                return Conflict(response);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in Register API: {ex.Message}");
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// API for User Login
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public IActionResult Login(UserLogin loginModel)
        {
            var response = new ResponseModel<string>();
            try
            {
                var result = _userBL.Login(loginModel.Email, loginModel.Password); // Pass Email and Password Separately
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Login Successful";
                    response.Data = result;
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "Invalid Credentials";
                return Unauthorized(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                return StatusCode(500, response);
            }
        }


        /// <summary>
        /// API to Handle Forget Password
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("ForgetPassword")]
        public IActionResult ForgetPassword(string email)
        {
            var response = new ResponseModel<string>();
            try
            {
                bool result = _userBL.ForgetPassword(email);
                if (result)
                {
                    response.Success = true;
                    response.Message = "Reset Link Sent Successfully";
                    _logger.Info($"Reset Password Link Sent to: {email}");
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "Email Not Found";
                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in ForgetPassword API: {ex.Message}");
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// API to Reset Password
        /// </summary>
        /// <param name="resetModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            var result = _userBL.ResetPassword(resetPasswordModel.Email, resetPasswordModel.NewPassword);
            ResponseModel<string> responseModel = new ResponseModel<string>();
            if (result == true)
            {
                responseModel.Success = true;
                responseModel.Message = "Password Reset Successfull.";
                responseModel.Data = "Your Password has been reset successfully";
                return Ok(responseModel);
            }
            responseModel.Success = false;
            responseModel.Message = "Password Reset Failed or Email Not Found";
            return BadRequest(responseModel);
        }


    }
}