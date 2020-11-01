using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Chameleon;
using Chameleon.Contracts;
using Chameleon.Entities;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;

namespace TestConsoleApp
{
    public class TestConfig
    {
        public string TestProp { get; set; }
        
        
    }

    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection();


            serviceProvider.AddSingleton<IConfigReader<TestConfig>>(MongoDbConfigReader<TestConfig>.Create(
                "",
                "Configs",
                "EndorAPIConfigs"
            ));


            var provider = serviceProvider.BuildServiceProvider();


            var configReader = provider.GetService<IConfigReader<TestConfig>>();

            var config = configReader.GetConfig();


            for (int i = 0; i < 2222; i++)
            {
                Thread.Sleep(1500);
                Console.WriteLine(config.ToJson());
            }
        }
    }
}