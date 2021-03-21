using System;
namespace LineageServer.Server.Server.Templates
{

	/// <summary>
	/// 旅館 </summary>
	public class L1Inn
	{

		private int _npcId;
		private int _roomNumber;
		private int _keyId;
		private int _lodgerId;
		private bool _hall;
		private DateTime _dueTime;

		public virtual int KeyId
		{
			get
			{
				return _keyId;
			}
			set
			{
				_keyId = value;
			}
		}


		public virtual int InnNpcId
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


		public virtual int RoomNumber
		{
			get
			{
				return _roomNumber;
			}
			set
			{
				_roomNumber = value;
			}
		}


		public virtual int LodgerId
		{
			get
			{
				return _lodgerId;
			}
			set
			{
				_lodgerId = value;
			}
		}


		public virtual bool Hall
		{
			get
			{
				return _hall;
			}
			set
			{
				_hall = value;
			}
		}


		public virtual DateTime DueTime
		{
			get
			{
				return _dueTime;
			}
			set
			{
				_dueTime = value;
			}
		}


	}

}