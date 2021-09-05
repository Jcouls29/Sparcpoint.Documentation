using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sparcpoint.Documentation.Sql
{
    public class SqlModelBase : ISqlModel
    {
        public SqlIdentifier Identifier { get; set; }
        public virtual string Description { get; set; } = string.Empty;
        public virtual TSqlFragment? Fragment { get; set; } = null;

        public virtual Dictionary<string, object> GetProperties()
        {
            IEnumerable<PropertyInfo> properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return properties
                .Where(prop => prop.Name != nameof(ISqlModel.Fragment))
                .Select(prop => new
            {
                Key = prop.Name,
                Value = prop.GetValue(this)
            }).ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    }
}
