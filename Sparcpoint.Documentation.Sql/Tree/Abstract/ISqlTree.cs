using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Sparcpoint.Documentation.Sql
{
    public interface ISqlTree
    {
        void Add<T>(T model) where T : ISqlModel;
        T Get<T>(SqlIdentifier identifier) where T : ISqlModel;
        IEnumerable<T> Get<T>() where T : ISqlModel;

        void DeferConstraint(TableModel table, ConstraintDefinition definition);
        IEnumerable<DeferredTableConstraint> GetDeferredConstraints();

        IReadOnlySqlIndexer<SchemaModel> Schemas { get; }
        IReadOnlySqlIndexer<TableModel> Tables { get; }
        IReadOnlySqlIndexer<TableTypeModel> TableTypes { get; }
    }
}
