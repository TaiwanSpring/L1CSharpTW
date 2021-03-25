using LineageServer.Models;

namespace LineageServer.Interfaces
{
    interface IFormatter
    {
        string format(LogRecord record);
    }
}
