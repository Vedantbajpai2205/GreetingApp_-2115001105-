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
        public bool GreetMessage(GreetingModel greetModel);

    }
}
