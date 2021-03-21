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
namespace LineageServer.Server.Server.Clientpackets
{
	using ClientThread = LineageServer.Server.Server.ClientThread;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using Teleportation = LineageServer.Server.Server.utils.Teleportation;

	// Referenced classes of package l1j.server.server.clientpackets:
	// ClientBasePacket

	/// <summary>
	/// 處理收到由客戶端傳來傳送的封包
	/// </summary>
	class C_Teleport : ClientBasePacket
	{

		private const string C_TELEPORT = "[C] C_Teleport";

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public C_Teleport(byte abyte0[], l1j.server.server.ClientThread clientthread) throws Exception
		public C_Teleport(sbyte[] abyte0, ClientThread clientthread) : base(abyte0)
		{

			L1PcInstance pc = clientthread.ActiveChar;
			if (pc == null)
			{
				return;
			}
			Teleportation.actionTeleportation(pc);
		}

		public override string Type
		{
			get
			{
				return C_TELEPORT;
			}
		}
	}

}