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

		public L1SpawnTime(L1SpawnTimeBuilder builder)
		{
			_spawnId = builder.SpawnId;
			_timeStart = builder.TimeStart;
			_timeEnd = builder.TimeEnd;
			_timePeriod = _timeEnd - _timeStart;
			_periodStart = builder.PeriodStart;
			_periodEnd = builder.PeriodEnd;
			_isDeleteAtEndTime = builder.IsDeleteAtEndTime;
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
		public virtual TimeSpan TimePeriod
		{
			get
			{
				return _timePeriod;
			}
		}
	}
	public class L1SpawnTimeBuilder
	{
		public int SpawnId { get; set; }

		public DateTime TimeStart { get; set; }

		public DateTime TimeEnd { get; set; }

		public DateTime PeriodStart { get; set; }

		public DateTime PeriodEnd { get; set; }

		public bool IsDeleteAtEndTime { get; set; }

		public L1SpawnTimeBuilder(int spawnId)
		{
			SpawnId = spawnId;

		}
		public virtual L1SpawnTime build()
		{
			return new L1SpawnTime(this);
		}
	}

}