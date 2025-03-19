using RepositoryLayer.Interface;
using RepositoryLayer.Context;
using System;
using ModelLayer.Model;
using NLog;
using RepositoryLayer.Entity;
using Microsoft.EntityFrameworkCore;

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
        public bool GreetMessage(RequestGreetingModel greetModel)
        {
            if (string.IsNullOrWhiteSpace(greetModel.GreetingMessage))
            {
                throw new ArgumentException("Greeting message cannot be empty.");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == greetModel.Email);
            if (user == null)
            {
                throw new Exception($"User with Email {greetModel.Email} does not exist.");
            }

            if (_context.GreetMessages.Any(greet => greet.Greeting == greetModel.GreetingMessage
                                                     && greet.UserId == user.UserId))
            {
                return false;
            }

            var greetingEntity = new GreetingEntity
            {
                Greeting = greetModel.GreetingMessage,
                UserId = user.UserId
            };

            _context.GreetMessages.Add(greetingEntity);
            _context.SaveChanges();
            return true;
        }
        public RequestGreetingModel GetGreetingById(int ID, string email)
        {
            var entity = _context.GreetMessages
                                 .Include(g => g.User)
                                 .FirstOrDefault(g => g.Id == ID && g.User != null && g.User.Email == email);

            if (entity != null)
            {
                return new RequestGreetingModel()
                {
                    UserId = entity.Id,
                    GreetingMessage = entity.Greeting,
                    Email = entity.User?.Email,    // Null-check ensures no exception
                };
            }
            return null;
        }

        public List<GreetingEntity> GetAllGreetings(string email)
        {
            return _context.GreetMessages
                           .Include(g => g.User) // Correct navigation property
                           .Where(g => g.User.Email == email) // Access email via the User navigation property
                           .ToList();
        }


        public GreetingEntity EditGreeting(int id, GreetingModel greetingModel, string email)
        {
            var entity = _context.GreetMessages
                                 .Include(g => g.User)
                                 .FirstOrDefault(g => g.Id == id && g.User.Email == email);

            if (entity != null)
            {
                entity.Greeting = greetingModel.GreetingMessage;
                _context.GreetMessages.Update(entity);
                _context.SaveChanges();
                return entity;
            }
            return null; // Greeting not found or unauthorized access
        }


        public bool DeleteGreeting(int id, string email)
        {
            var entity = _context.GreetMessages
                                 .Include(g => g.User)
                                 .FirstOrDefault(g => g.Id == id && g.User.Email == email);

            if (entity != null)
            {
                _context.GreetMessages.Remove(entity);
                _context.SaveChanges();
                return true; // Successfully deleted
            }
            return false; // Greeting not found or unauthorized access
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