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

	public class S_HowManyMake : ServerBasePacket
	{
		public S_HowManyMake(int objId, int max, string htmlId)
		{
			WriteC(Opcodes.S_OPCODE_INPUTAMOUNT);
			WriteD(objId);
			WriteD(0); // ?
			WriteD(0); // スピンコントロールの初期価格
			WriteD(0); // 価格の下限
			WriteD(max); // 価格の上限
			WriteH(0); // ?
			WriteS("request");
			WriteS(htmlId);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public byte[] getContent() throws java.io.IOException
		public override sbyte[] Content
		{
			get
			{
				return Bytes;
			}
		}
	}

}