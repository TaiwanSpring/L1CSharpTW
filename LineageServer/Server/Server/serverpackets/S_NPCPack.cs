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
namespace LineageServer.Server.Server.serverpackets
{
	using Config = LineageServer.Server.Config;
	using Opcodes = LineageServer.Server.Server.Opcodes;
	using NPCTalkDataTable = LineageServer.Server.Server.DataSources.NPCTalkDataTable;
	using L1NpcTalkData = LineageServer.Server.Server.Model.L1NpcTalkData;
	using L1FieldObjectInstance = LineageServer.Server.Server.Model.Instance.L1FieldObjectInstance;
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_NPCPack : ServerBasePacket
	{

		private const string S_NPC_PACK = "[S] S_NPCPack";

		private const int STATUS_POISON = 1;

		private const int STATUS_PC = 4;

		private const int HIDDEN_STATUS_FLY = 2;

		private byte[] _byte = null;

		public S_NPCPack(L1NpcInstance npc)
		{
			WriteC(Opcodes.S_OPCODE_CHARPACK);
			WriteH(npc.X);
			WriteH(npc.Y);
			WriteD(npc.Id);
			if (npc.TempCharGfx == 0)
			{
				WriteH(npc.GfxId);
			}
			else
			{
				WriteH(npc.TempCharGfx);
			}
			WriteC(npc.Status);
			WriteC(npc.Heading);
			WriteC(npc.ChaLightSize);
			WriteC(npc.MoveSpeed);
			WriteExp(npc.Exp);
			WriteH(npc.TempLawful);
			if (Config.SHOW_NPC_ID)
			{
				WriteS(npc.NameId + "[" + npc.NpcId + "]" + "面向[" + npc.Heading + "]" + "圖形[" + npc.GfxId + "]");
			}
			else
			{
				WriteS(npc.NameId);
			}
			if (npc is L1FieldObjectInstance)
			{ // SICの壁字、看板など
				L1NpcTalkData talkdata = NPCTalkDataTable.Instance.getTemplate(npc.NpcTemplate.get_npcId());
				if (talkdata != null)
				{
					WriteS(talkdata.NormalAction); // タイトルがHTML名として解釈される
				}
				else
				{
					WriteS(null);
				}
			}
			else
			{
				WriteS(npc.Title);
			}

			/// <summary>
			/// シシニテ - 0:mob,item(atk pointer), 1:poisoned(), 2:invisable(), 4:pc,
			/// 8:cursed(), 16:brave(), 32:??, 64:??(??), 128:invisable but name
			/// </summary>
			int status = 0;
			if (npc.Poison != null)
			{ // 毒状態
				if (npc.Poison.EffectId == 1)
				{
					status |= STATUS_POISON;
				}
			}
			if (npc.NpcTemplate.is_doppel())
			{
				// 變形怪需強制攻擊判斷
				if (npc.GfxId != 31 && npc.NpcTemplate.get_npcId() != 81069)
				{
					status |= STATUS_PC;
				}
			}
			// 二段加速狀態
			status |= npc.BraveSpeed * 16;

			WriteC(status);

			WriteD(0); // 0以外にするとC_27が飛ぶ
			WriteS(null);
			WriteS(null); // マスター名？
			if (npc.TempCharGfx == 1024 || npc.TempCharGfx == 2363 || npc.TempCharGfx == 6697 || npc.TempCharGfx == 8180 || npc.TempCharGfx == 1204 || npc.TempCharGfx == 2353 || npc.TempCharGfx == 3631 || npc.TempCharGfx == 2544)
			{ // 飛行系怪物
				WriteC(npc.HiddenStatus == HIDDEN_STATUS_FLY ? 2 : 1); // 判斷是否飛天中
			}
			else
			{
				WriteC(0);
			}
			WriteC(0xFF); // HP
			WriteC(0);
			WriteC(npc.Level);
			WriteC(0xFF);
			WriteC(0xFF);
			WriteC(0);
		}

		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = memoryStream.toByteArray();
				}
    
				return _byte;
			}
		}

		public override string Type
		{
			get
			{
				return S_NPC_PACK;
			}
		}

	}

}