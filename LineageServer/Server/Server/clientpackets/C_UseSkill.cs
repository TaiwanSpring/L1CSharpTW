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
namespace LineageServer.Server.Server.Clientpackets
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.ABSOLUTE_BARRIER;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.CALL_CLAN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.FIRE_WALL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.LIFE_STREAM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.MASS_TELEPORT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.MEDITATION;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.RUN_CLAN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.TELEPORT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.TRUE_TARGET;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.SUMMON_MONSTER;
	using Config = LineageServer.Server.Config;
	using ActionCodes = LineageServer.Server.Server.ActionCodes;
	using ClientThread = LineageServer.Server.Server.ClientThread;
	using SkillsTable = LineageServer.Server.Server.datatables.SkillsTable;
	using AcceleratorChecker = LineageServer.Server.Server.Model.AcceleratorChecker;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1SkillUse = LineageServer.Server.Server.Model.skill.L1SkillUse;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;

	// Referenced classes of package l1j.server.server.clientpackets:
	// ClientBasePacket

	/// <summary>
	/// 處理收到由客戶端傳來使用魔法的封包
	/// </summary>
	class C_UseSkill : ClientBasePacket
	{

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public C_UseSkill(byte abyte0[], l1j.server.server.ClientThread client) throws Exception
		public C_UseSkill(sbyte[] abyte0, ClientThread client) : base(abyte0)
		{

			L1PcInstance pc = client.ActiveChar;
			if ((pc == null) || pc.Teleport || pc.Dead)
			{
				return;
			}

			int row = readC();
			int column = readC();
			int skillId = (row * 8) + column + 1;
			string charName = null;
			string message = null;
			int targetId = 0;
			int targetX = 0;
			int targetY = 0;

			if (!pc.Map.UsableSkill)
			{
				pc.sendPackets(new S_ServerMessage(563)); // \f1ここでは使えません。
				return;
			}
			if (!pc.isSkillMastery(skillId))
			{
				return;
			}

			// 檢查使用魔法的間隔
			if (Config.CHECK_SPELL_INTERVAL)
			{
				int result;
				// FIXME 判斷有向及無向的魔法
				if (SkillsTable.Instance.getTemplate(skillId).ActionId == ActionCodes.ACTION_SkillAttack)
				{
					result = pc.AcceleratorChecker.checkInterval(AcceleratorChecker.ACT_TYPE.SPELL_DIR);
				}
				else
				{
					result = pc.AcceleratorChecker.checkInterval(AcceleratorChecker.ACT_TYPE.SPELL_NODIR);
				}
				if (result == AcceleratorChecker.R_DISPOSED)
				{
					return;
				}
			}

			if (abyte0.Length > 4)
			{
				try
				{
					if ((skillId == CALL_CLAN) || (skillId == RUN_CLAN))
					{ // コールクラン、ランクラン
						charName = readS();
					}
					else if (skillId == TRUE_TARGET)
					{ // トゥルーターゲット
						targetId = readD();
						targetX = readH();
						targetY = readH();
						message = readS();
					}
					else if ((skillId == TELEPORT) || (skillId == MASS_TELEPORT))
					{ // テレポート、マステレポート
						readH(); // MapID
						targetId = readD(); // Bookmark ID
					}
					else if ((skillId == FIRE_WALL) || (skillId == LIFE_STREAM))
					{ // ファイアーウォール、ライフストリーム
						targetX = readH();
						targetY = readH();
					}
					else if (skillId == SUMMON_MONSTER)
					{ // 法師魔法 (召喚術)
						if (pc.Inventory.checkEquipped(20284))
						{ // 有裝備召喚戒指
							int summonId = readD();
							pc.SummonId = summonId;
						}
						else
						{
							targetId = readD();
						}
					}
					else
					{
						targetId = readD();
						targetX = readH();
						targetY = readH();
					}
				}
				catch (Exception)
				{
					// _log.log(Enum.Level.Server, "", e);
				}
			}

			if (pc.hasSkillEffect(L1SkillId.ABSOLUTE_BARRIER))
			{ // 取消絕對屏障
				pc.removeSkillEffect(L1SkillId.ABSOLUTE_BARRIER);
			}
			if (pc.hasSkillEffect(L1SkillId.MEDITATION))
			{ // 取消冥想效果
				pc.removeSkillEffect(L1SkillId.MEDITATION);
			}

			try
			{
				if ((skillId == CALL_CLAN) || (skillId == RUN_CLAN))
				{ // コールクラン、ランクラン
					if (charName.Length == 0)
					{
						// 名前が空の場合クライアントで弾かれるはず
						return;
					}

					L1PcInstance target = L1World.Instance.getPlayer(charName);

					if (target == null)
					{
						// メッセージが正確であるか未調査
						pc.sendPackets(new S_ServerMessage(73, charName)); // \f1%0はゲームをしていません。
						return;
					}
					if (pc.Clanid != target.Clanid)
					{
						pc.sendPackets(new S_ServerMessage(414)); // 同じ血盟員ではありません。
						return;
					}
					targetId = target.Id;
					if (skillId == CALL_CLAN)
					{
						// 移動せずに連続して同じクラン員にコールクランした場合、向きは前回の向きになる
						int callClanId = pc.CallClanId;
						if ((callClanId == 0) || (callClanId != targetId))
						{
							pc.CallClanId = targetId;
							pc.CallClanHeading = pc.Heading;
						}
					}
				}
				L1SkillUse l1skilluse = new L1SkillUse();
				l1skilluse.handleCommands(pc, skillId, targetId, targetX, targetY, message, 0, L1SkillUse.TYPE_NORMAL);

			}
			catch (Exception e)
			{
                System.Console.WriteLine(e.ToString());
                System.Console.Write(e.StackTrace);
			}
		}
	}

}