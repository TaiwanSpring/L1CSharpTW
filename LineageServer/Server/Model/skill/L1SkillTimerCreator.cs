
namespace LineageServer.Server.Model.skill
{
    class L1SkillTimerCreator
    {
        public static IL1SkillTimer create(L1Character cha, int skillId, int timeMillis)
        {
            // 不正な値の場合は、とりあえずTimer
            return new L1SkillTimerTimerImpl(cha, skillId, timeMillis);
        }
    }

}