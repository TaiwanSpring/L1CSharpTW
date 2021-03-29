namespace LineageServer.Server.Templates
{
    public class L1CharName
    {
        public L1CharName()
        {
        }

        private int _id;

        public virtual int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }


        private string _name;

        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }


    }
}