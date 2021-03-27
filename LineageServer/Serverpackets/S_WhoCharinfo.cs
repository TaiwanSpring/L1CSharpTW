using System;

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
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;

	public class S_WhoCharinfo : ServerBasePacket
	{
		private const string S_WHO_CHARINFO = "[S] S_WhoCharinfo";
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(S_WhoCharinfo).FullName);

		private byte[] _byte = null;

		public S_WhoCharinfo(L1PcInstance pc)
		{
			_log.fine("Who charpack for : " + pc.Name);

			string lawfulness = "";
			int lawful = pc.Lawful;
			if (lawful < 0)
			{
				lawfulness = "(Chaotic)";
			}
			else if (lawful >= 0 && lawful < 500)
			{
				lawfulness = "(Neutral)";
			}
			else if (lawful >= 500)
			{
				lawfulness = "(Lawful)";
			}

			WriteC(Opcodes.S_OPCODE_GLOBALCHAT);
			WriteC(0x08);

			string title = "";
			string clan = "";

			if (pc.Title.Equals("", StringComparison.OrdinalIgnoreCase) == false)
			{
				title = pc.Title + " ";
			}

			if (pc.Clanid > 0)
			{
				clan = "[" + pc.Clanname + "]";
			}

			WriteS(title + pc.Name + " " + lawfulness + " " + clan);
			// WriteD(0x80157FE4);
			WriteD(0);
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
				return S_WHO_CHARINFO;
			}
		}
	}

}