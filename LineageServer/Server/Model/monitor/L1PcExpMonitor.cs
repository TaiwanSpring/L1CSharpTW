using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;

namespace LineageServer.Server.Model.monitor
{
	class L1PcExpMonitor : L1PcMonitor
	{

		private int _old_lawful;

		private long _old_exp;

		private int _old_karma;

		// 上一次判斷時的戰鬥特化類別
		private int _oldFight;

		public L1PcExpMonitor(int oId) : base(oId)
		{
		}

		public override void execTask(L1PcInstance pc)
		{

			// ロウフルが変わった場合はS_Lawfulを送信
			// // ただし色が変わらない場合は送信しない
			// if (_old_lawful != pc.getLawful()
			// && !((IntRange.includes(_old_lawful, 9000, 32767) && IntRange
			// .includes(pc.getLawful(), 9000, 32767)) || (IntRange
			// .includes(_old_lawful, -32768, -2000) && IntRange
			// .includes(pc.getLawful(), -32768, -2000)))) {
			if (_old_lawful != pc.Lawful)
			{
				_old_lawful = pc.Lawful;
				S_Lawful s_lawful = new S_Lawful(pc.Id, _old_lawful);
				pc.sendPackets(s_lawful);
				pc.broadcastPacket(s_lawful);

				// 處理戰鬥特化系統
				if (Config.FIGHT_IS_ACTIVE)
				{
					// 計算目前的戰鬥特化組別
					int fightType = _old_lawful / 10000;

					// 判斷戰鬥特化組別是否有所變更
					if (_oldFight != fightType)
					{
						// 進行戰鬥特化組別的變更
						pc.changeFightType(_oldFight, fightType);

						_oldFight = fightType;
					}
				}

			}

			if (_old_karma != pc.Karma)
			{
				_old_karma = pc.Karma;
				pc.sendPackets(new S_Karma(pc));
			}

			if (_old_exp != pc.Exp)
			{
				_old_exp = pc.Exp;
				pc.onChangeExp();
			}
		}
	}

}