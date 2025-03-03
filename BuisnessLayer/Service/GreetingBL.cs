using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLayer.Interface;
using RepositoryLayer.Interface;

namespace BuisnessLayer.Service
{
    public class GreetingBL : IGreetingBL
    {
        private readonly IGreetingRL _greetingRL;
        public GreetingBL(IGreetingRL greetingRL)
        {
            _greetingRL = greetingRL;
        }   
        public string Greeting(string name)
        {
            return _greetingRL.GetHello(name);
        }
    }
}
