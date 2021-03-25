using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.Models
{
    struct LogRecord
    {
        public string Message { get; set; }

        public Exception Thrown { get; set; }
    }
}
