namespace LineageServer.Server.Model.Npc
{
    public class L1NpcHtml
    {
        private readonly string _name;
        private readonly string[] _args;

        public static readonly L1NpcHtml HTML_CLOSE = new L1NpcHtml();
        private L1NpcHtml()
        {
        }

        public L1NpcHtml(string name) : this(name, new string[] { })
        {
        }

        public L1NpcHtml(string name, params string[] args)
        {
            if (string.IsNullOrEmpty(name) || args == null)
            {
                throw new System.NullReferenceException();
            }
            _name = name;
            _args = args;
        }

        public virtual string Name
        {
            get
            {
                return _name;
            }
        }

        public virtual string[] Args
        {
            get
            {
                return _args;
            }
        }
    }

}