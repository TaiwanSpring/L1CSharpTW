using LineageServer.Server.Model.Gametime;
using System;

namespace LineageServer.Server.Templates
{
    public class L1SpawnTime
    {
        private readonly int _spawnId;

        private readonly TimeSpan _timeStart;

        private readonly TimeSpan _timeEnd;

        private readonly TimeSpan _timePeriod;

        private readonly TimeSpan _periodStart;

        private readonly TimeSpan _periodEnd;

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
            return l1GameTime.Calendar >= DateTime.Today.Add(_timeStart) && l1GameTime.Calendar <= DateTime.Today.Add(_timeEnd);
        }
        public bool includes(DateTime dateTime)
        {
            return dateTime >= DateTime.Today.Add(_timeStart) && dateTime <= DateTime.Today.Add(_timeEnd);
        }
        public virtual int SpawnId
        {
            get
            {
                return _spawnId;
            }
        }

        public virtual TimeSpan TimeStart
        {
            get
            {
                return _timeStart;
            }
        }

        public virtual TimeSpan TimeEnd
        {
            get
            {
                return _timeEnd;
            }
        }

        public virtual TimeSpan PeriodStart
        {
            get
            {
                return _periodStart;
            }
        }

        public virtual TimeSpan PeriodEnd
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

        public TimeSpan TimeStart { get; set; }

        public TimeSpan TimeEnd { get; set; }

        public TimeSpan PeriodStart { get; set; }

        public TimeSpan PeriodEnd { get; set; }

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