using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Sparcpoint.Documentation.Sql
{
    public sealed class DeferredTableConstraint
    {
        public TableModel? Table { get; set; }
        public ConstraintDefinition? Constraint { get; set; }
    }
}
