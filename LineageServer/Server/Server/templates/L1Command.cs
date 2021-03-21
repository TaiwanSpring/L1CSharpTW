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
namespace LineageServer.Server.Server.Templates
{
	public class L1Command
	{
		private readonly string _name;
		private readonly int _level;
		private readonly string _executorClassName;

		public L1Command(string name, int level, string executorClassName)
		{
			_name = name;
			_level = level;
			_executorClassName = executorClassName;
		}

		public virtual string Name
		{
			get
			{
				return _name;
			}
		}

		public virtual int Level
		{
			get
			{
				return _level;
			}
		}

		public virtual string ExecutorClassName
		{
			get
			{
				return _executorClassName;
			}
		}
	}

}