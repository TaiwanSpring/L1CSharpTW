namespace LineageServer.Serverpackets
{
	using Opcodes = LineageServer.Server.Opcodes;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;

	public class S_EquipmentWindow : ServerBasePacket
	{
		private const string S_EQUIPMENTWINDOWS = "[S] S_EquipmentWindow";

		private byte[] _byte = null;

		/// <summary>
		/// 頭盔 </summary>
		public const sbyte EQUIPMENT_INDEX_HEML = 1;
		/// <summary>
		/// 盔甲 </summary>
		public const sbyte EQUIPMENT_INDEX_ARMOR = 2;
		/// <summary>
		/// T恤 </summary>
		public const sbyte EQUIPMENT_INDEX_T = 3;
		/// <summary>
		/// 斗篷 </summary>
		public const sbyte EQUIPMENT_INDEX_CLOAK = 4;
		/// <summary>
		/// 靴子 </summary>
		public const sbyte EQUIPMENT_INDEX_BOOTS = 5;
		/// <summary>
		/// 手套 </summary>
		public const sbyte EQUIPMENT_INDEX_GLOVE = 6;
		/// <summary>
		/// 盾 </summary>
		public const sbyte EQUIPMENT_INDEX_SHIELD = 7;
		/// <summary>
		/// 武器 </summary>
		public const sbyte EQUIPMENT_INDEX_WEAPON = 8;
		/// <summary>
		/// 項鏈 </summary>
		public const sbyte EQUIPMENT_INDEX_NECKLACE = 10;
		/// <summary>
		/// 腰帶 </summary>
		public const sbyte EQUIPMENT_INDEX_BELT = 11;
		/// <summary>
		/// 耳環 </summary>
		public const sbyte EQUIPMENT_INDEX_EARRING = 12;
		/// <summary>
		/// 戒指1 </summary>
		public const sbyte EQUIPMENT_INDEX_RING1 = 18;
		/// <summary>
		/// 戒指2 </summary>
		public const sbyte EQUIPMENT_INDEX_RING2 = 19;
		/// <summary>
		/// 戒指3 </summary>
		public const sbyte EQUIPMENT_INDEX_RING3 = 20;
		/// <summary>
		/// 戒指4 </summary>
		public const sbyte EQUIPMENT_INDEX_RING4 = 21;
		/// <summary>
		/// 符紋 </summary>
		public const sbyte EQUIPMENT_INDEX_RUNE1 = 22;

		public const sbyte EQUIPMENT_INDEX_RUNE2 = 23;

		public const sbyte EQUIPMENT_INDEX_RUNE3 = 24;

		public const sbyte EQUIPMENT_INDEX_RUNE4 = 25;

		public const sbyte EQUIPMENT_INDEX_RUNE5 = 26;

		/// <summary>
		/// 顯示指定物品到裝備窗口 </summary>
		/// <param name="itemObjId"> 對象ID </param>
		/// <param name="index"> 序號 </param>
		/// <param name="isEq"> 0:脫下 1:使用 </param>
		/// @param 0x00 (對應isEq)0:脫下 </param>
		/// @param 0x01 (對應isEq)1:使用  </param>
		public S_EquipmentWindow(L1PcInstance pc, int itemObjId, int index, bool isEq)
		{
			WriteC(Opcodes.S_OPCODE_CHARRESET);
			WriteC(0x42);
			WriteD(itemObjId);
			WriteC(index);
			if (isEq)
			{
				WriteC(0x01); //TODO 1:使用(0x01=1)
			}
			else
			{
				WriteC(0x00); //TODO 0:脫下(0x00=0)
			}
		}

		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = Bytes;
				}
				return _byte;
			}
		}

		public override string Type
		{
			get
			{
				return S_EQUIPMENTWINDOWS;
			}
		}
	}

}