using LineageServer.DataStruct;

namespace LineageServer.Interfaces
{
    interface IFormatter
    {
        string format(LogRecord record)
    }
}
