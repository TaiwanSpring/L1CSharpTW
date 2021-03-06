using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Command.Executors
{
    class L1HomeTown : ILineageCommand
	{

		public void Execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				StringTokenizer st = new StringTokenizer(arg);
				string para1 = st.nextToken();
				if (para1 == "daily")
				{
					HomeTownTimeController.Instance.dailyProc();
				}
				else if (para1 == "monthly")
				{
					HomeTownTimeController.Instance.monthlyProc();
				}
				else
				{
					throw new Exception();
				}
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage("請輸入 .hometown daily|monthly 。"));
			}
		}
	}

}