using System;

namespace Sparcpoint.Documentation.Sql
{
    public class TableColumnModel
    {
        public TableColumnModel(TableModel table)
        {
            Table = table ?? throw new ArgumentNullException(nameof(table));
        }

        public TableModel Table { get; }

        public string Name { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public bool IsPrimaryKey { get; set; }
        public string? DefaultValue { get; set; }

        public IdentityColumnModel? Identity { get; set; }

        public override string ToString()
            => Name;
    }
}
