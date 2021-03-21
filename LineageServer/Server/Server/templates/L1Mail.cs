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
namespace LineageServer.Server.Server.Templates
{

	public class L1Mail
	{
		public L1Mail()
		{
		}

		private int _id;

		public virtual int Id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}


		private int _type;

		public virtual int Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}


		private string _senderName;

		public virtual string SenderName
		{
			get
			{
				return _senderName;
			}
			set
			{
				_senderName = value;
			}
		}


		private string _receiverName;

		public virtual string ReceiverName
		{
			get
			{
				return _receiverName;
			}
			set
			{
				_receiverName = value;
			}
		}


		private Timestamp _date = null;

		public virtual Timestamp Date
		{
			get
			{
				return _date;
			}
			set
			{
				_date = value;
			}
		}


		private int _readStatus = 0;

		public virtual int ReadStatus
		{
			get
			{
				return _readStatus;
			}
			set
			{
				_readStatus = value;
			}
		}


		private sbyte[] _subject = null;

		public virtual sbyte[] Subject
		{
			get
			{
				return _subject;
			}
			set
			{
				_subject = value;
			}
		}


		private sbyte[] _content = null;

		public virtual sbyte[] Content
		{
			get
			{
				return _content;
			}
			set
			{
				_content = value;
			}
		}


		private int _inBoxId = 0;

		public virtual int InBoxId
		{
			get
			{
				return _inBoxId;
			}
			set
			{
				_inBoxId = value;
			}
		}


	}

}