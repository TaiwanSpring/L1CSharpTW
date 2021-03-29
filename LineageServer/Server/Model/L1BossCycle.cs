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
namespace LineageServer.Server.Model
{


	using BossSpawnTable = LineageServer.Server.DataTables.BossSpawnTable;
	using PerformanceTimer = LineageServer.Utils.PerformanceTimer;
	using Random = LineageServer.Utils.Random;
	using MapFactory = LineageServer.Utils.MapFactory;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAccessorType(XmlAccessType.FIELD) public class L1BossCycle
	public class L1BossCycle
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAttribute(name = "Name") private String _name;
		private string _name;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlElement(name = "Base") private Base _base;
		private Base _base;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAccessorType(XmlAccessType.FIELD) private static class Base
		private class Base
		{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAttribute(name = "Date") private String _date;
			internal string _date;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAttribute(name = "Time") private String _time;
			internal string _time;

			public virtual string Date
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


			public virtual string Time
			{
				get
				{
					return _time;
				}
				set
				{
					_time = value;
				}
			}

		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlElement(name = "Cycle") private Cycle _cycle;
		private Cycle _cycle;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAccessorType(XmlAccessType.FIELD) private static class Cycle
		private class Cycle
		{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAttribute(name = "Period") private String _period;
			internal string _period;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAttribute(name = "Start") private String _start;
			internal string _start;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAttribute(name = "End") private String _end;
			internal string _end;

			public virtual string Period
			{
				get
				{
					return _period;
				}
			}

			public virtual string Start
			{
				get
				{
					return _start;
				}
			}

			public virtual string End
			{
				get
				{
					return _end;
				}
			}
		}

		private DateTime _baseDate;

		private int _period; // 分換算

		private int _periodDay;

		private int _periodHour;

		private int _periodMinute;

		private int _startTime; // 分換算

		private int _endTime; // 分換算

		private static SimpleDateFormat _sdfYmd = new SimpleDateFormat("yyyy/MM/dd");

		private static SimpleDateFormat _sdfTime = new SimpleDateFormat("HH:mm");

		private static SimpleDateFormat _sdf = new SimpleDateFormat("yyyy/MM/dd HH:mm");

		private static DateTime _initDate = DateTime.Now;

		private static string _initTime = "0:00";

		private static readonly DateTime START_UP = new DateTime();

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public void init() throws Exception
		public virtual void init()
		{
			// 基準日時の設定
			Base @base = Base;
			// 基準がなければ、現在日付の0:00基準
			if (@base == null)
			{
				Base = new Base();
				Base.Date = _sdfYmd.format(_initDate);
				Base.Time = _initTime;
				@base = Base;
			}
			else
			{
				try
				{
					_sdfYmd.parse(@base.Date);
				}
				catch (Exception)
				{
					@base.Date = _sdfYmd.format(_initDate);
				}
				try
				{
					_sdfTime.parse(@base.Time);
				}
				catch (Exception)
				{
					@base.Time = _initTime;
				}
			}
			// 基準日時を決定
			DateTime baseCal = new DateTime();
			baseCal = new DateTime(_sdf.parse(@base.Date + " " + @base.Time));

			// 出現周期の初期化,チェック
			Cycle spawn = Cycle;
			if ((spawn == null) || (string.ReferenceEquals(spawn.Period, null)))
			{
				throw new Exception("CycleのPeriodは必須");
			}

			string period = spawn.Period;
			_periodDay = getTimeParse(period, "d");
			_periodHour = getTimeParse(period, "h");
			_periodMinute = getTimeParse(period, "m");

			string start = spawn.Start;
			int sDay = getTimeParse(start, "d");
			int sHour = getTimeParse(start, "h");
			int sMinute = getTimeParse(start, "m");
			string end = spawn.End;
			int eDay = getTimeParse(end, "d");
			int eHour = getTimeParse(end, "h");
			int eMinute = getTimeParse(end, "m");

			// 分換算
			_period = (_periodDay * 24 * 60) + (_periodHour * 60) + _periodMinute;
			_startTime = (sDay * 24 * 60) + (sHour * 60) + sMinute;
			_endTime = (eDay * 24 * 60) + (eHour * 60) + eMinute;
			if (_period <= 0)
			{
				throw new Exception("must be Period > 0");
			}
			// start補正
			if ((_startTime < 0) || (_period < _startTime))
			{ // 補正
				_startTime = 0;
			}
			// end補正
			if ((_endTime < 0) || (_period < _endTime) || (string.ReferenceEquals(end, null)))
			{ // 補正
				_endTime = _period;
			}
			if (_startTime > _endTime)
			{
				_startTime = _endTime;
			}
			// start,endの相関補正(最低でも1分の間をあける)
			// start==endという指定でも、出現時間が次の周期に被らないようにするため
			if (_startTime == _endTime)
			{
				if (_endTime == _period)
				{
					_startTime--;
				}
				else
				{
					_endTime++;
				}
			}

			// 最近の周期まで補正(再計算するときに厳密に算出するので、ここでは近くまで適当に補正するだけ)
			while (!(baseCal > START_UP))
			{
				baseCal.AddDays(_periodDay);
				baseCal.AddHours(_periodHour);
				baseCal.AddMinutes(_periodMinute);
			}
			_baseDate = baseCal;
		}

		/*
		 * 指定日時を含む周期(の開始時間)を返す ex.周期が2時間の場合 target base 戻り値 4:59 7:00 3:00 5:00 7:00
		 * 5:00 5:01 7:00 5:00 6:00 7:00 5:00 6:59 7:00 5:00 7:00 7:00 7:00 7:01
		 * 7:00 7:00 9:00 7:00 9:00 9:01 7:00 9:00
		 */
		private DateTime getBaseCycleOnTarget(DateTime target)
		{
			// 基準日時取得
			DateTime @base = (DateTime) _baseDate.clone();
			if (target > @base)
			{
				// target <= baseとなるまで繰り返す
				while (target > @base)
				{
					@base.AddDays(_periodDay);
					@base.AddHours(_periodHour);
					@base.AddMinutes(_periodMinute);
				}
			}
			if (target < @base)
			{
				while (target < @base)
				{
					@base.AddDays(-_periodDay);
					@base.AddHours(-_periodHour);
					@base.AddMinutes(-_periodMinute);
				}
			}
			// 終了時間を算出してみて、過去の時刻ならボス時間が過ぎている→次の周期を返す。
			DateTime end = (DateTime) @base.clone();
			end.AddMinutes(_endTime);
			if (end < target)
			{
				@base.AddDays(_periodDay);
				@base.AddHours(_periodHour);
				@base.AddMinutes(_periodMinute);
			}
			return @base;
		}

		/// <summary>
		/// 指定日時を含む周期に対して、出現タイミングを算出する。
		/// </summary>
		/// <returns> 出現する時間 </returns>
		public virtual DateTime calcSpawnTime(DateTime now)
		{
			// 基準日時取得
			DateTime @base = getBaseCycleOnTarget(now);
			// 出現期間の計算
			@base.AddMinutes(_startTime);
			// 出現時間の決定 start～end迄の間でランダムの秒
			int diff = (_endTime - _startTime) * 60;
			int random = diff > 0 ? RandomHelper.Next(diff) : 0;
			@base.AddSeconds(random);
			return @base;
		}

		/// <summary>
		/// 指定日時を含む周期に対して、出現開始時間を算出する。
		/// </summary>
		/// <returns> 周期の出現開始時間 </returns>
		public virtual DateTime getSpawnStartTime(DateTime now)
		{
			// 基準日時取得
			DateTime startDate = getBaseCycleOnTarget(now);
			// 出現期間の計算
			startDate.AddMinutes(_startTime);
			return startDate;
		}

		/// <summary>
		/// 指定日時を含む周期に対して、出現終了時間を算出する。
		/// </summary>
		/// <returns> 周期の出現終了時間 </returns>
		public virtual DateTime getSpawnEndTime(DateTime now)
		{
			// 基準日時取得
			DateTime endDate = getBaseCycleOnTarget(now);
			// 出現期間の計算
			endDate.AddMinutes(_endTime);
			return endDate;
		}

		/// <summary>
		/// 指定日時を含む周期に対して、次の周期の出現タイミングを算出する。
		/// </summary>
		/// <returns> 次の周期の出現する時間 </returns>
		public virtual DateTime nextSpawnTime(DateTime now)
		{
			// 基準日時取得
			DateTime next = (DateTime) now.clone();
			next.AddDays(_periodDay);
			next.AddHours(_periodHour);
			next.AddMinutes(_periodMinute);
			return calcSpawnTime(next);
		}

		/// <summary>
		/// 指定日時に対して、最近の出現開始時間を返却する。
		/// </summary>
		/// <returns> 最近の出現開始時間 </returns>
		public virtual DateTime getLatestStartTime(DateTime now)
		{
			// 基準日時取得
			DateTime latestStart = getSpawnStartTime(now);
			if (!now < latestStart)
			{ // now >= latestStart
			}
			else
			{
				// now < latestStartなら1個前が最近の周期
				latestStart.AddDays(-_periodDay);
				latestStart.AddHours(-_periodHour);
				latestStart.AddMinutes(-_periodMinute);
			}

			return latestStart;
		}

		private static int getTimeParse(string target, string search)
		{
			if (string.ReferenceEquals(target, null))
			{
				return 0;
			}
			int n = 0;
			Matcher matcher = Pattern.compile("\\d+" + search).matcher(target);
			if (matcher.find())
			{
				string match = matcher.group();
				n = int.Parse(match.Replace(search, ""));
			}
			return n;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlAccessorType(XmlAccessType.FIELD) @XmlRootElement(name = "BossCycleList") static class L1BossCycleList
		internal class L1BossCycleList
		{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @XmlElement(name = "BossCycle") private java.util.List<L1BossCycle> bossCycles;
			internal IList<L1BossCycle> bossCycles;

			public virtual IList<L1BossCycle> BossCycles
			{
				get
				{
					return bossCycles;
				}
				set
				{
					this.bossCycles = value;
				}
			}

		}

		public static void load()
		{
			PerformanceTimer timer = new PerformanceTimer();
            System.Console.Write("【讀取】 【bosscycle】【設定】");
			try
			{
				// BookOrder クラスをバインディングするコンテキストを生成
				JAXBContext context = JAXBContext.newInstance(typeof(L1BossCycle.L1BossCycleList));
				// XML -> POJO 変換を行うアンマーシャラを生成
				Unmarshaller um = context.createUnmarshaller();

				// XML -> POJO 変換
				File file = new File("./data/xml/Cycle/BossCycle.xml");
				L1BossCycleList bossList = (L1BossCycleList) um.unmarshal(file);

				foreach (L1BossCycle cycle in bossList.BossCycles)
				{
					cycle.init();
					_cycleMap[cycle.Name] = cycle;
				}

				// userデータがあれば上書き
				File userFile = new File("./data/xml/Cycle/users/BossCycle.xml");
				if (userFile.exists())
				{
					bossList = (L1BossCycleList) um.unmarshal(userFile);

					foreach (L1BossCycle cycle in bossList.BossCycles)
					{
						cycle.init();
						_cycleMap[cycle.Name] = cycle;
					}
				}
				// spawnlist_bossから読み込んで配置
				BossSpawnTable.fillSpawnTable();
			}
			catch (Exception e)
			{
				_log.Error(Enum.Level.Server, "找不到BossCycle.xml的檔案位置。", e);
				Environment.Exit(0);
			}
            System.Console.WriteLine("【完成】【" + timer.get() + "】【毫秒】。");
		}

		/// <summary>
		/// 周期名と指定日時に対する出現期間、出現時間をコンソール出力
		/// </summary>
		/// <param name="now">
		///            周期を出力する日時 </param>
		public virtual void showData(DateTime now)
		{
            System.Console.WriteLine("[Type]" + Name);
            System.Console.Write("  [出現期間]");
            System.Console.Write(_sdf.format(getSpawnStartTime(now)) + " - ");
            System.Console.WriteLine(_sdf.format(getSpawnEndTime(now)));
		}

		private static IDictionary<string, L1BossCycle> _cycleMap = MapFactory.newMap();

		public static L1BossCycle getBossCycle(string type)
		{
			return _cycleMap[type];
		}

		public virtual string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}


		public virtual Base Base
		{
			get
			{
				return _base;
			}
			set
			{
				_base = value;
			}
		}


		public virtual Cycle Cycle
		{
			get
			{
				return _cycle;
			}
			set
			{
				_cycle = value;
			}
		}


//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(L1BossCycle).FullName);
	}

}