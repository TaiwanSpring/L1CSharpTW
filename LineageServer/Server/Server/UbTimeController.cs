using System;
using System.Threading;

/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Server
{

	using UBTable = LineageServer.Server.Server.datatables.UBTable;
	using L1UltimateBattle = LineageServer.Server.Server.Model.L1UltimateBattle;

	public class UbTimeController : IRunnableStart
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(UbTimeController).FullName);

		private static UbTimeController _instance;

		public static UbTimeController Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new UbTimeController();
				}
				return _instance;
			}
		}

		public override void run()
		{
			try
			{
				while (true)
				{
					checkUbTime(); // 開始檢查無限大戰的時間
					Thread.Sleep(15000);
				}
			}
			catch (Exception e1)
			{
				_log.warning(e1.Message);
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