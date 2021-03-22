using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.Interfaces
{
    /// <summary>
    /// 先做個簡易的
    /// </summary>
    interface IDataSourceRow
    {
        T GetColumn<T>(string column);
        void SetColumn<T>(string column, T value);
        bool Update();
        bool Selecte();
        bool Delete();
        bool IsNewRow { get; }
        bool IsDirty { get; }
    }
}
