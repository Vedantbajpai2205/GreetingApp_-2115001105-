using BuisnessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;
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
            logger.Info("This is Info Message.");
            logger.Error("This is Error Message.");
            logger.Debug("This is Debug Message.");
            logger.Warn("This is Warning Message.");
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

        [HttpPost("greetmessage")]

        public IActionResult GreetMessage(GreetingModel greetModel)
        {
            try
            {
                var response = new ResponseModel<string>();
                bool isMessageGrret = _greetingBL.GreetMessage(greetModel);
                if (isMessageGrret)
                {
                    response.Success = true;
                    response.Message = "Greet Message!";
                    response.Data = greetModel.ToString();
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "Greet Message Already Exist.";
                return Conflict(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error occurred during login.");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("GetGreetingById/{id}")]
        public IActionResult GetGreetingById(int id)
        {
            var response = new ResponseModel<GreetingModel>();
            try
            {
                var result = _greetingBL.GetGreetingById(id);
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Greeting Message Found";
                    response.Data = result;
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "Greeting Message Not Found";
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                return StatusCode(500, response);
            }
        }
        [HttpGet("GetAllGreetings")]
        public IActionResult GetAllGreetings()
        {
            ResponseModel<List<GreetingModel>> response = new ResponseModel<List<GreetingModel>>();
            try
            {
                var result = _greetingBL.GetAllGreetings();
                if (result != null && result.Count > 0)
                {
                    response.Success = true;
                    response.Message = "Greeting Messages Found";
                    response.Data = result;
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "No Greeting Messages Found";
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                return StatusCode(500, response);
            }
        }

    }
}





