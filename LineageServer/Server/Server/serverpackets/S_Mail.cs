using System;
using System.Collections.Generic;

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
	using MailTable = LineageServer.Server.Server.DataSources.MailTable;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1Mail = LineageServer.Server.Server.Templates.L1Mail;
	using Lists = LineageServer.Server.Server.Utils.collections.Lists;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_Mail : ServerBasePacket
	{

		private const string S_MAIL = "[S] S_Mail";

		private byte[] _byte = null;

		/// <summary>
		/// 『來源:伺服器』<位址:186>{長度:216}(時間:1061159132)
		///  0000:  ba 00 08 00 63 78 00 00 01 d8 dc 25 52 01 ae a9    ....cx.....%R...
		///  0010:  ac f5 b9 d0 00 35 00 35 00 35 00 35 00 35 00 35    .....5.5.5.5.5.5
		///  0020:  00 00 00 65 78 00 00 01 c8 dd 25 52 01 ae a9 ac    ...ex.....%R....
		///  0030:  f5 b9 d0 00 32 00 32 00 32 00 32 00 32 00 32 00    ....2.2.2.2.2.2.
		///  0040:  32 00 32 00 32 00 00 00 eb 78 00 00 00 50 2f 3d    2.2.2....x...P/=
		///  0050:  52 00 ae a9 ac f5 b9 d0 00 33 00 33 00 33 00 00    R........3.3.3..
		///  0060:  00 ed 78 00 00 00 50 2f 3d 52 00 ae a9 ac f5 b9    ..x...P/=R......
		///  0070:  d0 00 33 00 33 00 00 00 ef 78 00 00 00 40 30 3d    ..3.3....x...@0=
		///  0080:  52 00 ae a9 ac f5 b9 d0 00 32 00 32 00 00 00 f1    R........2.2....
		///  0090:  78 00 00 00 40 30 3d 52 00 ae a9 ac f5 b9 d0 00    x...@0=R........
		///  00a0:  32 00 32 00 00 00 f3 78 00 00 00 d4 32 3d 52 00    2.2....x....2=R.
		///  00b0:  ae a9 ac f5 b9 d0 00 32 00 32 00 00 00 f5 78 00    .......2.2....x.
		///  00c0:  00 00 d0 36 3d 52 00 ae a9 ac f5 b9 d0 00 35 00    ...6=R........5.
		///  00d0:  35 00 00 00 45 1c c9 93                            5...E...
		/// </summary>
		// 打開收信夾 ?封信件顯示標題
		public S_Mail(L1PcInstance pc, int type)
		{
			IList<L1Mail> mails = Lists.newList();
			MailTable.Instance;
			foreach (L1Mail mail in MailTable.AllMail)
			{
				if (mail.InBoxId == pc.Id)
				{
					if (mail.Type == type)
					{
						mails.Add(mail);
					}
				}
			}

			WriteC(Opcodes.S_OPCODE_MAIL);
			WriteC(type);
			WriteH(mails.Count);

			if (mails.Count == 0)
			{
				return;
			}

			for (int i = 0; i < mails.Count; i++)
			{
				L1Mail mail = mails[i];
				WriteD(mail.Id);
				WriteC(mail.ReadStatus);
				WriteD((int)(mail.Date.Time / 1000));
				WriteC(mail.SenderName.Equals(pc.Name, StringComparison.OrdinalIgnoreCase) ? 1 : 0); // 寄件/備份
				WriteS(mail.SenderName.Equals(pc.Name, StringComparison.OrdinalIgnoreCase) ? mail.ReceiverName : mail.SenderName);
				WriteByte(mail.Subject);
			}
		}

		/// <summary>
		/// <b>寄出信件</b> </summary>
		/// <param name="pc"> 寄出信件: 寄信人 , 寄件備份: 收信人<br> </param>
		/// <param name="isDraft"> 是否是寄件備份 ? true:備份  , false:寄出 </param>
		public S_Mail(L1PcInstance pc, int mailId, bool isDraft)
		{
			MailTable.Instance;
			L1Mail mail = MailTable.getMail(mailId);
			WriteC(Opcodes.S_OPCODE_MAIL);
			WriteC(0x50);
			WriteD(mailId);
			WriteC(isDraft ? 1 : 0);
			WriteS(pc.Name);
			WriteByte(mail.Subject);
		}

		/// <summary>
		/// 寄信結果通知 </summary>
		/// <param name="type"> 信件類別 </param>
		/// <param name="isDelivered"> 寄出:1 ,失敗:0 </param>
		public S_Mail(int type, bool isDelivered)
		{
			WriteC(Opcodes.S_OPCODE_MAIL);
			WriteC(type);
			WriteC(isDelivered ? 1 : 0);
		}

		/// <summary>
		/// //讀取一般信件 [Server] opcode = 48 0000: [30] [10] [29 00 00 00] [32 00] 00 00
		/// a4 cb 00 03 08 00 0.)...2.........
		/// 
		/// //信件存到保管箱 [Server] opcode = 48 0000: [30] [40] [2b 00 00 00] [01] 95
		/// 
		/// </summary>
		public S_Mail(int mailId, int type)
		{
			// 刪除信件
			// 0x30: 刪除一般 0x31:刪除血盟 0x32:一般存到保管箱 0x40:刪除保管箱
			if ((type == 0x30) || (type == 0x31) || (type == 0x32) || (type == 0x40))
			{
				WriteC(Opcodes.S_OPCODE_MAIL);
				WriteC(type);
				WriteD(mailId);
				WriteC(1);
				return;
			}
			MailTable.Instance;
			L1Mail mail = MailTable.getMail(mailId);
			if (mail != null)
			{
				WriteC(Opcodes.S_OPCODE_MAIL);
				WriteC(type);
				WriteD(mail.Id);
				WriteByte(mail.Content);
			}
		}



		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = Bytes;
				}
				return _byte;
			}
		}

		public override string Type
		{
			get
			{
				return S_MAIL;
			}
		}
	}

}