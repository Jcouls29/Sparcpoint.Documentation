using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Sparcpoint.Documentation.Sql
{
    public class TableIndexModel
    {
        public string Name { get; set; }
        public TableColumnModel[] Columns { get; set; }

        public TSqlFragment Fragment { get; set; }
    }
}
