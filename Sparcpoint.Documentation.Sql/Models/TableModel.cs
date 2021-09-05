using System.Collections.Generic;

namespace Sparcpoint.Documentation.Sql
{
    public class TableModel : SqlModelBase
    {
        public TableColumnModel[] Columns { get; set; }

        public IEnumerable<TableIndexModel> UniqueIndices { get; set; }
        public IEnumerable<TableIndexModel> Indices { get; set; }
    }
}
