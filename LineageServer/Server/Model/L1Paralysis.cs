namespace LineageServer.Server.Model
{
	/*
	 * 将来的には、凍結や石化もまとめて扱えればいいなーと思い、基礎クラス制定。 SkillTimerとの関係で難しいかもしれない。
	 */
	public abstract class L1Paralysis
	{
		public abstract int EffectId {get;}

		public abstract void cure();
	}

}