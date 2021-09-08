using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Sparcpoint.Documentation.Sql
{
    public class TableIndexModel : SqlModelBase
    {
        public SortedColumnModel[] Columns { get; set; } = new SortedColumnModel[] { };
        public ColumnList IncludeColumns { get; set; } = new ColumnList();
        public string Filter { get; set; } = string.Empty;

        public override string ToString()
            => Identifier.ToString();
    }
}
