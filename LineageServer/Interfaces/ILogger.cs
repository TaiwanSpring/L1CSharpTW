using LineageServer.Enum;
using System;

namespace LineageServer.Interfaces
{
    interface ILogger
    {
        void log(Level level, string message, Exception e);
        void log(Level level, string message);
        void info(string message);
        void warning(string message);
    }
}
