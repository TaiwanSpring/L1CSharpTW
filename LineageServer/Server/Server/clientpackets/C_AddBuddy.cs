﻿using LineageServer.Server.Server.datatables;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Templates;
using System;
namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理客戶端傳來增加好友的封包
	/// </summary>
	class C_AddBuddy : ClientBasePacket
	{

		private const string C_ADD_BUDDY = "[C] C_AddBuddy";

		public C_AddBuddy(sbyte[] decrypt, ClientThread client) : base(decrypt)
		{

			L1PcInstance pc = client.ActiveChar;
			if (pc == null)
			{
				return;
			}

			BuddyTable buddyTable = BuddyTable.Instance;
			L1Buddy buddyList = buddyTable.getBuddyTable(pc.Id);
			string charName = readS();

			if (charName.Equals(pc.Name, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			else if (buddyList.containsName(charName))
			{
				pc.sendPackets(new S_ServerMessage(1052, charName)); // %s
																		// は既に登録されています。
				return;
			}

			foreach (L1CharName cn in CharacterTable.Instance.CharNameList)
			{
				if (charName.Equals(cn.Name, StringComparison.OrdinalIgnoreCase))
				{
					int objId = cn.Id;
					string name = cn.Name;
					buddyList.add(objId, name);
					buddyTable.addBuddy(pc.Id, objId, name);
					return;
				}
			}
			pc.sendPackets(new S_ServerMessage(109, charName)); // %0という名前の人はいません。
		}

		public override string Type
		{
			get
			{
				return C_ADD_BUDDY;
			}
		}
	}

}