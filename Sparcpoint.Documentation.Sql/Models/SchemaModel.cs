using System.Collections.Generic;
using System.Linq;

namespace Sparcpoint.Documentation.Sql
{
    public class SchemaModel : SqlModelBase
    {
        public IList<TableModel> Tables { get; } = new List<TableModel>();
        public bool HasTables => Tables?.Any() ?? false;

        public IList<TableTypeModel> TableTypes { get; } = new List<TableTypeModel>();
        public bool HasTableTypes => TableTypes?.Any() ?? false;
    }
}
