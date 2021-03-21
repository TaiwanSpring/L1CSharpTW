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

	using TimePeriod = LineageServer.Server.Server.utils.TimePeriod;

	public class L1SpawnTime
	{
		private readonly int _spawnId;

		private readonly Time _timeStart;

		private readonly Time _timeEnd;

		private readonly TimePeriod _timePeriod;

		private readonly Timestamp _periodStart;

		private readonly Timestamp _periodEnd;

		private bool _isDeleteAtEndTime;

		public virtual bool DeleteAtEndTime
		{
			get
			{
				return _isDeleteAtEndTime;
			}
			set
			{
					_isDeleteAtEndTime = value;
			}
		}

		private L1SpawnTime(L1SpawnTimeBuilder builder)
		{
			_spawnId = builder._spawnId;
			_timeStart = builder._timeStart;
			_timeEnd = builder._timeEnd;
			_timePeriod = new TimePeriod(_timeStart, _timeEnd);
			_periodStart = builder._periodStart;
			_periodEnd = builder._periodEnd;
			_isDeleteAtEndTime = builder._isDeleteAtEndTime;
		}

		public virtual int SpawnId
		{
			get
			{
				return _spawnId;
			}
		}

		public virtual Time TimeStart
		{
			get
			{
				return _timeStart;
			}
			set
			{
					_timeStart = value;
			}
		}

		public virtual Time TimeEnd
		{
			get
			{
				return _timeEnd;
			}
			set
			{
					_timeEnd = value;
			}
		}

		public virtual Timestamp PeriodStart
		{
			get
			{
				return _periodStart;
			}
			set
			{
					_periodStart = value;
			}
		}

		public virtual Timestamp PeriodEnd
		{
			get
			{
				return _periodEnd;
			}
			set
			{
					_periodEnd = value;
			}
		}

		public class L1SpawnTimeBuilder
		{
			internal readonly int _spawnId;

			internal Time _timeStart;

			internal Time _timeEnd;

			internal Timestamp _periodStart;

			internal Timestamp _periodEnd;

			internal bool _isDeleteAtEndTime;

			public L1SpawnTimeBuilder(int spawnId)
			{
				_spawnId = spawnId;
			}

			public virtual L1SpawnTime build()
			{
				return new L1SpawnTime(this);
			}






		}

		public virtual TimePeriod TimePeriod
		{
			get
			{
				return _timePeriod;
			}
		}
	}

}