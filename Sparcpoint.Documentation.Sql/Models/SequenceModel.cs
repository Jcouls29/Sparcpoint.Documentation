using System;

namespace Sparcpoint.Documentation.Sql
{
    public class SequenceModel : SqlModelBase
    {
        public SequenceModel(SchemaModel schema)
        {
            Schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }

        public SchemaModel Schema { get; set; }
    }
}
