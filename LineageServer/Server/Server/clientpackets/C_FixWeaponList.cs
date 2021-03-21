﻿using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
/// <summary>
namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來要求修復武器清單的封包
	/// </summary>
	class C_FixWeaponList : ClientBasePacket
	{

		private const string C_FIX_WEAPON_LIST = "[C] C_FixWeaponList";

		public C_FixWeaponList(sbyte[] abyte0, ClientThread clientthread) : base(abyte0)
		{
			L1PcInstance pc = clientthread.ActiveChar;
			if (pc == null)
			{
				return;
			}
			pc.sendPackets(new S_FixWeaponList(pc));
		}

		public override string Type
		{
			get
			{
				return C_FIX_WEAPON_LIST;
			}
		}

	}

}