using System;
using System.Collections.Generic;
using System.Linq;

namespace Sparcpoint.Documentation.Sql
{
    public class ViewModel : SqlModelBase
    {
        public ViewModel(SchemaModel schema)
        {
            Schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }

        public SchemaModel Schema { get; set; }

        public IList<TriggerModel> Triggers { get; set; } = new List<TriggerModel>();
        public bool HasTriggers => Triggers?.Any() ?? false;
    }
}
