﻿using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.Extensions.DependencyInjection;
using Test_Taste_Console_Application.Constants;
using Test_Taste_Console_Application.Domain.Services;
using Test_Taste_Console_Application.Domain.Services.Interfaces;
using Test_Taste_Console_Application.Utilities;

namespace Test_Taste_Console_Application
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Program started.");
            var serviceCollection = new ServiceCollection();
            //The ConfigureServices function configures the services.
            ConfigureServices(serviceCollection);
            Console.WriteLine("Configuring services...");
            //The RunServiceOperations function executes the code that can create the outputs.
            Console.WriteLine("Running main logic...");
            RunServiceOperations(serviceCollection);

            Console.WriteLine("Program finished. Press any key to exit.");
            Console.ReadKey();
        }

        private static void RunServiceOperations(IServiceCollection serviceCollection)
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();

            //The service provider gets the services.
            var screenOutputService = serviceProvider.GetService<IOutputService>();
            if (screenOutputService == null)
            {
                Console.WriteLine("Failed to resolve IOutputService.");
                return;
            }

            try
            {
                screenOutputService.OutputAllPlanetsAndTheirAverageMoonGravityToConsole();
                screenOutputService.OutputAllMoonsAndTheirMassToConsole();
                screenOutputService.OutputAllPlanetsAndTheirMoonsToConsole();
            }
            catch (Exception exception)
            {
                //The users and developers can see the thrown exceptions.
                Logger.Instance.Error($"{LoggerMessage.ScreenOutputOperationFailed}{exception.Message}");
                Console.WriteLine($"{ExceptionMessage.ScreenOutputOperationFailed}{exception.Message}");
                System.Diagnostics.Debug.WriteLine($"{ExceptionMessage.ScreenOutputOperationFailed}{exception.Message}");
            }

            serviceProvider.Dispose();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            //The function configures all the services.
            XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetEntryAssembly()),
                new FileInfo(ConfigurationFileName.Logger));
            serviceCollection.AddHttpClient<HttpClientService>();
            serviceCollection.AddSingleton<IPlanetService, PlanetService>();
            serviceCollection.AddSingleton<IOutputService, ScreenOutputService>();
            serviceCollection.AddSingleton<IMoonService, MoonService>();
        }
    }
}
