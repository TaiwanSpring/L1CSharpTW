private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Mail);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Mail.Column_id, obj.Id)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Mail.Column_id, obj.Id)
.Set(Mail.Column_type, obj.Type)
.Set(Mail.Column_sender, obj.Sender)
.Set(Mail.Column_receiver, obj.Receiver)
.Set(Mail.Column_date, obj.Date)
.Set(Mail.Column_read_status, obj.ReadStatus)
.Set(Mail.Column_subject, obj.Subject)
.Set(Mail.Column_content, obj.Content)
.Set(Mail.Column_inbox_id, obj.InboxId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Mail.Column_id, obj.Id)
.Set(Mail.Column_type, obj.Type)
.Set(Mail.Column_sender, obj.Sender)
.Set(Mail.Column_receiver, obj.Receiver)
.Set(Mail.Column_date, obj.Date)
.Set(Mail.Column_read_status, obj.ReadStatus)
.Set(Mail.Column_subject, obj.Subject)
.Set(Mail.Column_content, obj.Content)
.Set(Mail.Column_inbox_id, obj.InboxId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Mail.Column_id, obj.Id)
.Execute();


obj.Id = dataSourceRow.getString(Mail.Column_id);
obj.Type = dataSourceRow.getString(Mail.Column_type);
obj.Sender = dataSourceRow.getString(Mail.Column_sender);
obj.Receiver = dataSourceRow.getString(Mail.Column_receiver);
obj.Date = dataSourceRow.getString(Mail.Column_date);
obj.ReadStatus = dataSourceRow.getString(Mail.Column_read_status);
obj.Subject = dataSourceRow.getString(Mail.Column_subject);
obj.Content = dataSourceRow.getString(Mail.Column_content);
obj.InboxId = dataSourceRow.getString(Mail.Column_inbox_id);

