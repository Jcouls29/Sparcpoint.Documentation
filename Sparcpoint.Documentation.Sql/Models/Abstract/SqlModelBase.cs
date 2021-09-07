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
        public virtual string? CreateStatement { get; set; }

        public bool HasCreateStatement => CreateStatement != null;

        public override string ToString()
            => Identifier.ToString();
    }
}
