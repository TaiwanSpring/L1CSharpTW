using LineageServer.Server.Model.Instance;
using LineageServer.Server.Storage;
using LineageServer.Serverpackets;

namespace LineageServer.Server.Model.trap
{
	abstract class L1Trap
	{
		protected internal readonly int _id;
		protected internal readonly int _gfxId;
		protected internal readonly bool _isDetectionable;

		public L1Trap(ITrapStorage storage)
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

			foreach (L1PcInstance pc in Container.Instance.Resolve<IGameWorld>().getRecognizePlayer(trapObj))
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