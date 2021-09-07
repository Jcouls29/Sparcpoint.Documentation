using System;

namespace Sparcpoint.Documentation.Sql
{
    public class TriggerModel : SqlModelBase
    {
        public TriggerModel(SchemaModel schema)
        {
            Schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }

        public SchemaModel Schema { get; set; }

        public SqlModelBase Target { get; set; }
        public bool IsInsert { get; set; } = false;
        public bool IsUpdate { get; set; } = false;
        public bool IsDelete { get; set; } = false;
    }
}
