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
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;

	public class S_UseMap : ServerBasePacket
	{
		private const string S_USE_MAP = "[S] S_UseMap";

		public S_UseMap(L1PcInstance pc, int objid, int itemid)
		{

			//WriteC(Opcodes.S_OPCODE_USEMAP);
			WriteD(objid);

			switch (itemid)
			{
				case 40373:
					WriteD(16);
					break;
				case 40374:
					WriteD(1);
					break;
				case 40375:
					WriteD(2);
					break;
				case 40376:
					WriteD(3);
					break;
				case 40377:
					WriteD(4);
					break;
				case 40378:
					WriteD(5);
					break;
				case 40379:
					WriteD(6);
					break;
				case 40380:
					WriteD(7);
					break;
				case 40381:
					WriteD(8);
					break;
				case 40382:
					WriteD(9);
					break;
				case 40383:
					WriteD(10);
					break;
				case 40384:
					WriteD(11);
					break;
				case 40385:
					WriteD(12);
					break;
				case 40386:
					WriteD(13);
					break;
				case 40387:
					WriteD(14);
					break;
				case 40388:
					WriteD(15);
					break;
				case 40389:
					WriteD(17);
					break;
				case 40390:
					WriteD(18);
					break;
			}
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
				return S_USE_MAP;
			}
		}
	}
}