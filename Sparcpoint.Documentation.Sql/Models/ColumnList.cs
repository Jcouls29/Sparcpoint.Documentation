using System.Collections.Generic;

namespace Sparcpoint.Documentation.Sql
{
    public class ColumnList : List<TableColumnModel>
    {
        public ColumnList(IEnumerable<TableColumnModel> columns) : base(columns) { }

        public override string ToString()
            => string.Join(", ", this);
    }
}
