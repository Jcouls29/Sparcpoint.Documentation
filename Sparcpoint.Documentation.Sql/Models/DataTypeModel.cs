using System;

namespace Sparcpoint.Documentation.Sql
{
    public class DataTypeModel : SqlModelBase
    {
        public DataTypeModel(SchemaModel schema)
        {
            Schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }

        public SchemaModel Schema { get; set; }
    }
}
