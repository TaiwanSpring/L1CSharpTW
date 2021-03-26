namespace LineageServer.Server.Server.serverpackets
{
	using Opcodes = LineageServer.Server.Server.Opcodes;
	using L1PetInstance = LineageServer.Server.Server.Model.Instance.L1PetInstance;

	public class S_PetEquipment : ServerBasePacket
	{
		private const string S_PET_EQUIPMENT = "[S] S_PetEquipment";

		/// <summary>
		/// 【Client】 id:60 size:8 time:1302335819781
		/// 0000	3c 00 04 bd 54 00 00 00                            <...T...
		/// 
		/// 【Server】 id:82 size:16 time:1302335819812
		/// 0000	52 25 00 04 bd 54 00 00 0a 37 80 08 7e ec d0 46    R%...T...7..~..F
		/// </summary>

		public S_PetEquipment(int i, L1PetInstance pet, int j)
		{
			WriteC(Opcodes.S_OPCODE_PACKETBOX);
			WriteC(0x25);
			WriteC(i);
			WriteD(pet.Id);
			WriteC(j);
			WriteC(pet.Ac); // 寵物防禦
		}

		public override sbyte[] Content
		{
			get
			{
				return Bytes;
			}
		}

		public override string Type
		{
			get
			{
				return S_PET_EQUIPMENT;
			}
		}
	}

}