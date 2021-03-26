using LineageServer.Enum;
using LineageServer.Interfaces;
using System;

namespace LineageServer
{
    class Logger : ILogger
    {
        public static ILogger getLogger(string name)
        {
            return new Logger();
        }

        public void info(string message)
        {
          
        }

        public void log(Level level, string message, Exception e)
        {
            throw e;
        }

        public void log(Level level, string message)
        {

        }

        public void warning(string message)
        {
            
        }
    }
}
