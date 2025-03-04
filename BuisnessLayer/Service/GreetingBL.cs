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
    }
}