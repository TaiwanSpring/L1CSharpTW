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
namespace LineageServer.Serverpackets
{
	using Opcodes = LineageServer.Server.Opcodes;
	using L1FollowerInstance = LineageServer.Server.Model.Instance.L1FollowerInstance;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket, S_NPCPack

	public class S_FollowerPack : ServerBasePacket
	{

		private const string S_FOLLOWER_PACK = "[S] S_FollowerPack";

		private const int STATUS_POISON = 1;

		private byte[] _byte = null;

		public S_FollowerPack(L1FollowerInstance follower, L1PcInstance pc)
		{
			WriteC(Opcodes.S_OPCODE_CHARPACK);
			WriteH(follower.X);
			WriteH(follower.Y);
			WriteD(follower.Id);
			WriteH(follower.GfxId);
			WriteC(follower.Status);
			WriteC(follower.Heading);
			WriteC(follower.ChaLightSize);
			WriteC(follower.MoveSpeed);
			WriteD(0);
			WriteH(0);
			WriteS(follower.NameId);
			WriteS(follower.Title);
			int status = 0;
			if (follower.Poison != null)
			{ // 毒状態
				if (follower.Poison.EffectId == 1)
				{
					status |= STATUS_POISON;
				}
			}
			WriteC(status);
			WriteD(0);
			WriteS(null);
			WriteS(null);
			WriteC(0);
			WriteC(0xFF);
			WriteC(0);
			WriteC(follower.Level);
			WriteC(0);
			WriteC(0xFF);
			WriteC(0xFF);
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
				return S_FOLLOWER_PACK;
			}
		}

	}

}