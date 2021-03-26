using LineageServer.Server.Server.Model.Gametime;
using System;

namespace LineageServer.Server.Server.Utils
{
    public class TimePeriod
    {
        private readonly DateTime start;

        private readonly DateTime end;

        public TimePeriod(DateTime timeStart, DateTime timeEnd)
        {
            this.start = timeStart;
            this.end = timeEnd;
        }

        private bool Includes(L1GameTime time, DateTime timeStart, DateTime timeEnd)
        {

            return Includes(time.Calendar, timeStart, timeEnd);
        }
        private bool Includes(DateTime time, DateTime timeStart, DateTime timeEnd)
        {
            return time >= timeStart && time <= timeEnd;
        }
        public virtual bool Includes(L1GameTime time)
        {
            /*
			 * 分かりづらいロジック・・・ timeStart after timeEndのとき(例:18:00~06:00)
			 * timeEnd~timeStart(06:00~18:00)の範囲内でなければ、
			 * timeStart~timeEnd(18:00~06:00)の範囲内と見なせる
			 */
            return start > end ? !Includes(time, end, start) : Includes(time, start, end);
        }
    }

}