using LineageServer.Enum;
using LineageServer.Interfaces;
using System;

namespace LineageServer.Models
{
    class Logger : ILogger
    {
        public static ILogger GenericLogger { get; } = GetLogger("Generic");
        public static ILogger GetLogger(string name)
        {
            return new Logger(name);
        }

        private readonly string name;

        public Logger(string name)
        {
            this.name = name;
        }

        public void Info(string message)
        {
            Console.WriteLine($"{message}");
        }

        public void Log(string message)
        {
            Console.WriteLine($"{message}");
        }

        public void Warning(string message)
        {
            Console.WriteLine($"{message}");
        }

        public void Error(Exception e)
        {
            Console.WriteLine($"{e.Message}{Environment.NewLine}{e.StackTrace}");
        }
    }
}
