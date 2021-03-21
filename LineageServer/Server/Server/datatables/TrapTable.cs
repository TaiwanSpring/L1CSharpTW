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
namespace LineageServer.Server.Server.datatables
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1Trap = LineageServer.Server.Server.Model.trap.L1Trap;
	using TrapStorage = LineageServer.Server.Server.storage.TrapStorage;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	public class TrapTable
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(TrapTable).FullName);

		private static TrapTable _instance;

		private IDictionary<int, L1Trap> _traps = Maps.newMap();

		private TrapTable()
		{
			initialize();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private l1j.server.server.model.trap.L1Trap createTrapInstance(String name, l1j.server.server.storage.TrapStorage storage) throws Exception
		private L1Trap createTrapInstance(string name, TrapStorage storage)
		{
			const string packageName = "l1j.server.server.model.trap.";

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: java.lang.reflect.Constructor<?> con = Class.forName(packageName + name).getConstructor(new Class[] { l1j.server.server.storage.TrapStorage.class });
			System.Reflection.ConstructorInfo<object> con = Type.GetType(packageName + name).GetConstructor(new Type[] {typeof(TrapStorage)});
			return (L1Trap) con.Invoke(storage);
		}

		private void initialize()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;

			try
			{
				con = L1DatabaseFactory.Instance.Connection;

				pstm = con.prepareStatement("SELECT * FROM trap");

				rs = pstm.executeQuery();

				while (rs.next())
				{
					string typeName = rs.getString("type");

					L1Trap trap = createTrapInstance(typeName, new SqlTrapStorage(this, rs));

					_traps[trap.Id] = trap;
				}
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			catch (Exception e)
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

		public static TrapTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new TrapTable();
				}
				return _instance;
			}
		}

		public static void reload()
		{
			TrapTable oldInstance = _instance;
			_instance = new TrapTable();

			oldInstance._traps.Clear();
		}

		public virtual L1Trap getTemplate(int id)
		{
			return _traps[id];
		}

		private class SqlTrapStorage : TrapStorage
		{
			private readonly TrapTable outerInstance;

			internal readonly ResultSet _rs;

			public SqlTrapStorage(TrapTable outerInstance, ResultSet rs)
			{
				this.outerInstance = outerInstance;
				_rs = rs;
			}

			public virtual string getString(string name)
			{
				try
				{
					return _rs.getString(name);
				}
				catch (SQLException)
				{
				}
				return "";
			}

			public virtual int getInt(string name)
			{
				try
				{
					return _rs.getInt(name);
				}
				catch (SQLException)
				{

				}
				return 0;
			}

			public virtual bool getBoolean(string name)
			{
				try
				{
					return _rs.getBoolean(name);
				}
				catch (SQLException)
				{
				}
				return false;
			}
		}
	}

}