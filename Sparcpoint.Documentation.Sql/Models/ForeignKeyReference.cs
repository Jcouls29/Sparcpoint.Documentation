using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Sparcpoint.Documentation.Sql
{
    public class ForeignKeyReference
    {
        public SqlIdentifier Name { get; set; }

        public TableModel TargetTable { get; set; }
        public ColumnList LocalColumns { get; set; }
        public ColumnList ForeignColumns { get; set; }

        public DeleteUpdateAction DeleteAction { get; set; }
        public DeleteUpdateAction UpdateAction { get; set; }

        public TSqlFragment Fragment { get; set; }
    }
}
