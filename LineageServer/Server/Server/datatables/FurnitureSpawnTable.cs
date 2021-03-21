using System;

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
namespace LineageServer.Server.Server.datatables
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using IdFactory = LineageServer.Server.Server.IdFactory;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1FurnitureInstance = LineageServer.Server.Server.Model.Instance.L1FurnitureInstance;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;

	public class FurnitureSpawnTable
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(FurnitureSpawnTable).FullName);

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
			Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
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

					L1Npc l1npc = NpcTable.Instance.getTemplate(rs.getInt(2));
					if (l1npc != null)
					{
						string s = l1npc.Impl;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: java.lang.reflect.Constructor<?> constructor = Class.forName("l1j.server.server.model.Instance." + s + "Instance").getConstructors()[0];
						System.Reflection.ConstructorInfo<object> constructor = Type.GetType("l1j.server.server.model.Instance." + s + "Instance").GetConstructors()[0];
						object[] parameters = new object[] {l1npc};
						L1FurnitureInstance furniture = (L1FurnitureInstance) constructor.Invoke(parameters);
						furniture = (L1FurnitureInstance) constructor.Invoke(parameters);
						furniture.Id = IdFactory.Instance.nextId();

						furniture.ItemObjId = rs.getInt(1);
						furniture.X = rs.getInt(3);
						furniture.Y = rs.getInt(4);
						furniture.Map = (short) rs.getInt(5);
						furniture.HomeX = furniture.X;
						furniture.HomeY = furniture.Y;
						furniture.Heading = 0;

						L1World.Instance.storeObject(furniture);
						L1World.Instance.addVisibleObject(furniture);
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
				_log.log(Enum.Level.Server, e.Message, e);
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
			Connection con = null;
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
			Connection con = null;
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