using RepositoryLayer.Interface;
using RepositoryLayer.Context;
using System;
using ModelLayer.Model;
using NLog;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Services
{
    public class GreetingRL : IGreetingRL
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly GreetingContext _context;

        public GreetingRL(GreetingContext context)
        {
            _context = context;
        }
        public bool GreetMessage(GreetingModel greetModel)
        {
            if (_context.GreetMessages.Any(greet => greet.Greeting == greetModel.GreetingMessage))
            {
                return false;
            }
            var greetingEntity = new GreetingEntity
            {
                Greeting = greetModel.GreetingMessage,
            };
            _context.GreetMessages.Add(greetingEntity);
            _context.SaveChanges();
            return true;
        }
        public GreetingModel GetGreetingById(int ID)
        {
            var entity = _context.GreetMessages.FirstOrDefault(g => g.Id == ID);

            if (entity != null)
            {
                return new GreetingModel()
                {
                    id = entity.Id,
                    GreetingMessage = entity.Greeting
                };
            }
            return null;
        }

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

            logger.Info($"Generated Greeting: {greetingMessage}");
            return greetingMessage;
        }
    }
}