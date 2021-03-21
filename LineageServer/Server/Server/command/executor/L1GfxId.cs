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
namespace LineageServer.Server.Server.command.executor
{

	using IdFactory = LineageServer.Server.Server.IdFactory;
	using NpcTable = LineageServer.Server.Server.datatables.NpcTable;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;

	public class L1GfxId : L1CommandExecutor
	{
		private L1GfxId()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1GfxId();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				StringTokenizer st = new StringTokenizer(arg);
				int gfxid = Convert.ToInt32(st.nextToken(), 10);
				int count = Convert.ToInt32(st.nextToken(), 10);
				for (int i = 0; i < count; i++)
				{
					L1Npc l1npc = NpcTable.Instance.getTemplate(45001);
					if (l1npc != null)
					{
						string s = l1npc.Impl;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: java.lang.reflect.Constructor<?> constructor = Class.forName("l1j.server.server.model.Instance." + s + "Instance").getConstructors()[0];
						System.Reflection.ConstructorInfo<object> constructor = Type.GetType("l1j.server.server.model.Instance." + s + "Instance").GetConstructors()[0];
						object[] aobj = new object[] {l1npc};
						L1NpcInstance npc = (L1NpcInstance) constructor.Invoke(aobj);
						npc.Id = IdFactory.Instance.nextId();
						npc.GfxId = gfxid + i;
						npc.TempCharGfx = 0;
						npc.NameId = "";
						npc.Map = pc.MapId;
						npc.X = pc.X + i * 2;
						npc.Y = pc.Y + i * 2;
						npc.HomeX = npc.X;
						npc.HomeY = npc.Y;
						npc.Heading = 4;

						L1World.Instance.storeObject(npc);
						L1World.Instance.addVisibleObject(npc);
					}
				}
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage(cmdName + " 請輸入  動畫編號  動畫數量  人物ID。"));
			}
		}
	}

}