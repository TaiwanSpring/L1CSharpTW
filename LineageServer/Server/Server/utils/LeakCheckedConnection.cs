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
namespace LineageServer.Server.Server.utils
{

	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	public class LeakCheckedConnection
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static readonly Logger _log = Logger.getLogger(typeof(LeakCheckedConnection).FullName);

		private Connection _con;

		private IDictionary<Statement, Exception> _openedStatements = Maps.newMap();

		private IDictionary<ResultSet, Exception> _openedResultSets = Maps.newMap();

		private object _proxy;

		private LeakCheckedConnection(Connection con)
		{
			_con = con;
			_proxy = Proxy.newProxyInstance(typeof(Connection).ClassLoader, new Type[] {typeof(Connection)}, new ConnectionHandler(this));
		}

		public static Connection create(Connection con)
		{
			return (Connection) (new LeakCheckedConnection(con))._proxy;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private Object send(Object o, java.lang.reflect.Method m, Object[] args) throws Throwable
		private object send(object o, System.Reflection.MethodInfo m, object[] args)
		{
			try
			{
				return m.invoke(o, args);
			}
			catch (InvocationTargetException e)
			{
				if (e.InnerException != null)
				{
					throw e.InnerException;
				}
				throw e;
			}
		}

		private void remove(object o)
		{
			if (o is ResultSet)
			{
				_openedResultSets.Remove(o);
			}
			else if (o is Statement)
			{
				_openedStatements.Remove(o);
			}
			else
			{
				throw new System.ArgumentException("bad class:" + o);
			}
		}

		internal virtual void closeAll()
		{
			if (_openedResultSets.Count > 0)
			{
				foreach (Exception t in _openedResultSets.Values)
				{
					_log.log(Level.WARNING, "Leaked ResultSets detected.", t);
				}
			}
			if (_openedStatements.Count > 0)
			{
				foreach (Exception t in _openedStatements.Values)
				{
					_log.log(Level.WARNING, "Leaked Statement detected.", t);
				}
			}
			foreach (ResultSet rs in _openedResultSets.Keys)
			{
				SQLUtil.close(rs);
			}
			foreach (Statement ps in _openedStatements.Keys)
			{
				SQLUtil.close(ps);
			}
		}

		private class ConnectionHandler : InvocationHandler
		{
			private readonly LeakCheckedConnection outerInstance;

			public ConnectionHandler(LeakCheckedConnection outerInstance)
			{
				this.outerInstance = outerInstance;
			}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public Object invoke(Object proxy, java.lang.reflect.Method method, Object[] args) throws Throwable
			public override object invoke(object proxy, System.Reflection.MethodInfo method, object[] args)
			{
				if (method.Name.Equals("close"))
				{
					outerInstance.closeAll();
				}
				object o = outerInstance.send(outerInstance._con, method, args);
				if (o is Statement)
				{
					outerInstance._openedStatements[(Statement) o] = new Exception();
					o = (new Delegate(outerInstance, o, typeof(PreparedStatement)))._delegateProxy;
				}
				return o;
			}
		}

		private class Delegate : InvocationHandler
		{
			private readonly LeakCheckedConnection outerInstance;

			internal object _delegateProxy;

			internal object _original;

			internal Delegate(LeakCheckedConnection outerInstance, object o, Type c)
			{
				this.outerInstance = outerInstance;
				_original = o;
				_delegateProxy = Proxy.newProxyInstance(c.ClassLoader, new Type[] {c}, this);
			}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public Object invoke(Object proxy, java.lang.reflect.Method method, Object[] args) throws Throwable
			public override object invoke(object proxy, System.Reflection.MethodInfo method, object[] args)
			{
				if (method.Name.Equals("close"))
				{
					outerInstance.remove(_original);
				}
				object o = outerInstance.send(_original, method, args);
				if (o is ResultSet)
				{
					outerInstance._openedResultSets[(ResultSet) o] = new Exception();
					o = (new Delegate(outerInstance, o, typeof(ResultSet)))._delegateProxy;
				}
				return o;
			}
		}
	}

}