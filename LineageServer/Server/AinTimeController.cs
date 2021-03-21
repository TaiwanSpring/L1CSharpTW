using System;
using System.Threading;

/*
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful ,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not , write to the Free Software
 * Foundation , Inc., 59 Temple Place - Suite 330, Boston , MA
 * 02111-1307, USA.
 *
 * http://www.gnu.org/copyleft/gpl.html
 */
/* 殷海薩的祝福 時間控制 */
namespace LineageServer.Server
{

	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_SkillIconExp = LineageServer.Server.Server.serverpackets.S_SkillIconExp;

	public class AinTimeController : IRunnableStart
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(AinTimeController).FullName);

		private static AinTimeController _instance;

		public static AinTimeController Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new AinTimeController();
				}
				return _instance;
			}
		}

		public override void run()
		{
			try
			{
				while (true)
				{
					checkAinTime();
					Thread.Sleep(60000);
				}
			}
			catch (Exception e1)
			{
				_log.warning(e1.Message);
			}
		}

		private DateTime RealTime
		{
			get
			{
				TimeZone _tz = TimeZone.getTimeZone(Config.TIME_ZONE);
				DateTime cal = DateTime.getInstance(_tz);
				return cal;
			}
		}

		private void checkAinTime()
		{
			SimpleDateFormat tempTime = new SimpleDateFormat("HHmm");
			int nowTime = Convert.ToInt32(tempTime.format(RealTime));

			int ainTime = Config.RATE_AIN_TIME; // 時間比例
			int ainMaxPercent = Config.RATE_MAX_CHARGE_PERCENT; // 殷海薩的祝福 百分比
			if (nowTime % ainTime == 0)
			{
				foreach (L1PcInstance pc in L1World.Instance.AllPlayers)
				{
					if (pc.Level >= 49)
					{ // 等級限制
						if (pc.AinPoint < ainMaxPercent && pc.Map.isSafetyZone(pc.Location))
						{ // 限制最高點數
							pc.AinPoint = pc.AinPoint + 1; // 安全區域點數 +1
							pc.AinZone = 1;
							pc.sendPackets(new S_SkillIconExp(pc.AinPoint));
						}
						else
						{
							pc.AinZone = 0;
						}
					}
				}
			}
			else
			{
				return;
			}
		}
	}

}