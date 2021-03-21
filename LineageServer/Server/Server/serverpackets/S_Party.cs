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
			writeC(Opcodes.S_OPCODE_SHOWHTML);
			writeD(objid);
			writeS(htmlid);
			writeH(type);
			writeH(0x02);
			writeS(partyname);
			writeS(partymembers);
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
				writeC(Opcodes.S_OPCODE_PACKETBOX);
				writeC(S_PacketBox.UPDATE_OLD_PART_MEMBER);
				nowhp = leader.CurrentHp;
				maxhp = leader.MaxHp;
				writeC(member.Length - 1);
				writeD(leader.Id);
				writeS(leader.Name);
				writeC((int)(nowhp / maxhp) * 100);
				writeD(leader.MapId);
				writeH(leader.X);
				writeH(leader.Y);
				for (int i = 0, a = member.Length; i < a; i++)
				{
					if (member[i].Id == leader.Id || member[i] == null)
					{
						continue;
					}
					nowhp = member[i].CurrentHp;
					maxhp = member[i].MaxHp;
					writeD(member[i].Id);
					writeS(member[i].Name);
					writeC((int)(nowhp / maxhp) * 100);
					writeD(member[i].MapId);
					writeH(member[i].X);
					writeH(member[i].Y);
				}
				writeC(0x00);
			}
		}

		/// <summary>
		/// 舊的隊員
		/// </summary>
		/// <param name="pc"> </param>
		public virtual void oldMember(L1PcInstance pc)
		{
			writeC(Opcodes.S_OPCODE_PACKETBOX);
			writeC(S_PacketBox.PATRY_UPDATE_MEMBER);
			writeD(pc.Id);
			writeS(pc.Name);
			writeD(pc.MapId);
			writeH(pc.X);
			writeH(pc.Y);
		}

		/// <summary>
		/// 更換隊長
		/// </summary>
		/// <param name="pc"> </param>
		public virtual void changeLeader(L1PcInstance pc)
		{
			writeC(Opcodes.S_OPCODE_PACKETBOX);
			writeC(S_PacketBox.PATRY_SET_MASTER);
			writeD(pc.Id);
			writeH(0x0000);
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
				writeC(Opcodes.S_OPCODE_PACKETBOX);
				writeC(S_PacketBox.PATRY_MEMBERS);
				writeC(member.Length);
				for (int i = 0, a = member.Length; i < a; i++)
				{
					writeD(member[i].Id);
					writeD(member[i].MapId);
					writeH(member[i].X);
					writeH(member[i].Y);
				}
				writeC(0xff);
				writeC(0xff);
			}
		}

		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = _bao.toByteArray();
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