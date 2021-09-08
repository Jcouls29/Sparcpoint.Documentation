using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Sparcpoint.Documentation.Sql
{
    public class ForeignKeyReference
    {
        public SqlIdentifier Name { get; set; }
        public string Description { get; set; } = string.Empty;

        public TableModel? TargetTable { get; set; } = null;
        public ColumnList LocalColumns { get; set; } = new ColumnList();
        public ColumnList ForeignColumns { get; set; } = new ColumnList();

        public IEnumerable<ForeignKeyColumnMap> ColumnMapping
        {
            get
            {
                if (LocalColumns.Count != ForeignColumns.Count)
                    throw new System.Exception("Column mismatch.");

                List<ForeignKeyColumnMap> mapping = new List<ForeignKeyColumnMap>();
                for(int i = 0; i < LocalColumns.Count; i++)
                {
                    mapping.Add(new ForeignKeyColumnMap { Local = LocalColumns[i], Foreign = ForeignColumns[i] });
                }

                return mapping;
            }
        }

        public DeleteUpdateAction DeleteAction { get; set; }
        public DeleteUpdateAction UpdateAction { get; set; }

        public TSqlFragment? Fragment { get; set; }
    }

    public class ForeignKeyColumnMap
    {
        public TableColumnModel Local { get; set; }
        public TableColumnModel Foreign { get; set; }
    }
}
