using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LineageServer.Command
{
    /// <summary>
    /// 處理 GM 指令
    /// </summary>
    class L1Commands
    {
        static Dictionary<string, L1Command> commands = new Dictionary<string, L1Command>();
        static Dictionary<string, ILineageCommand> commandExecutors = new Dictionary<string, ILineageCommand>();

        private static ILogger _log = Logger.GetLogger(nameof(L1Commands));

        static L1Commands()
        {
            commandExecutors = GetCommandExecutors(GetCommands());
        }

        private static Dictionary<string, ILineageCommand> GetCommandExecutors(Dictionary<string, L1Command> commands)
        {
            Assembly assembly = typeof(L1Commands).Assembly;
            Type[] types = assembly.GetTypes();
            string interfaceName = typeof(ILineageCommand).FullName;
            Dictionary<string, Type> tempMapping = new Dictionary<string, Type>();
            foreach (var item in types)
            {
                if (item.GetInterface(interfaceName) != null)
                {
                    tempMapping.Add(item.Name, item);
                }
            }
            Dictionary<string, ILineageCommand> result = new Dictionary<string, ILineageCommand>();
            foreach (var item in commands)
            {
                if (tempMapping.ContainsKey(item.Key))
                {
                    if (assembly.CreateInstance(tempMapping[item.Key].FullName) is ILineageCommand commandExecutor)
                    {
                        result.Add(item.Key, commandExecutor);
                    }
                }
            }
            return result;
        }
        private static Dictionary<string, L1Command> GetCommands()
        {
            Dictionary<string, L1Command> commands = new Dictionary<string, L1Command>();
            try
            {
                IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Commands);

                IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();
                for (int i = 0; i < dataSourceRows.Count; i++)
                {
                    L1Command l1Command = new L1Command(
                        dataSourceRows[i].getString(Commands.Column_name),
                        dataSourceRows[i].getInt(Commands.Column_access_level),
                        dataSourceRows[i].getString(Commands.Column_class_name));
                    commands.Add(l1Command.Name, l1Command);
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
            return commands;
        }

        public static L1Command GetCommand(string name)
        {
            if (commands.ContainsKey(name))
            {
                return commands[name];
            }
            else
            {
                return null;
            }
        }
        public static ILineageCommand GetCommandExecutor(string name)
        {
            if (commandExecutors.ContainsKey(name))
            {
                return commandExecutors[name];
            }
            else
            {
                return null;
            }
        }
        public static IList<L1Command> availableCommandList(int accessLevel)
        {
            IList<L1Command> result = ListFactory.NewList<L1Command>();
            foreach (L1Command item in commands.Values)
            {
                if (item.CanExecute(accessLevel))
                {
                    result.Add(item);
                }
            }
            return result;
        }
    }

}