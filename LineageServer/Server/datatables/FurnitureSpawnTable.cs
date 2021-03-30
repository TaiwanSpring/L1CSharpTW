using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using System;
using System.Collections.Generic;

namespace LineageServer.Server.DataTables
{
	class FurnitureSpawnTable
	{
		private readonly static IDataSource dataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.SpawnlistFurniture);

		private static FurnitureSpawnTable _instance;

		public static FurnitureSpawnTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new FurnitureSpawnTable();
				}
				return _instance;
			}
		}

		private FurnitureSpawnTable()
		{
			FillFurnitureSpawnTable();
		}

		private void FillFurnitureSpawnTable()
		{
			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];
				L1Npc l1npc = NpcTable.Instance.getTemplate(dataSourceRow.getInt(SpawnlistFurniture.Column_npcid));
				if (l1npc != null)
				{
					string s = l1npc.Impl;

					if (L1NpcInstance.Factory(l1npc) is L1FurnitureInstance furniture)
					{
						furniture.Id = IdFactory.Instance.nextId();
						furniture.ItemObjId = dataSourceRow.getInt(SpawnlistFurniture.Column_item_obj_id);
						furniture.X = dataSourceRow.getInt(SpawnlistFurniture.Column_locx);
						furniture.Y = dataSourceRow.getInt(SpawnlistFurniture.Column_locy);
						furniture.MapId = (short)dataSourceRow.getInt(SpawnlistFurniture.Column_mapid);
						furniture.HomeX = furniture.X;
						furniture.HomeY = furniture.Y;
						furniture.Heading = 0;
						L1World.Instance.storeObject(furniture);
						L1World.Instance.addVisibleObject(furniture);
					}
				}
			}
			try
			{

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM spawnlist_furniture");
				rs = pstm.executeQuery();
				do
				{
					if (!rs.next())
					{
						break;
					}

					L1Npc l1npc = NpcTable.Instance.getTemplate(dataSourceRow.getInt(2));
					if (l1npc != null)
					{
						string s = l1npc.Impl;

						if (L1NpcInstance.Factory(l1npc) is L1FurnitureInstance furniture)
						{
							furniture.Id = IdFactory.Instance.nextId();
							furniture.ItemObjId = dataSourceRow.getInt(1);
							furniture.X = dataSourceRow.getInt(3);
							furniture.Y = dataSourceRow.getInt(4);
							furniture.Map = (short)dataSourceRow.getInt(5);
							furniture.HomeX = furniture.X;
							furniture.HomeY = furniture.Y;
							furniture.Heading = 0;

							L1World.Instance.storeObject(furniture);
							L1World.Instance.addVisibleObject(furniture);
						}
						//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
						//ORIGINAL LINE: java.lang.reflect.Constructor<?> constructor = Class.forName("l1j.server.server.model.Instance." + s + "Instance").getConstructors()[0];
						System.Reflection.ConstructorInfo<object> constructor = Type.GetType("l1j.server.server.model.Instance." + s + "Instance").GetConstructors()[0];
						object[] parameters = new object[] { l1npc };
						L1FurnitureInstance furniture = (L1FurnitureInstance)constructor.Invoke(parameters);

					}
				} while (true);
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			catch (SecurityException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			catch (ClassNotFoundException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			catch (System.ArgumentException e)
			{
				_log.Error(e);
			}
			catch (InstantiationException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			catch (IllegalAccessException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			catch (InvocationTargetException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		public virtual void insertFurniture(L1FurnitureInstance furniture)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("INSERT INTO spawnlist_furniture SET item_obj_id=?, npcid=?, locx=?, locy=?, mapid=?");
				pstm.setInt(1, furniture.ItemObjId);
				pstm.setInt(2, furniture.NpcTemplate.get_npcId());
				pstm.setInt(3, furniture.X);
				pstm.setInt(4, furniture.Y);
				pstm.setInt(5, furniture.MapId);
				pstm.execute();
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		public virtual void deleteFurniture(L1FurnitureInstance furniture)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("DELETE FROM spawnlist_furniture WHERE item_obj_id=?");
				pstm.setInt(1, furniture.ItemObjId);
				pstm.execute();
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

	}

}