using LineageServer.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.Interfaces
{
    interface IHasFlag
    {
        bool Has(FlagEnum flag);
    }
}
