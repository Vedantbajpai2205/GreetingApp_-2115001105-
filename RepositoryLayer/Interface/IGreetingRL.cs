using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        public string Greeting(UserNameModel nameModel);
        public bool GreetMessage(RequestGreetingModel greetModel);

        public RequestGreetingModel GetGreetingById(int ID, string email);
        public List<GreetingEntity> GetAllGreetings(string email);


        public GreetingEntity EditGreeting(int id, GreetingModel greetingModel, string email);

        public bool DeleteGreeting(int id, string email);
    }
}
