private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Letter);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Letter.Column_item_object_id, obj.ItemObjectId)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Letter.Column_item_object_id, obj.ItemObjectId)
.Set(Letter.Column_code, obj.Code)
.Set(Letter.Column_sender, obj.Sender)
.Set(Letter.Column_receiver, obj.Receiver)
.Set(Letter.Column_date, obj.Date)
.Set(Letter.Column_template_id, obj.TemplateId)
.Set(Letter.Column_subject, obj.Subject)
.Set(Letter.Column_content, obj.Content)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Letter.Column_item_object_id, obj.ItemObjectId)
.Set(Letter.Column_code, obj.Code)
.Set(Letter.Column_sender, obj.Sender)
.Set(Letter.Column_receiver, obj.Receiver)
.Set(Letter.Column_date, obj.Date)
.Set(Letter.Column_template_id, obj.TemplateId)
.Set(Letter.Column_subject, obj.Subject)
.Set(Letter.Column_content, obj.Content)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Letter.Column_item_object_id, obj.ItemObjectId)
.Execute();


obj.ItemObjectId = dataSourceRow.getString(Letter.Column_item_object_id);
obj.Code = dataSourceRow.getString(Letter.Column_code);
obj.Sender = dataSourceRow.getString(Letter.Column_sender);
obj.Receiver = dataSourceRow.getString(Letter.Column_receiver);
obj.Date = dataSourceRow.getString(Letter.Column_date);
obj.TemplateId = dataSourceRow.getString(Letter.Column_template_id);
obj.Subject = dataSourceRow.getString(Letter.Column_subject);
obj.Content = dataSourceRow.getString(Letter.Column_content);

