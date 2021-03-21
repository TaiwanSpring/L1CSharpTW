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
	using L1FieldObjectInstance = LineageServer.Server.Server.Model.Instance.L1FieldObjectInstance;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;

	public class LightSpawnTable
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(LightSpawnTable).FullName);

		private static LightSpawnTable _instance;

		public static LightSpawnTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new LightSpawnTable();
				}
				return _instance;
			}
		}

		private LightSpawnTable()
		{
			FillLightSpawnTable();
		}

		private void FillLightSpawnTable()
		{
			Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM spawnlist_light");
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
						L1FieldObjectInstance field = (L1FieldObjectInstance) constructor.Invoke(parameters);
						field = (L1FieldObjectInstance) constructor.Invoke(parameters);
						field.Id = IdFactory.Instance.nextId();
						field.X = rs.getInt("locx");
						field.Y = rs.getInt("locy");
						field.Map = (short) rs.getInt("mapid");
						field.HomeX = field.X;
						field.HomeY = field.Y;
						field.Heading = 0;
						field.LightSize = l1npc.LightSize;

						L1World.Instance.storeObject(field);
						L1World.Instance.addVisibleObject(field);
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

	}

}