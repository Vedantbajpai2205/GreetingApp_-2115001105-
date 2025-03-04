﻿using BuisnessLayer.Interface;
using ModelLayer.Model;
using NLog;
using RepositoryLayer.Interface;
using System;

namespace BusinessLayer.Services
{
    public class GreetingBL : IGreetingBL
    {
        private readonly IGreetingRL _greetingRL;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public GreetingBL(IGreetingRL greetingRL)
        {
            _greetingRL = greetingRL;
        }

        public string GetGreet()
        {
            return "Hello World";

        }
        public string greeting(UserNameModel nameModel)
        {
            return _greetingRL.Greeting(nameModel);
        }
    }
}