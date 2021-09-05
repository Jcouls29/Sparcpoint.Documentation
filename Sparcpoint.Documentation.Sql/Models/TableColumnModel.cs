using System;

namespace Sparcpoint.Documentation.Sql
{
    public class TableColumnModel
    {
        public TableColumnModel(ISqlModel table)
        {
            ParentReference = table ?? throw new ArgumentNullException(nameof(table));
        }

        public ISqlModel ParentReference { get; }

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
