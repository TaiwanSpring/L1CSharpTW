using LineageServer.Server.Model.Instance;
using System.Collections.Generic;
using System.Linq;

namespace LineageServer.Server.Model
{
    public class L1Buddy
    {
        private readonly int _charId;

        private readonly IDictionary<int, string> _buddys = new Dictionary<int, string>();

        public L1Buddy(int charId)
        {
            _charId = charId;
        }

        public virtual int CharId
        {
            get
            {
                return _charId;
            }
        }

        public virtual bool add(int objId, string name)
        {
            if (_buddys.ContainsKey(objId))
            {
                return false;
            }
            _buddys.Add(objId, name);
            return true;
        }

        public virtual bool remove(int objId)
        {
            return _buddys.Remove(objId);
        }

        public virtual bool remove(string name)
        {
            int id = 0;
            foreach (KeyValuePair<int, string> buddy in _buddys.ToArray())
            {
                if (name == buddy.Value)
                {
                    id = buddy.Key;
                    break;
                }
            }
            if (id == 0)
            {
                return false;
            }
            _buddys.Remove(id);
            return true;
        }

        public virtual string OnlineBuddyListString
        {
            get
            {
                string result = "";
                foreach (L1PcInstance pc in Container.Instance.Resolve<IGameWorld>().AllPlayers)
                {
                    if (_buddys.ContainsKey(pc.Id))
                    {
                        result += pc.Name + " ";
                    }
                }
                return result;
            }
        }

        public virtual string BuddyListString
        {
            get
            {
                string result = "";
                foreach (string name in _buddys.Values.ToArray())
                {
                    result += name + " ";
                }
                return result;
            }
        }

        public virtual bool containsId(int objId)
        {
            return _buddys.ContainsKey(objId);
        }

        public virtual bool containsName(string name)
        {
            foreach (string buddyName in _buddys.Values.ToArray())
            {
                if (name == buddyName)

                {
                    return true;
                }
            }
            return false;
        }

        public virtual int size()
        {
            return _buddys.Count;
        }
    }

}