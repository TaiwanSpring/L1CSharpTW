using LineageServer.Models;
using LineageServer.Interfaces;
using System.Text;

namespace LineageServer.Server.Console
{
    class ConsoleLogFormatter : IFormatter
    {
        public string format(LogRecord record)
        {
            StringBuilder output = new StringBuilder();
            output.Append(record.Message);
            output.AppendLine();
            if (record.Thrown != null)
            {
                output.Append(record.Thrown.StackTrace);
            }
            return output.ToString();
        }
    }

}