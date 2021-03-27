using System;
using System.IO;

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

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_Emblem : ServerBasePacket
	{
		private const string S_EMBLEM = "[S] S_Emblem";

		public S_Emblem(int emblemId)
		{
			BufferedInputStream bis = null;
			try
			{
				string emblem_file = emblemId.ToString();
				File file = new File("emblem/" + emblem_file);
				if (file.exists())
				{
					int data = 0;
					bis = new BufferedInputStream(new FileStream(file, FileMode.Open, FileAccess.Read));
					WriteC(Opcodes.S_OPCODE_EMBLEM);
					WriteD(emblemId);
					while ((data = bis.read()) != -1)
					{
						WriteP(data);
					}
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				if (bis != null)
				{
					try
					{
						bis.close();
					}
					catch (IOException)
					{
						// ignore
					}
				}
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
				return S_EMBLEM;
			}
		}
	}

}