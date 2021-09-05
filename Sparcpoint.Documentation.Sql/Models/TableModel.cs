using System;
using System.Collections.Generic;
using System.Linq;

namespace Sparcpoint.Documentation.Sql
{
    public class TableModel : SqlModelBase
    {
        public TableModel(SchemaModel schema)
        {
            Schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }

        public SchemaModel Schema { get; }

        public TableColumnModel[] Columns { get; set; }

        public IList<TableIndexModel> UniqueIndices { get; } = new List<TableIndexModel>();
        public IList<TableIndexModel> Indices { get; } = new List<TableIndexModel>();

        public TableColumnModel GetColumn(string name)
        {
            return Columns?.FirstOrDefault(c => c.Name == name) ?? throw new Exception("Column not found.");
        }

        public override string ToString()
            => Identifier.ToString();
    }
}
