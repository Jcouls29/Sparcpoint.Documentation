using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Sparcpoint.Documentation.Sql
{
    public interface ISqlTree : IReadOnlySqlTree
    {
        void Add<T>(T model) where T : ISqlModel;

        void DeferConstraint(TableModel table, ConstraintDefinition definition);
        IEnumerable<DeferredTableConstraint> GetDeferredConstraints();
    }
}
