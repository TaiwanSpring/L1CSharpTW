using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;

namespace LineageServer.Server.DataTables
{
    class LogEnchantTable
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.LogEnchant);
        public virtual void storeLogEnchant(int char_id, int item_id, int old_enchantlvl, int new_enchantlvl)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Insert()
            .Set(LogEnchant.Column_char_id, char_id)
            .Set(LogEnchant.Column_item_id, item_id)
            .Set(LogEnchant.Column_old_enchantlvl, old_enchantlvl)
            .Set(LogEnchant.Column_new_enchantlvl, new_enchantlvl)
            .Execute();
        }
    }
}