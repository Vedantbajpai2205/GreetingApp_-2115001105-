using BuisnessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;
using Middleware.GlobalExceptionHandler;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace HelloGreetingApplication.Controllers
{
    /// <summary>
    /// Class providing API for HelloGreeting
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IGreetingBL _greetingBL;

        public HelloGreetingController(IGreetingBL greetingBL)
        {
            _greetingBL = greetingBL;
        }
        /// <summary>
        /// Get method to Greeting Message
        /// </summary>
        /// <returns>"Hello, World"</returns>
        [HttpGet]
        public IActionResult Get()
        {
            logger.Info("Received a GET request for greeting message.");
            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Message = "Hello to Greeting App API Endpoint Hit";
            responseModel.Success = true;
            responseModel.Data = "Hello,World";
            return Ok(responseModel);
        }

        /// <summary>
        /// post method to add new greeting message
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>request model</returns>
        [HttpPost]
        public IActionResult Post(RequestModel requestModel)
        {
            logger.Error("Product is null");
            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Message = "Request Received Successfully.";
            responseModel.Success = true;
            responseModel.Data = $"Key: {requestModel.key}, Value: {requestModel.value}";
            return Ok(responseModel);
        }
        /// <summary>
        /// Put method to update existing greeting message
        /// </summary>
        /// <param name="requestModel">Updated greeting message data</param>
        /// <returns>Updated greeting message</returns>
        [HttpPut]
        public IActionResult Put(RequestModel requestModel)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Message = "Greeting message updated successfully.";
            responseModel.Success = true;
            responseModel.Data = $"UpdatedKey: {requestModel.key}, UpdatedValue: {requestModel.value}";
            return Ok(responseModel);
        }
        /// <summary>
        /// Patch method for partial update of greeting message
        /// </summary>
        /// <param name="id">ID of the message to patch</param>
        /// <param name="requestModel">Partial greeting message data</param>
        /// <returns>Updated greeting message</returns>

        [HttpPatch]
        public IActionResult Patch(RequestModel requestModel)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Message = "Greeting message partially updated successfully.";
            responseModel.Success = true;
            responseModel.Data = $"PartiallyUpdatedKey: {requestModel.key}, PartiallyUpdatedValue: {requestModel.value}";
            return Ok(responseModel);
        }

        /// <summary>
        /// Delete method to remove a greeting message
        /// </summary>
        /// <param name="requestModel">ID of the greeting message to delete</param>
        /// <returns>request model</returns>
        [HttpDelete("{key}")]
        public IActionResult Delete(RequestModel requestModel)
        {
            logger.Warn("Product not found.");
            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Message = "Greeting message deleted successfully.";
            responseModel.Success = true;
            responseModel.Data = $"Key: {requestModel.key}, Deleted Successfully.";
            return Ok(responseModel);
        }

        [HttpGet]
        [Route("GetHelloWorld")]
        public string GetHello()
        {
            return _greetingBL.GetGreet();
        }


        [HttpPost]
        [Route("PostGreet")]
        public IActionResult PostGreeting(UserNameModel nameModel)
        {
            var result = _greetingBL.greeting(nameModel);
            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = "Greet Message With Name";
            responseModel.Data = result;
            return Ok(responseModel);
        }

        [HttpPost("AddGreetMessage")]
        public IActionResult GreetMessage([FromBody] RequestGreetingModel greetModel)
        {
            var response = new ResponseModel<string>();
            try
            {
                logger.Info("UserId in request: {greetModel.UserId}");
                bool isMessageGreet = _greetingBL.GreetMessage(greetModel);
                if (isMessageGreet)
                {
                    response.Success = true;
                    response.Message = "Greet Message Created Successfully!";
                    response.Data = Newtonsoft.Json.JsonConvert.SerializeObject(greetModel);
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "Greet Message Already Exists.";
                return Conflict(response);
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                var errorResponse = new
                {
                    success = false,
                    message = "An Error occurred",
                    error = innerExceptionMessage
                };

                return StatusCode(500, errorResponse);
            }
        }
        [HttpGet("GetGreetingById/{id}")]
        [Authorize]
        public IActionResult GetGreetingById(int id)
        {
            var emailFromToken = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(emailFromToken))
                return Unauthorized("Token iis Invalid or Email claim missing.");

            var greeting = _greetingBL.GetGreetingById(id, emailFromToken);

            if (greeting == null)
                return NotFound("Greeting Messages not found or Unauthorized Access Occur.");

            return Ok(greeting);
        }
        [HttpGet("GetAllGreetings")]
        [Authorize]
        public IActionResult GetAllGreetings()
        {
            var emailClaim = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(emailClaim))
            {
                return Unauthorized(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Unauthorized access. Email ID is missing or invalid.",
                    Data = null
                });
            }

            var greetings = _greetingBL.GetAllGreetings(emailClaim);

            if (greetings == null || !greetings.Any())
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "No greetings found for the provided email.",
                    Data = null
                });
            }

            return Ok(new ResponseModel<List<RequestGreetingModel>>
            {
                Success = true,
                Message = "Greetings retrieved successfully.",
                Data = greetings
            });
        }
        [HttpPut("EditGreeting/{id}")]
        [Authorize]
        public IActionResult EditGreeting(int id, GreetingModel greetModel)
        {
            var emailClaim = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(emailClaim))
            {
                return Unauthorized(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Unauthorized access. Email ID is missing or invalid.",
                    Data = null
                });
            }

            ResponseModel<GreetingModel> response = new ResponseModel<GreetingModel>();
            try
            {
                var result = _greetingBL.EditGreeting(id, greetModel, emailClaim);
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Greeting Message Updated Successfully";
                    response.Data = result;
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "Greeting Message Not Found";
                return NotFound(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ExceptionHandler.CreateErrorResponse(ex);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpDelete("DeleteGreeting/{id}")]
        [Authorize]
        public IActionResult DeleteGreeting(int id)
        {
            var emailClaim = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(emailClaim))
            {
                return Unauthorized(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Unauthorized access. Email ID is missing or invalid.",
                    Data = null
                });
            }

            ResponseModel<string> response = new ResponseModel<string>();
            try
            {
                bool result = _greetingBL.DeleteGreeting(id, emailClaim);
                if (result)
                {
                    response.Success = true;
                    response.Message = "Greeting Message Deleted Successfully";
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "Greeting Message Not Found";
                return NotFound(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ExceptionHandler.CreateErrorResponse(ex);
                return StatusCode(500, errorResponse);
            }
        }
    }
}





