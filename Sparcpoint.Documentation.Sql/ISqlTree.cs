using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;
using System.Linq;

namespace Sparcpoint.Documentation.Sql
{
    public interface ISqlTree
    {
        void Add<T>(T model) where T : ISqlModel;
        T Get<T>(SqlIdentifier identifier) where T : ISqlModel;

        void DeferConstraint(ConstraintDefinition definition);
        IEnumerable<ConstraintDefinition> GetDeferredConstraints();

        IReadOnlySqlIndexer<SchemaModel> Schemas { get; }
        IReadOnlySqlIndexer<TableModel> Tables { get; }
    }

    public sealed class InMemorySqlTree : ISqlTree
    {
        private readonly List<ISqlModel> _Models = new List<ISqlModel>();
        private readonly List<ConstraintDefinition> _DeferredConstraints = new List<ConstraintDefinition>();

        public InMemorySqlTree()
        {
            Schemas = new DefaultReadOnlySqlIndexer<SchemaModel>(this);
            Tables = new DefaultReadOnlySqlIndexer<TableModel>(this);

            _Models.Add(new SchemaModel
            {
                Identifier = new SqlIdentifier("dbo"),
            });
        }

        public IReadOnlySqlIndexer<SchemaModel> Schemas { get; }
        public IReadOnlySqlIndexer<TableModel> Tables { get; }

        public void Add<T>(T model) where T : ISqlModel
        {
            // TODO: Check for model already added.

            _Models.Add(model);
        }

        public T Get<T>(SqlIdentifier identifier) where T : ISqlModel
            => (T)_Models.FirstOrDefault(m => m is T && m.Identifier == identifier);

        public void DeferConstraint(ConstraintDefinition definition)
            => _DeferredConstraints.Add(definition);
        public IEnumerable<ConstraintDefinition> GetDeferredConstraints()
            => _DeferredConstraints;
    }
}
