using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.DataStruct
{
    struct LogRecord
    {
        public string Message { get; set; }

        public Exception Thrown { get; set; }
    }
}
