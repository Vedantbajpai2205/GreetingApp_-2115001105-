using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;

namespace BuisnessLayer.Interface
{
    public interface IGreetingBL
    {
        public string GetGreet();
        public string greeting(UserNameModel userName);
        public bool GreetMessage(RequestGreetingModel greetModel);

        public RequestGreetingModel GetGreetingById(int id, string email);
        public List<RequestGreetingModel> GetAllGreetings(string email);

        public GreetingModel EditGreeting(int id, GreetingModel greetingModel, string email);
        public bool DeleteGreeting(int id, string email);

    }
}
