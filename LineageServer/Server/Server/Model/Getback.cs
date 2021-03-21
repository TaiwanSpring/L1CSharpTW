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
namespace LineageServer.Server.Server.Model
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using Random = LineageServer.Server.Server.utils.Random;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	public class Getback
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(Getback).FullName);

		private static IDictionary<int, IList<Getback>> _getback = Maps.newMap();

		private int _areaX1;

		private int _areaY1;

		private int _areaX2;

		private int _areaY2;

		private int _areaMapId;

		private int _getbackX1;

		private int _getbackY1;

		private int _getbackX2;

		private int _getbackY2;

		private int _getbackX3;

		private int _getbackY3;

		private int _getbackMapId;

		private int _getbackTownId;

		private int _getbackTownIdForElf;

		private int _getbackTownIdForDarkelf;

		private Getback()
		{
		}

		private bool SpecifyArea
		{
			get
			{
				return ((_areaX1 != 0) && (_areaY1 != 0) && (_areaX2 != 0) && (_areaY2 != 0));
			}
		}

		public static void loadGetBack()
		{
			_getback.Clear();
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				// 同マップでエリア指定と無指定が混在していたら、エリア指定を先に読み込む為にarea_x1 DESC
				string sSQL = "SELECT * FROM getback ORDER BY area_mapid,area_x1 DESC ";
				pstm = con.prepareStatement(sSQL);
				rs = pstm.executeQuery();
				while (rs.next())
				{
					Getback getback = new Getback();
					getback._areaX1 = rs.getInt("area_x1");
					getback._areaY1 = rs.getInt("area_y1");
					getback._areaX2 = rs.getInt("area_x2");
					getback._areaY2 = rs.getInt("area_y2");
					getback._areaMapId = rs.getInt("area_mapid");
					getback._getbackX1 = rs.getInt("getback_x1");
					getback._getbackY1 = rs.getInt("getback_y1");
					getback._getbackX2 = rs.getInt("getback_x2");
					getback._getbackY2 = rs.getInt("getback_y2");
					getback._getbackX3 = rs.getInt("getback_x3");
					getback._getbackY3 = rs.getInt("getback_y3");
					getback._getbackMapId = rs.getInt("getback_mapid");
					getback._getbackTownId = rs.getInt("getback_townid");
					getback._getbackTownIdForElf = rs.getInt("getback_townid_elf");
					getback._getbackTownIdForDarkelf = rs.getInt("getback_townid_darkelf");
					rs.getBoolean("scrollescape");
					IList<Getback> getbackList = _getback[getback._areaMapId];
					if (getbackList == null)
					{
						getbackList = Lists.newList();
						_getback[getback._areaMapId] = getbackList;
					}
					getbackList.Add(getback);
				}
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, "could not Get Getback data", e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		/// <summary>
		/// pcの現在地から帰還ポイントを取得する。
		/// </summary>
		/// <param name="pc"> </param>
		/// <param name="bScroll_Escape">
		///            (未使用) </param>
		/// <returns> locx,locy,mapidの順に格納されている配列 </returns>
		public static int[] GetBack_Location(L1PcInstance pc, bool bScroll_Escape)
		{

			int[] loc = new int[3];

			int nPosition = RandomHelper.Next(3);

			int pcLocX = pc.X;
			int pcLocY = pc.Y;
			int pcMapId = pc.MapId;
			IList<Getback> getbackList = _getback[pcMapId];

			if (getbackList != null)
			{
				Getback getback = null;
				foreach (Getback gb in getbackList)
				{
					if (gb.SpecifyArea)
					{
						if ((gb._areaX1 <= pcLocX) && (pcLocX <= gb._areaX2) && (gb._areaY1 <= pcLocY) && (pcLocY <= gb._areaY2))
						{
							getback = gb;
							break;
						}
					}
					else
					{
						getback = gb;
						break;
					}
				}

				loc = ReadGetbackInfo(getback, nPosition);

				// town_idが指定されている場合はそこへ帰還させる
				if (pc.Elf && (getback._getbackTownIdForElf > 0))
				{
					loc = L1TownLocation.getGetBackLoc(getback._getbackTownIdForElf);
				}
				else if (pc.Darkelf && (getback._getbackTownIdForDarkelf > 0))
				{
					loc = L1TownLocation.getGetBackLoc(getback._getbackTownIdForDarkelf);
				}
				else if (getback._getbackTownId > 0)
				{
					loc = L1TownLocation.getGetBackLoc(getback._getbackTownId);
				}
			}
			// getbackテーブルにデータがない場合、SKTに帰還
			else
			{
				loc[0] = 33089;
				loc[1] = 33397;
				loc[2] = 4;
			}
			return loc;
		}

		private static int[] ReadGetbackInfo(Getback getback, int nPosition)
		{
			int[] loc = new int[3];
			switch (nPosition)
			{
				case 0:
					loc[0] = getback._getbackX1;
					loc[1] = getback._getbackY1;
					break;

				case 1:
					loc[0] = getback._getbackX2;
					loc[1] = getback._getbackY2;
					break;

				case 2:
					loc[0] = getback._getbackX3;
					loc[1] = getback._getbackY3;
					break;
			}
			loc[2] = getback._getbackMapId;

			return loc;
		}
	}

}