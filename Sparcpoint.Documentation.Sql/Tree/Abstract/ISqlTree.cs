using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Sparcpoint.Documentation.Sql
{
    public interface IReadOnlySqlTree
    {
        T Get<T>(SqlIdentifier identifier) where T : ISqlModel;
        IEnumerable<T> Get<T>() where T : ISqlModel;

        IReadOnlySqlIndexer<SchemaModel> Schemas { get; }
        IReadOnlySqlIndexer<TableModel> Tables { get; }
        IReadOnlySqlIndexer<TableTypeModel> TableTypes { get; }
        IReadOnlySqlIndexer<SequenceModel> Sequences { get; }
        IReadOnlySqlIndexer<ViewModel> Views { get; }
        IReadOnlySqlIndexer<StoredProcedureModel> StoredProcedures { get; }
        IReadOnlySqlIndexer<FunctionModel> Functions { get; }
        IReadOnlySqlIndexer<DataTypeModel> DataTypes { get; }
    }

    public interface ISqlTree : IReadOnlySqlTree
    {
        void Add<T>(T model) where T : ISqlModel;

        void DeferConstraint(TableModel table, ConstraintDefinition definition);
        IEnumerable<DeferredTableConstraint> GetDeferredConstraints();
    }
}
