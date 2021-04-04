
/*using LineageServer.Server;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace LineageServer.Serverpackets
{
    class S_CommonNews : ServerBasePacket
    {

        public S_CommonNews()
        {
            _announcements = ListFactory.NewList<string>();
            loadAnnouncements();
            if (_announcements.Count == 0)
            {
                return;
            }
            WriteC(Opcodes.S_OPCODE_COMMONNEWS);
            string message = "";
            for (int i = 0; i < _announcements.Count; i++)
            {
                message = (new StringBuilder()).Append(message).Append(_announcements[i]).Append("\n").ToString();
            }
            WriteS(message);
        }

        public S_CommonNews(string s)
        {
            WriteC(Opcodes.S_OPCODE_COMMONNEWS);
            WriteS(s);
        }

        private void loadAnnouncements()
        {
            _announcements.Clear();
            File file = new File("data/announcements.txt");
            if (file.exists())
            {
                readFromDisk(file);
            }
        }

        private void readFromDisk(File file)
        {
            LineNumberReader lnr = null;
            try
            {
                string line = null;
                lnr = new LineNumberReader(new StreamReader(file));
                do
                {
                    if (string.ReferenceEquals((line = lnr.readLine()), null))
                    {
                        break;
                    }
                    StringTokenizer st = new StringTokenizer(line, "\n\r");
                    if (st.hasMoreTokens())
                    {
                        string announcement = st.nextToken();
                        _announcements.Add(announcement);
                    }
                    else
                    {
                        _announcements.Add(" ");
                    }
                } while (true);
            }
            catch (Exception)
            {
            }
        }

        public override string Type
        {
            get
            {
                return "[S] S_CommonNews";
            }
        }

        private IList<string> _announcements;

    }

}*/