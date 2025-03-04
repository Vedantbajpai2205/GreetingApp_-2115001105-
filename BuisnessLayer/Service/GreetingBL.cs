using BuisnessLayer.Interface;
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
        public bool GreetMessage(GreetingModel greetModel)
        {
            return _greetingRL.GreetMessage(greetModel);
        }
        public GreetingModel GetGreetingById(int id)
        {
            return _greetingRL.GetGreetingById(id);
        }
        public List<GreetingModel> GetAllGreetings()
        {
            var entityList = _greetingRL.GetAllGreetings();  
            if (entityList != null)
            {
                return entityList.Select(g => new GreetingModel
                {
                    id = g.Id,
                    GreetingMessage = g.Greeting
                }).ToList();  // Converting List of Entity to List of Model
            }
            return null;
        }
    }
}