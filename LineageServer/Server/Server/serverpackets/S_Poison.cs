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

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_Poison : ServerBasePacket
	{

		/// <summary>
		/// キャラクターの外見を毒状態へ変更する際に送信するパケットを構築する
		/// </summary>
		/// <param name="objId">
		///            外見を変えるキャラクターのID </param>
		/// <param name="type">
		///            外見のタイプ 0 = 通常色, 1 = 緑色, 2 = 灰色 </param>
		public S_Poison(int objId, int type)
		{
			WriteC(Opcodes.S_OPCODE_POISON);
			WriteD(objId);

			if (type == 0)
			{ // 通常
				WriteC(0);
				WriteC(0);
			}
			else if (type == 1)
			{ // 緑色
				WriteC(1);
				WriteC(0);
			}
			else if (type == 2)
			{ // 灰色
				WriteC(0);
				WriteC(1);
			}
			else
			{
				throw new System.ArgumentException("不正な引数です。type = " + type);
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
				return S_POISON;
			}
		}

		private const string S_POISON = "[S] S_Poison";
	}

}