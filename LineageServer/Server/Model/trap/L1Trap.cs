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
namespace LineageServer.Server.Model.trap
{
	using GameObject = LineageServer.Server.Model.GameObject;
	using L1World = LineageServer.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using S_EffectLocation = LineageServer.Serverpackets.S_EffectLocation;
	using TrapStorage = LineageServer.Server.Storage.TrapStorage;

	public abstract class L1Trap
	{
		protected internal readonly int _id;
		protected internal readonly int _gfxId;
		protected internal readonly bool _isDetectionable;

		public L1Trap(TrapStorage storage)
		{
			_id = storage.getInt("id");
			_gfxId = storage.getInt("gfxId");
			_isDetectionable = storage.getBoolean("isDetectionable");
		}

		public L1Trap(int id, int gfxId, bool detectionable)
		{
			_id = id;
			_gfxId = gfxId;
			_isDetectionable = detectionable;
		}

		public virtual int Id
		{
			get
			{
				return _id;
			}
		}

		public virtual int GfxId
		{
			get
			{
				return _gfxId;
			}
		}

		protected internal virtual void sendEffect(GameObject trapObj)
		{
			if (GfxId == 0)
			{
				return;
			}
			S_EffectLocation effect = new S_EffectLocation(trapObj.Location, GfxId);

			foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(trapObj))
			{
				pc.sendPackets(effect);
			}
		}

		public abstract void onTrod(L1PcInstance trodFrom, GameObject trapObj);

		public virtual void onDetection(L1PcInstance caster, GameObject trapObj)
		{
			if (_isDetectionable)
			{
				sendEffect(trapObj);
			}
		}

		public static L1Trap newNull()
		{
			return new L1NullTrap();
		}
	}

	internal class L1NullTrap : L1Trap
	{
		public L1NullTrap() : base(0, 0, false)
		{
		}

		public override void onTrod(L1PcInstance trodFrom, GameObject trapObj)
		{
		}
	}

}