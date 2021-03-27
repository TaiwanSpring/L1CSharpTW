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
namespace LineageServer.Server.Model.Instance
{
	using L1World = LineageServer.Server.Model.L1World;
	using S_RemoveObject = LineageServer.Serverpackets.S_RemoveObject;
	using L1Npc = LineageServer.Server.Templates.L1Npc;

	[Serializable]
	public class L1FurnitureInstance : L1NpcInstance
	{

		private const long serialVersionUID = 1L;

		private int _itemObjId;

		public L1FurnitureInstance(L1Npc template) : base(template)
		{
		}

		public override void onAction(L1PcInstance player)
		{
		}

		public override void deleteMe()
		{
			_destroyed = true;
			if (Inventory != null)
			{
				Inventory.clearItems();
			}
			L1World.Instance.removeVisibleObject(this);
			L1World.Instance.removeObject(this);
			foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(this))
			{
				pc.removeKnownObject(this);
				pc.sendPackets(new S_RemoveObject(this));
			}
			removeAllKnownObjects();
		}

		public virtual int ItemObjId
		{
			get
			{
				return _itemObjId;
			}
			set
			{
				_itemObjId = value;
			}
		}


	}

}