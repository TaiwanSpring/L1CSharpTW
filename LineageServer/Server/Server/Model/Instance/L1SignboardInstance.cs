﻿using System;

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
namespace LineageServer.Server.Server.Model.Instance
{
	using S_SignboardPack = LineageServer.Server.Server.serverpackets.S_SignboardPack;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;

	[Serializable]
	public class L1SignboardInstance : L1NpcInstance
	{
		/// 
		private const long serialVersionUID = 1L;

		public L1SignboardInstance(L1Npc template) : base(template)
		{
		}

		public override void onAction(L1PcInstance pc)
		{
		}

		public override void onPerceive(L1PcInstance perceivedFrom)
		{
			perceivedFrom.addKnownObject(this);
			perceivedFrom.sendPackets(new S_SignboardPack(this));
		}
	}

}