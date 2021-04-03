using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
    class MailTable : IGameComponent
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.Mail);
        private static MailTable _instance;

        private static IList<L1Mail> _allMail = ListFactory.NewList<L1Mail>();

        public static MailTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MailTable();
                }
                return _instance;
            }
        }

        private MailTable()
        {

        }
        public void Initialize()
        {
            loadMail();
        }
        private void loadMail()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                L1Mail mail = new L1Mail();
                mail.Id = dataSourceRow.getInt(Mail.Column_id);
                mail.Type = dataSourceRow.getInt(Mail.Column_type);
                mail.SenderName = dataSourceRow.getString(Mail.Column_sender);
                mail.ReceiverName = dataSourceRow.getString(Mail.Column_receiver);
                mail.Date = dataSourceRow.getTimestamp(Mail.Column_date);
                mail.ReadStatus = dataSourceRow.getInt(Mail.Column_read_status);
                mail.Subject = dataSourceRow.getBlob(Mail.Column_subject);
                mail.Content = dataSourceRow.getBlob(Mail.Column_content);
                mail.InBoxId = dataSourceRow.getInt(Mail.Column_inbox_id);

                _allMail.Add(mail);
            }
        }

        public virtual int ReadStatus
        {
            set
            {
                IDataSourceRow dataSourceRow = dataSource.NewRow();
                dataSourceRow.Update()
                .Where(Mail.Column_id, value)
                .Set(Mail.Column_read_status, 1)
                .Execute();
            }
        }

        public virtual void setMailType(int mailId, int type)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Update()
            .Where(Mail.Column_id, mailId)
            .Set(Mail.Column_type, type)
            .Execute();
        }

        public virtual void deleteMail(int mailId)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Delete()
            .Where(Mail.Column_id, mailId)
            .Execute();
        }

        public virtual int writeMail(int type, string receiver, L1PcInstance writer, byte[] text, int inboxId)
        {
            // subjectとcontentの区切り(0x00 0x00)位置を見つける
            int spacePosition1 = 0;
            int spacePosition2 = 0;
            for (int i = 0; i < text.Length; i += 2)
            {
                if ((text[i] == 0) && (text[i + 1] == 0))
                {
                    if (spacePosition1 == 0)
                    {
                        spacePosition1 = i;
                    }
                    else if ((spacePosition1 != 0) && (spacePosition2 == 0))
                    {
                        spacePosition2 = i;
                        break;
                    }
                }
            }

            // mailテーブルに書き込む
            int subjectLength = spacePosition1 + 2;
            int contentLength = spacePosition2 - spacePosition1;
            if (contentLength <= 0)
            {
                contentLength = 1;
            }
            byte[] subject = new byte[subjectLength];
            byte[] content = new byte[contentLength];
            Array.Copy(text, 0, subject, 0, subjectLength);
            Array.Copy(text, subjectLength, content, 0, contentLength);

            int id = Container.Instance.Resolve<IIdFactory>().nextId();
            DateTime date = DateTime.Now;
            int readStatus = 0;
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Insert()
            .Set(Mail.Column_id, id)
            .Set(Mail.Column_type, type)
            .Set(Mail.Column_sender, writer.Name)
            .Set(Mail.Column_receiver, receiver)
            .Set(Mail.Column_date, date)
            .Set(Mail.Column_read_status, readStatus)
            .Set(Mail.Column_subject, subject)
            .Set(Mail.Column_content, content)
            .Set(Mail.Column_inbox_id, inboxId)
            .Execute();

            L1Mail mail = new L1Mail();
            mail.Id = id;
            mail.Type = type;
            mail.SenderName = writer.Name;
            mail.ReceiverName = receiver;
            mail.Date = date;
            mail.Subject = subject;
            mail.Content = content;
            mail.ReadStatus = readStatus;
            mail.InBoxId = inboxId;
            _allMail.Add(mail);
            return id;
        }

        public static IList<L1Mail> AllMail
        {
            get
            {
                return _allMail;
            }
        }

        public static L1Mail getMail(int mailId)
        {
            foreach (L1Mail mail in _allMail)
            {
                if (mail.Id == mailId)
                {
                    return mail;
                }
            }
            return null;
        }

        private void changeMailStatus(int mailId)
        {
            foreach (L1Mail mail in _allMail)
            {
                if (mail.Id == mailId)
                {
                    L1Mail newMail = mail;
                    newMail.ReadStatus = 1;

                    _allMail.Remove(mail);
                    _allMail.Add(newMail);
                    break;
                }
            }
        }

        private void changeMailType(int mailId, int type)
        {
            foreach (L1Mail mail in _allMail)
            {
                if (mail.Id == mailId)
                {
                    L1Mail newMail = mail;
                    newMail.Type = type;

                    _allMail.Remove(mail);
                    _allMail.Add(newMail);
                    break;
                }
            }
        }

        private void delMail(int mailId)
        {
            foreach (L1Mail mail in _allMail)
            {
                if (mail.Id == mailId)
                {
                    _allMail.Remove(mail);
                    break;
                }
            }
        }

    }

}