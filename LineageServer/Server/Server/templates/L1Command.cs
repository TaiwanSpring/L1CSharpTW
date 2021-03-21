namespace LineageServer.Server.Server.Templates
{
    public class L1Command
    {
        readonly int level;
        public string Name { get; }
        public string TypeName { get; }
        public L1Command(string name, int level, string typeName)
        {
            Name = name;
            this.level = level;
            TypeName = typeName;
        }

        public bool CanExecute(int level)
        {
            return level >= this.level;
        }
    }
}