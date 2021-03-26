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
	using Opcodes = LineageServer.Server.Server.Opcodes;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;

	public class S_Party : ServerBasePacket
	{

		private const string _S_Party = "[S] S_Party";
		private byte[] _byte = null;

		public S_Party(string htmlid, int objid)
		{
			buildPacket(htmlid, objid, "", "", 0);
		}

		public S_Party(string htmlid, int objid, string partyname, string partymembers)
		{

			buildPacket(htmlid, objid, partyname, partymembers, 1);
		}

		private void buildPacket(string htmlid, int objid, string partyname, string partymembers, int type)
		{
			WriteC(Opcodes.S_OPCODE_SHOWHTML);
			WriteD(objid);
			WriteS(htmlid);
			WriteH(type);
			WriteH(0x02);
			WriteS(partyname);
			WriteS(partymembers);
		}

		public S_Party(int type, L1PcInstance pc)
		{ // 3.3C 組隊系統
			switch (type)
			{
			case 104:
				newMember(pc);
				break;
			case 105:
				oldMember(pc);
				break;
			case 106:
				changeLeader(pc);
				goto case 110;
			case 110:
				refreshParty(pc);
				break;
			default:
				break;
			}
		}

		/// <summary>
		/// 新加入隊伍的玩家
		/// </summary>
		/// <param name="pc"> </param>
		public virtual void newMember(L1PcInstance pc)
		{
			L1PcInstance leader = pc.Party.Leader;
			L1PcInstance[] member = pc.Party.Members;
			double nowhp = 0.0d;
			double maxhp = 0.0d;
			if (pc.Party == null)
			{
				return;
			}
			else
			{
				WriteC(Opcodes.S_OPCODE_PACKETBOX);
				WriteC(S_PacketBox.UPDATE_OLD_PART_MEMBER);
				nowhp = leader.CurrentHp;
				maxhp = leader.MaxHp;
				WriteC(member.Length - 1);
				WriteD(leader.Id);
				WriteS(leader.Name);
				WriteC((int)(nowhp / maxhp) * 100);
				WriteD(leader.MapId);
				WriteH(leader.X);
				WriteH(leader.Y);
				for (int i = 0, a = member.Length; i < a; i++)
				{
					if (member[i].Id == leader.Id || member[i] == null)
					{
						continue;
					}
					nowhp = member[i].CurrentHp;
					maxhp = member[i].MaxHp;
					WriteD(member[i].Id);
					WriteS(member[i].Name);
					WriteC((int)(nowhp / maxhp) * 100);
					WriteD(member[i].MapId);
					WriteH(member[i].X);
					WriteH(member[i].Y);
				}
				WriteC(0x00);
			}
		}

		/// <summary>
		/// 舊的隊員
		/// </summary>
		/// <param name="pc"> </param>
		public virtual void oldMember(L1PcInstance pc)
		{
			WriteC(Opcodes.S_OPCODE_PACKETBOX);
			WriteC(S_PacketBox.PATRY_UPDATE_MEMBER);
			WriteD(pc.Id);
			WriteS(pc.Name);
			WriteD(pc.MapId);
			WriteH(pc.X);
			WriteH(pc.Y);
		}

		/// <summary>
		/// 更換隊長
		/// </summary>
		/// <param name="pc"> </param>
		public virtual void changeLeader(L1PcInstance pc)
		{
			WriteC(Opcodes.S_OPCODE_PACKETBOX);
			WriteC(S_PacketBox.PATRY_SET_MASTER);
			WriteD(pc.Id);
			WriteH(0x0000);
		}

		/// <summary>
		/// 更新隊伍
		/// </summary>
		/// <param name="pc"> </param>
		public virtual void refreshParty(L1PcInstance pc)
		{
			L1PcInstance[] member = pc.Party.Members;
			if (pc.Party == null)
			{
				return;
			}
			else
			{
				WriteC(Opcodes.S_OPCODE_PACKETBOX);
				WriteC(S_PacketBox.PATRY_MEMBERS);
				WriteC(member.Length);
				for (int i = 0, a = member.Length; i < a; i++)
				{
					WriteD(member[i].Id);
					WriteD(member[i].MapId);
					WriteH(member[i].X);
					WriteH(member[i].Y);
				}
				WriteC(0xff);
				WriteC(0xff);
			}
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
				return _S_Party;
			}
		}

	}

}