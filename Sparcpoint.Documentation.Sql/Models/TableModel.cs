using Microsoft.SqlServer.TransactSql.ScriptDom;
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
        public IList<ForeignKeyReference> ForeignKeys { get; set; } = new List<ForeignKeyReference>();
        public PrimaryKeyConstraint? PrimaryKeyConstraint { get; set; }

        public TableColumnModel GetColumn(string name)
            => Columns?.FirstOrDefault(c => c.Name == name) ?? throw new Exception("Column not found.");
        public TableColumnModel GetColumn(Identifier name)
            => Columns?.FirstOrDefault(c => c.Name == name.Value) ?? throw new Exception("Column not found.");

        public override string ToString()
            => Identifier.ToString();
    }
}
