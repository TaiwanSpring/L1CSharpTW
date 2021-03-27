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

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_SkillBuy : ServerBasePacket
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(S_SkillBuy).FullName);
		private const string _S_SKILL_BUY = "[S] S_SkillBuy";

		private byte[] _byte = null;

		public S_SkillBuy(int o, L1PcInstance pc)
		{
			int count = Scount(pc);
			int inCount = 0;
			for (int k = 0; k < count; k++)
			{
				if (!pc.isSkillMastery((k + 1)))
				{
					inCount++;
				}
			}

			try
			{
				WriteC(Opcodes.S_OPCODE_SKILLBUY);
				WriteD(100);
				WriteH(inCount);
				for (int k = 0; k < count; k++)
				{
					if (!pc.isSkillMastery((k + 1)))
					{
						WriteD(k);
					}
				}
			}
			catch (Exception e)
			{
				_log.Error(e);
			}
		}

		public virtual int Scount(L1PcInstance pc)
		{
			int RC = 0;
			switch (pc.Type)
			{
			case 0: // 君主
				if (pc.Level > 20 || pc.Gm)
				{
					RC = 16;
				}
				else if (pc.Level > 10)
				{
					RC = 8;
				}
				break;

			case 1: // ナイト
				if (pc.Level >= 50 || pc.Gm)
				{
					RC = 8;
				}
				break;

			case 2: // エルフ
				if (pc.Level >= 24 || pc.Gm)
				{
					RC = 23;
				}
				else if (pc.Level >= 16)
				{
					RC = 16;
				}
				else if (pc.Level >= 8)
				{
					RC = 8;
				}
				break;

			case 3: // WIZ
				if (pc.Level >= 12 || pc.Gm)
				{
					RC = 23;
				}
				else if (pc.Level >= 8)
				{
					RC = 16;
				}
				else if (pc.Level >= 4)
				{
					RC = 8;
				}
				break;

			case 4: // DE
				if (pc.Level >= 24 || pc.Gm)
				{
					RC = 16;
				}
				else if (pc.Level >= 12)
				{
					RC = 8;
				}
				break;

			default:
				break;
			}
			return RC;
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
				return _S_SKILL_BUY;
			}
		}

	}

}