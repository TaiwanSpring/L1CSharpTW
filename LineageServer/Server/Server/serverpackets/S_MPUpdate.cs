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

	public class S_MPUpdate : ServerBasePacket
	{
		public S_MPUpdate(int currentmp, int maxmp)
		{
			WriteC(Opcodes.S_OPCODE_MPUPDATE);

			if (currentmp < 0)
			{
				WriteH(0);
			}
			else if (currentmp > 32767)
			{
				WriteH(32767);
			}
			else
			{
				WriteH(currentmp);
			}

			if (maxmp < 1)
			{
				WriteH(1);
			}
			else if (maxmp > 32767)
			{
				WriteH(32767);
			}
			else
			{
				WriteH(maxmp);
			}

			// WriteH(currentmp);
			// WriteH(maxmp);
			// WriteC(0);
			// WriteD(GameTimeController.getInstance().getGameTime());
		}

		public override sbyte[] Content
		{
			get
			{
				return Bytes;
			}
		}
	}

}