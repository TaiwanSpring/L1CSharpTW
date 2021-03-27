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
namespace LineageServer.Server.Templates
{
	public class L1NpcChat
	{
		public L1NpcChat()
		{
		}

		private int _npcId;

		public virtual int NpcId
		{
			get
			{
				return _npcId;
			}
			set
			{
				_npcId = value;
			}
		}


		private int _chatTiming;

		public virtual int ChatTiming
		{
			get
			{
				return _chatTiming;
			}
			set
			{
				_chatTiming = value;
			}
		}


		private int _startDelayTime;

		public virtual int StartDelayTime
		{
			get
			{
				return _startDelayTime;
			}
			set
			{
				_startDelayTime = value;
			}
		}


		private string _chatId1;

		public virtual string ChatId1
		{
			get
			{
				return _chatId1;
			}
			set
			{
				_chatId1 = value;
			}
		}


		private string _chatId2;

		public virtual string ChatId2
		{
			get
			{
				return _chatId2;
			}
			set
			{
				_chatId2 = value;
			}
		}


		private string _chatId3;

		public virtual string ChatId3
		{
			get
			{
				return _chatId3;
			}
			set
			{
				_chatId3 = value;
			}
		}


		private string _chatId4;

		public virtual string ChatId4
		{
			get
			{
				return _chatId4;
			}
			set
			{
				_chatId4 = value;
			}
		}


		private string _chatId5;

		public virtual string ChatId5
		{
			get
			{
				return _chatId5;
			}
			set
			{
				_chatId5 = value;
			}
		}


		private int _chatInterval;

		public virtual int ChatInterval
		{
			get
			{
				return _chatInterval;
			}
			set
			{
				_chatInterval = value;
			}
		}


		private bool _isShout;

		public virtual bool Shout
		{
			get
			{
				return _isShout;
			}
			set
			{
				_isShout = value;
			}
		}


		private bool _isWorldChat;

		public virtual bool WorldChat
		{
			get
			{
				return _isWorldChat;
			}
			set
			{
				_isWorldChat = value;
			}
		}


		private bool _isRepeat;

		public virtual bool Repeat
		{
			get
			{
				return _isRepeat;
			}
			set
			{
				_isRepeat = value;
			}
		}


		private int _repeatInterval;

		public virtual int RepeatInterval
		{
			get
			{
				return _repeatInterval;
			}
			set
			{
				_repeatInterval = value;
			}
		}


		private int _gameTime;

		public virtual int GameTime
		{
			get
			{
				return _gameTime;
			}
			set
			{
				_gameTime = value;
			}
		}


	}

}