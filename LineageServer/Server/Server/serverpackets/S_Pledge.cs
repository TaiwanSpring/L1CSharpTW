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
namespace LineageServer.Server.Server.serverpackets
{

	using Opcodes = LineageServer.Server.Server.Opcodes;
	using CharacterTable = LineageServer.Server.Server.datatables.CharacterTable;
	using ClanTable = LineageServer.Server.Server.datatables.ClanTable;
	using L1Clan = LineageServer.Server.Server.Model.L1Clan;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;

	//Referenced classes of package l1j.server.server.serverpackets:
	//ServerBasePacket


	public class S_Pledge : ServerBasePacket
	{

		private const string _S_Pledge = "[S] _S_Pledge";

		private byte[] _byte = null;

		/// <summary>
		/// 盟友查詢 公告視窗 </summary>
		/// <param name="ClanId"> 血盟Id </param>
		public S_Pledge(int ClanId)
		{
			L1Clan clan = ClanTable.Instance.getTemplate(ClanId);
			writeC(Opcodes.S_OPCODE_PACKETBOX);
			writeC(S_PacketBox.HTML_PLEDGE_ANNOUNCE);
			writeS(clan.ClanName);
			writeS(clan.LeaderName);
			writeD(clan.EmblemId); // 盟徽id
			writeD((int)(clan.FoundDate.Time / 1000)); // 血盟創立日
			try
			{
				sbyte[] text = new sbyte[478];
				Arrays.Fill(text, (sbyte) 0);
				int i = 0;
				foreach (sbyte b in clan.Announcement.GetBytes("Big5"))
				{
					text[i++] = b;
				}
				writeByte(text);
			}
			catch (UnsupportedEncodingException e)
			{
                System.Console.WriteLine(e.ToString());
                System.Console.Write(e.StackTrace);
			}
		}

		/// <summary>
		/// 盟友查詢 盟友清單 </summary>
		/// <param name="clanName"> </param>
		/// <exception cref="Exception">  </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public S_Pledge(l1j.server.server.model.Instance.L1PcInstance pc) throws Exception
		public S_Pledge(L1PcInstance pc)
		{
			L1Clan clan = ClanTable.Instance.getTemplate(pc.Clanid);
			writeC(Opcodes.S_OPCODE_PACKETBOX);
			writeC(S_PacketBox.HTML_PLEDGE_MEMBERS);
			writeH(1);
			writeC(clan.AllMembers.Length); // 血盟總人數

			// 血盟成員資料
			/* Name/Rank/Level/Notes/MemberId/ClassType */
			foreach (string member in clan.AllMembers)
			{
				L1PcInstance clanMember = CharacterTable.Instance.restoreCharacter(member);
				writeS(clanMember.Name);
				writeC(clanMember.ClanRank);
				writeC(clanMember.Level);

				/// <summary>
				/// 產生全由0填充的byte陣列 </summary>
				sbyte[] text = new sbyte[62];
				Arrays.Fill(text, (sbyte) 0);

				/// <summary>
				/// 將備註字串填入byte陣列 </summary>
				if (clanMember.ClanMemberNotes.Length != 0)
				{
					int i = 0;
					foreach (sbyte b in clanMember.ClanMemberNotes.GetBytes("Big5"))
					{
						text[i++] = b;
					}
				}
				writeByte(text);
				writeD(clanMember.ClanMemberId);
				writeC(clanMember.Type);
			}
		}

		/// <summary>
		/// 盟友查詢 寫入備註 </summary>
		/// <param name="name"> 玩家名稱 </param>
		/// <param name="notes"> 備註文字 </param>
		public S_Pledge(string name, string notes)
		{
			writeC(Opcodes.S_OPCODE_PACKETBOX);
			writeC(S_PacketBox.HTML_PLEDGE_WRITE_NOTES);
			writeS(name);

			/// <summary>
			/// 產生全由0填充的byte陣列 </summary>
			sbyte[] text = new sbyte[62];
			Arrays.Fill(text, (sbyte) 0);

			/// <summary>
			/// 將備註字串填入byte陣列 </summary>
			if (notes.Length != 0)
			{
				int i = 0;
				try
				{
					foreach (sbyte b in notes.GetBytes("Big5"))
					{
						text[i++] = b;
					}
				}
				catch (UnsupportedEncodingException e)
				{
                    System.Console.WriteLine(e.ToString());
                    System.Console.Write(e.StackTrace);
				}
			}
			writeByte(text);
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
				return _S_Pledge;
			}
		}
	}

}