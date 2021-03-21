using System;

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
namespace LineageServer.Server.Server.Model
{

	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_Party = LineageServer.Server.Server.serverpackets.S_Party;

	public class L1PartyRefresh : TimerTask
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1PartyRefresh).FullName);

		private readonly L1PcInstance _pc;

		public L1PartyRefresh(L1PcInstance pc)
		{
			_pc = pc;
		}

		/// <summary>
		/// 3.3C 更新隊伍封包
		/// </summary>
		public virtual void fresh()
		{
			_pc.sendPackets(new S_Party(110, _pc));
		}

		public override void run()
		{
			try
			{
				if (_pc.Dead || _pc.Party == null)
				{
					_pc.stopRefreshParty();
					return;
				}
				fresh();
			}
			catch (Exception e)
			{
				_pc.stopRefreshParty();
				_log.log(Level.WARNING, e.Message, e);
			}
		}
	}

}