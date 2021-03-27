using LineageServer.Enum;
using System;

namespace LineageServer.Interfaces
{
    interface ILogger
    {
        void Info(string message);

        void Error(Exception e);

        void Log(string message);

        void Warning(string message);
    }
}
