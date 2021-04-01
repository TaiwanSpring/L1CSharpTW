using LineageServer.Server.Model.Instance;
using LineageServer.Utils;
using System;
using System.Collections.Generic;

namespace LineageServer.Server.Model
{
	class L1Clan
	{
		private bool InstanceFieldsInitialized = false;

		public L1Clan()
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

		private void InitializeInstanceFields()
		{
			_dwarfForClan = new L1DwarfForClanInventory(this);
		}

		/// <summary>
		/// 聯盟一般 </summary>
		public const int CLAN_RANK_LEAGUE_PUBLIC = 2;
		/// <summary>
		/// 聯盟 副君主 </summary>
		public const int CLAN_RANK_LEAGUE_VICEPRINCE = 3;
		/// <summary>
		/// 聯盟君主 </summary>
		public const int CLAN_RANK_LEAGUE_PRINCE = 4;
		/// <summary>
		/// 聯盟見習 </summary>
		public const int CLAN_RANK_LEAGUE_PROBATION = 5;
		/// <summary>
		/// 聯盟守護騎士 </summary>
		public const int CLAN_RANK_LEAGUE_GUARDIAN = 6;
		/// <summary>
		/// 一般 </summary>
		public const int CLAN_RANK_PUBLIC = 7;
		/// <summary>
		/// 見習 </summary>
		public const int CLAN_RANK_PROBATION = 8;
		/// <summary>
		/// 守護騎士 </summary>
		public const int CLAN_RANK_GUARDIAN = 9;
		/// <summary>
		/// 君主 </summary>
		public const int CLAN_RANK_PRINCE = 10;

		private int _clanId;

		private string _clanName;

		private DateTime _foundDate;

		private string _announcement;

		private int _leaderId;

		private string _leaderName;

		private int _castleId;

		private int _houseId;

		private int _warehouse = 0;

		private int _emblemId = 0;

		private int _emblemStatus = 0;

		private L1DwarfForClanInventory _dwarfForClan;

		private readonly IList<string> membersNameList = ListFactory.NewList<string>();

		public virtual int ClanId
		{
			get
			{
				return _clanId;
			}
			set
			{
				_clanId = value;
			}
		}


		public virtual string ClanName
		{
			get
			{
				return _clanName;
			}
			set
			{
				_clanName = value;
			}
		}


		public virtual DateTime FoundDate
		{
			get
			{
				return _foundDate;
			}
			set
			{
				this._foundDate = value;
			}
		}


		public virtual string Announcement
		{
			get
			{
				return _announcement;
			}
			set
			{
				this._announcement = value;
			}
		}


		public virtual int EmblemId
		{
			get
			{
				return _emblemId;
			}
			set
			{
				this._emblemId = value;
			}
		}


		public virtual int EmblemStatus
		{
			get
			{
				return _emblemStatus;
			}
			set
			{
				this._emblemStatus = value;
			}
		}


		public virtual int LeaderId
		{
			get
			{
				return _leaderId;
			}
			set
			{
				_leaderId = value;
			}
		}


		public virtual string LeaderName
		{
			get
			{
				return _leaderName;
			}
			set
			{
				_leaderName = value;
			}
		}


		public virtual int CastleId
		{
			get
			{
				return _castleId;
			}
			set
			{
				_castleId = value;
			}
		}


		public virtual int HouseId
		{
			get
			{
				return _houseId;
			}
			set
			{
				_houseId = value;
			}
		}


		public virtual void addMemberName(string member_name)
		{
			if (!membersNameList.Contains(member_name))
			{
				membersNameList.Add(member_name);
			}
		}

		public virtual void delMemberName(string member_name)
		{
			if (membersNameList.Contains(member_name))
			{
				membersNameList.Remove(member_name);
			}
		}

		public virtual L1PcInstance[] OnlineClanMember
		{
			get
			{
				IList<L1PcInstance> onlineMembers = ListFactory.NewList<L1PcInstance>();
				foreach (string name in membersNameList)
				{
					L1PcInstance pc = Container.Instance.Resolve<IGameWorld>().getPlayer(name);
					if (( pc != null ) && !onlineMembers.Contains(pc))
					{
						onlineMembers.Add(pc);
					}
				}
				return ( (List<L1PcInstance>)onlineMembers ).ToArray();
			}
		}

		public virtual string OnlineMembersFP
		{
			get
			{ // FP means "For Pledge"
				string result = "";
				foreach (string name in membersNameList)
				{
					L1PcInstance pc = Container.Instance.Resolve<IGameWorld>().getPlayer(name);
					if (pc != null)
					{
						result = result + name + " ";
					}
				}
				return result;
			}
		}

		public virtual string AllMembersFP
		{
			get
			{
				string result = "";
				foreach (string name in membersNameList)
				{
					result = result + name + " ";
				}
				return result;
			}
		}

		public virtual string[] AllMembers
		{
			get
			{
				return ( (List<string>)membersNameList ).ToArray();
			}
		}

		public virtual L1DwarfForClanInventory DwarfForClanInventory
		{
			get
			{
				return _dwarfForClan;
			}
		}

		public virtual int WarehouseUsingChar
		{
			get
			{
				return _warehouse;
			}
			set
			{
				_warehouse = value;
			}
		}

	}

}