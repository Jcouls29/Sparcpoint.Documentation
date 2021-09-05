using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Sparcpoint.Documentation.Sql
{
    public interface ISqlModel
    {
        SqlIdentifier Identifier { get; }
        string Description { get; }
        TSqlFragment? Fragment { get; }

        Dictionary<string, object> GetProperties();
    }
}
