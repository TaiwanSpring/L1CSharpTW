using LineageServer.Server.Model.Gametime;
using System;

namespace LineageServer.Server.Templates
{
	public class L1SpawnTime
	{
		private readonly int _spawnId;

		private readonly DateTime _timeStart;

		private readonly DateTime _timeEnd;

		private readonly TimeSpan _timePeriod;

		private readonly DateTime _periodStart;

		private readonly DateTime _periodEnd;

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
			_timePeriod = _timeEnd - _timeStart;
			_periodStart = builder._periodStart;
			_periodEnd = builder._periodEnd;
			_isDeleteAtEndTime = builder._isDeleteAtEndTime;
		}
		public bool includes(L1GameTime l1GameTime)
		{
			return l1GameTime.Calendar >= _timeStart && l1GameTime.Calendar <= _timeEnd;
		}
		public bool includes(DateTime dateTime)
		{
			return dateTime >= _timeStart && dateTime <= _timeEnd;
		}
		public virtual int SpawnId
		{
			get
			{
				return _spawnId;
			}
		}

		public virtual DateTime TimeStart
		{
			get
			{
				return _timeStart;
			}
		}

		public virtual DateTime TimeEnd
		{
			get
			{
				return _timeEnd;
			}
		}

		public virtual DateTime PeriodStart
		{
			get
			{
				return _periodStart;
			}
		}

		public virtual DateTime PeriodEnd
		{
			get
			{
				return _periodEnd;
			}
		}

		public class L1SpawnTimeBuilder
		{
			internal readonly int _spawnId;

			internal DateTime _timeStart;

			internal DateTime _timeEnd;

			internal DateTime _periodStart;

			internal DateTime _periodEnd;

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

		public virtual TimeSpan TimePeriod
		{
			get
			{
				return _timePeriod;
			}
		}
	}

}