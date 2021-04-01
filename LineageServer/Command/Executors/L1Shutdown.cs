using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Command.Executors
{
	class L1Shutdown : ILineageCommand
	{
		public void Execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				if (arg == "now")
				{
					Container.Instance.Resolve<IRestartController>().Start(TimeSpan.FromSeconds(1));
				}
				else if (arg == "abort")
				{
					Container.Instance.Resolve<IRestartController>().Stop();
				}
				else
				{
					int sec = Math.Max(5, int.Parse(arg));
					Container.Instance.Resolve<IRestartController>().Start(TimeSpan.FromSeconds(sec));
				}
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage("請輸入: .shutdown sec|now|abort 。"));
			}
		}
	}
}