using System.Linq;
using System;

namespace Sparcpoint.Documentation.Sql
{
    public class TableTypeModel : SqlModelBase
    {
        public TableTypeModel(SchemaModel schema)
        {
            Schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }

        public SchemaModel Schema { get; }

        public TableColumnModel[] Columns { get; set; }

        public TableColumnModel GetColumn(string name)
        {
            return Columns?.FirstOrDefault(c => c.Name == name) ?? throw new Exception("Column not found.");
        }

        public override string ToString()
            => Identifier.ToString();
    }
}
