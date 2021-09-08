using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Sparcpoint.Documentation.Sql
{
    public interface ISqlServerConstraintHandler<TConstraint>
        where TConstraint : ConstraintDefinition
    {
        void Handle(TableModel table, TConstraint constraint, ISqlTree tree, SqlScriptGenerator generator);
    }

    public interface ISqlServerConstraintHandler
    {
        bool CanHandle(DeferredTableConstraint constraint);
        void Handle(DeferredTableConstraint constraint, ISqlTree tree, SqlScriptGenerator generator);
    }
}
