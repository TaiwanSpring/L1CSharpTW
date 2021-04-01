using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using System;
using System.Threading;
namespace LineageServer.Server
{
	public class UbTimeController : IRunnable
	{
		public void run()
		{
			while (true)
			{
				checkUbTime(); // 開始檢查無限大戰的時間
				Thread.Sleep(15 * 1000);
			}
		}

		private void checkUbTime()
		{
			foreach (L1UltimateBattle ub in UBTable.Instance.AllUb)
			{
				if (ub.checkUbTime() && !ub.Active)
				{
					ub.start(); // 無限大戰開始
				}
			}
		}
	}
}