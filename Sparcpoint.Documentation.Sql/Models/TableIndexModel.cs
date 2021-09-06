using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Sparcpoint.Documentation.Sql
{
    public class TableIndexModel : SqlModelBase
    {
        public SortedColumnModel[] Columns { get; set; }
        public ColumnList IncludeColumns { get; set; }
        public string Filter { get; set; }

        public override string ToString()
            => Identifier.ToString();
    }
}
