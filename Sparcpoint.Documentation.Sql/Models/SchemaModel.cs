using System.Collections.Generic;

namespace Sparcpoint.Documentation.Sql
{
    public class SchemaModel : SqlModelBase
    {
        public IList<TableModel> Tables { get; } = new List<TableModel>();
        public IList<TableTypeModel> TableTypes { get; } = new List<TableTypeModel>();
    }
}
