using System;
using DotEnv.Core;

namespace DotEnv.Example.NetFramework
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var envVars = new EnvLoader().Load();
            Console.WriteLine(envVars["APP_BASE_URL"]);

            var reader = new EnvReader();
            Console.WriteLine(reader["APP_BASE_URL"]);

            Console.ReadLine();
        }
    }
}
