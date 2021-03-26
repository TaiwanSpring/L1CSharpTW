using LineageServer.Enum;
using LineageServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LineageServer.DataBase
{
    class DataSourceFactory : IDataSourceFactory
    {
        private static readonly Dictionary<DataSourceTypeEnum, IDataSource> dataSourceMapping = new Dictionary<DataSourceTypeEnum, IDataSource>();
        private static readonly IDataSource nullDataSource;

        static DataSourceFactory()
        {
            //開機init一次
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            for (int index = 0; index < assemblies.Length; index++)
            {
                Assembly assembly = assemblies[index];

                Type[] types = assembly.GetTypes();
                for (int i = 0; i < types.Length; i++)
                {
                    if (types[i].IsSubclassOf(typeof(DataSource)) && !types[i].IsAbstract)
                    {
                        if (assembly.CreateInstance(types[i].FullName) is IDataSource dataSource)
                        {
                            if (dataSource.DataSourceType == DataSourceTypeEnum.Unknow)
                            {
                                if (nullDataSource != null)
                                {
                                    nullDataSource = dataSource;
                                }
                            }
                            else
                            {
                                if (!dataSourceMapping.ContainsKey(dataSource.DataSourceType))
                                {
                                    dataSourceMapping.Add(dataSource.DataSourceType, dataSource);
                                }
                            }
                        }
                    }
                }
            }
        }

        public IDataSource NullDataSource => nullDataSource;

        public IDataSource Factory(DataSourceTypeEnum dataSourceType)
        {
            if (dataSourceMapping.ContainsKey(dataSourceType))
            {
                return dataSourceMapping[dataSourceType];
            }
            else
            {
                return NullDataSource;
            }
        }
    }
}
