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
namespace LineageServer.Server.Server.Model.poison
{
	using L1Character = LineageServer.Server.Server.Model.L1Character;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_Poison = LineageServer.Server.Server.serverpackets.S_Poison;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;

	public abstract class L1Poison
	{
		protected internal static bool isValidTarget(L1Character cha)
		{
			if (cha == null)
			{
				return false;
			}
			// 毒は重複しない
			if (cha.Poison != null)
			{
				return false;
			}

			if (!(cha is L1PcInstance))
			{
				return true;
			}

			L1PcInstance player = (L1PcInstance) cha;
			if (player.Inventory.checkEquipped(20298) || player.Inventory.checkEquipped(20117) || player.Inventory.checkEquipped(21115) || player.Inventory.checkEquipped(21116) || player.Inventory.checkEquipped(21117) || player.Inventory.checkEquipped(21118) || player.hasSkillEffect(104)) // 黑暗妖精魔法(毒性抵抗)
			{
				return false;
			}
			return true;
		}

		// 微妙・・・素直にsendPacketsをL1Characterへ引き上げるべきかもしれない
		protected internal static void sendMessageIfPlayer(L1Character cha, int msgId)
		{
			if (!(cha is L1PcInstance))
			{
				return;
			}

			L1PcInstance player = (L1PcInstance) cha;
			player.sendPackets(new S_ServerMessage(msgId));
		}

		/// <summary>
		/// この毒のエフェクトIDを返す。
		/// </summary>
		/// <seealso cref= S_Poison#S_Poison(int, int)
		/// </seealso>
		/// <returns> S_Poisonで使用されるエフェクトID </returns>
		public abstract int EffectId {get;}

		/// <summary>
		/// この毒の効果を取り除く。<br>
		/// </summary>
		/// <seealso cref= L1Character#curePoison() </seealso>
		public abstract void cure();
	}

}