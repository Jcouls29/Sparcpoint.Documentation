using System;

namespace Sparcpoint.Documentation.Sql
{
    public class FunctionModel : SqlModelBase
    {
        public FunctionModel(SchemaModel schema)
        {
            Schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }

        public SchemaModel Schema { get; set; }
    }
}
