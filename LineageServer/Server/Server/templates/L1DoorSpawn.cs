﻿using System.Collections.Generic;

namespace LineageServer.Server.Server.Templates
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1Location = LineageServer.Server.Server.Model.L1Location;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;

	public class L1DoorSpawn
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1DoorSpawn).FullName);
		private readonly int _id;
		private readonly L1DoorGfx _gfx;
		private readonly int _x;
		private readonly int _y;
		private readonly int _mapId;
		private readonly L1Location _loc;
		private readonly int _hp;
		private readonly int _keeper;
		private readonly bool _isOpening;

		public L1DoorSpawn(int id, L1DoorGfx gfx, int x, int y, int mapId, int hp, int keeper, bool isOpening) : base()
		{
			_id = id;
			_gfx = gfx;
			_x = x;
			_y = y;
			_mapId = mapId;
			_loc = new L1Location(_x, _y, _mapId);
			_hp = hp;
			_keeper = keeper;
			_isOpening = isOpening;
		}

		public virtual int Id
		{
			get
			{
				return _id;
			}
		}

		public virtual L1DoorGfx Gfx
		{
			get
			{
				return _gfx;
			}
		}

		public virtual int X
		{
			get
			{
				return _x;
			}
		}

		public virtual int Y
		{
			get
			{
				return _y;
			}
		}

		public virtual int MapId
		{
			get
			{
				return _mapId;
			}
		}

		public virtual L1Location Location
		{
			get
			{
				return _loc;
			}
		}

		public virtual int Hp
		{
			get
			{
				return _hp;
			}
		}

		public virtual int Keeper
		{
			get
			{
				return _keeper;
			}
		}

		public virtual bool Opening
		{
			get
			{
				return _isOpening;
			}
		}

		public static IList<L1DoorSpawn> all()
		{
			IList<L1DoorSpawn> result = Lists.newArrayList();
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM spawnlist_door");
				rs = pstm.executeQuery();
				while (rs.next())
				{
					int id = rs.getInt("id");
					int gfxId = rs.getInt("gfxid");
					int x = rs.getInt("locx");
					int y = rs.getInt("locy");
					int mapId = rs.getInt("mapid");
					int hp = rs.getInt("hp");
					int keeper = rs.getInt("keeper");
					bool isOpening = rs.getBoolean("isOpening");
					L1DoorGfx gfx = L1DoorGfx.findByGfxId(gfxId);
					L1DoorSpawn spawn = new L1DoorSpawn(id, gfx, x, y, mapId, hp, keeper, isOpening);
					result.Add(spawn);
				}

			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
			return result;
		}
	}

}