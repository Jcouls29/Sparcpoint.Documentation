using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Sparcpoint.Documentation.Sql
{
    public class TableIndexModel
    {
        public string Name { get; set; }
        public SortedColumnModel[] Columns { get; set; }
        public ColumnList IncludeColumns { get; set; }
        public string Filter { get; set; }

        public TSqlFragment Fragment { get; set; }

        public override string ToString()
            => Name;
    }
}
