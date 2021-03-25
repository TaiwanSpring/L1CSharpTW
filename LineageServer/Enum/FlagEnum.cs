using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.Enum
{
    [Flags]
    enum FlagEnum
    {        
        NoFlag = 0x00,
        NewRow = 0x01,
        Dirty = NewRow << 1,
        Initialized = Dirty << 1,
    }
}
