using BuisnessLayer.Interface;
using BusinessLayer.Interface;
using ModelLayer.Model;
using NLog;
using RepositoryLayer.Interface;
using RepositoryLayer.Services;
using System;

namespace BusinessLayer.Services
{
    public class GreetingBL : IGreetingBL
    {
        private readonly IGreetingRL _greetingRL;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public GreetingBL(IGreetingRL greetingRL)
        {
            _greetingRL = greetingRL;
        }

        public string GetGreet()
        {
            return "Hello World";

        }
        public string greeting(UserNameModel userName) 
        {
            return _greetingRL.Greeting(userName);
        }
        public bool GreetMessage(RequestGreetingModel greetModel)
        {
            return _greetingRL.GreetMessage(greetModel);
        }
        public RequestGreetingModel GetGreetingById(int id, string email)
        {
            return _greetingRL.GetGreetingById(id, email);
        }
        public List<RequestGreetingModel> GetAllGreetings(string email)
        {
            var entityList = _greetingRL.GetAllGreetings(email);
            if (entityList != null)
            {
                return entityList.Select(g => new RequestGreetingModel
                {
                    UserId = g.UserId,
                    GreetingMessage = g.Greeting,
                    Email = g.User?.Email, // Ensure User data is included
                    //UserName = g.User != null ? $"{g.User.FirstName} {g.User.LastName}" : null
                }).ToList();
            }
            return null;
        }
        public GreetingModel EditGreeting(int id, GreetingModel greetingModel, string email)
        {
            var result = _greetingRL.EditGreeting(id, greetingModel, email);
            if (result != null)
            {
                return new GreetingModel()
                {
                    Id = result.Id,
                    GreetingMessage = result.Greeting
                };
            }
            return null;
        }
        public bool DeleteGreeting(int id, string email)
        {
            return _greetingRL.DeleteGreeting(id, email);
        }
    }
}