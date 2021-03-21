using ConsoleApp1.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Model
{
    abstract class RunnableBase : IRunnable
    {
        public abstract void Run();
    }
}
