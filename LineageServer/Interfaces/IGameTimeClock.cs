using LineageServer.Server.Model.Gametime;
using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.Interfaces
{
	interface IGameTimeClock
	{
		L1GameTime CurrentTime();
		void AddListener(IL1GameTimeListener listener);
		void RemoveListener(IL1GameTimeListener listener);
	}
}
