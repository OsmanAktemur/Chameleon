using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Chameleon;
using Chameleon.Contracts;
using Chameleon.Entities;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;

namespace TestConsoleApp
{
    public class TestConfig
    {
        private readonly IConfigReader<TestConfig> _configReader;

        public TestConfig(IConfigReader<TestConfig> configReader)
        {
            _configReader = configReader;
        }
        
        
 
    }
    
    
    
    
    
    
    public class SomeService
    {
        private readonly IConfigReader<TestConfig> _configReader;

        public SomeService(IConfigReader<TestConfig> configReader)
        {
            _configReader = configReader;
        }

    }

    
    
    
    
    
    


    public class TestConfigSub
    {
        public string ProjectName2 { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection();


            serviceProvider.AddSingleton<IConfigReader<TestConfig>>(new MongoDbConfigReader<TestConfig>(
                "",
                "Configs",
                "EndorAPIConfigs"
            ));


            var provider = serviceProvider.BuildServiceProvider();


   


            var configReader = provider.GetService<IConfigReader<TestConfig>>();

            var  config = configReader.GetConfig();


            if (config.c)
            {
                
            }
            
            var value2 = value;
            var value3 = value2;


            for (int i = 0; i < 2222; i++)
            {
                Thread.Sleep(1500);
                Console.WriteLine(value3.ToJson());
            }
        }
    }
}
