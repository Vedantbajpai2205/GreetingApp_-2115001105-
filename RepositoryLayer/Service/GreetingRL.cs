using RepositoryLayer.Interface;
using System;
using ModelLayer.Model;
using NLog;

namespace RepositoryLayer.Services
{
    public class GreetingRL : IGreetingRL
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public string Greeting(UserNameModel nameModel)
        {
            string greetingMessage = string.Empty;

            if (!string.IsNullOrEmpty(nameModel.FirstName) && !string.IsNullOrEmpty(nameModel.LastName))
            {
                greetingMessage = $"Hello Sir/Mam {nameModel.FirstName} {nameModel.LastName}";
            }
            else if (!string.IsNullOrEmpty(nameModel.FirstName))
            {
                greetingMessage = $"Hello Sir/Mam {nameModel.FirstName}";
            }
            else if (!string.IsNullOrEmpty(nameModel.LastName))
            {
                greetingMessage = $"Hello Mr/Mrs {nameModel.LastName}";
            }
            else
            {
                greetingMessage = "Hello World";
            }

            _logger.Info($"Generated Greeting: {greetingMessage}");
            return greetingMessage;
        }
    }
}