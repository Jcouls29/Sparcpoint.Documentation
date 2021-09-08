using System;

namespace Sparcpoint.Documentation.Sql
{
    public class StoredProcedureModel : SqlModelBase
    {
        public StoredProcedureModel(SchemaModel schema)
        {
            Schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }

        public SchemaModel Schema { get; set; }
    }
}
