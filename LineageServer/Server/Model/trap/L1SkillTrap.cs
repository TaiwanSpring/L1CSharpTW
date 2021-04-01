using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Server.Storage;

namespace LineageServer.Server.Model.trap
{
	class L1SkillTrap : L1Trap
	{
		private readonly int _skillId;
		private readonly int _skillTimeSeconds;

		public L1SkillTrap(ITrapStorage storage) : base(storage)
		{

			_skillId = storage.getInt("skillId");
			_skillTimeSeconds = storage.getInt("skillTimeSeconds");
		}

		public override void onTrod(L1PcInstance trodFrom, GameObject trapObj)
		{
			sendEffect(trapObj);

			( new L1SkillUse() ).handleCommands(trodFrom, _skillId, trodFrom.Id, trodFrom.X, trodFrom.Y, null, _skillTimeSeconds, L1SkillUse.TYPE_GMBUFF);
		}

	}

}