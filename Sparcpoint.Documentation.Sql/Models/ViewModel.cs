using System;

namespace Sparcpoint.Documentation.Sql
{
    public class ViewModel : SqlModelBase
    {
        public ViewModel(SchemaModel schema)
        {
            Schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }

        public SchemaModel Schema { get; set; }
    }
}
