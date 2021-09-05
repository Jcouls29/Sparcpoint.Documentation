using System;

namespace Sparcpoint.Documentation.Sql
{
    internal sealed class DefaultReadOnlySqlIndexer<T> : IReadOnlySqlIndexer<T>
        where T : ISqlModel
    {
        private readonly ISqlTree _Tree;

        public DefaultReadOnlySqlIndexer(ISqlTree tree)
        {
            _Tree = tree ?? throw new ArgumentNullException(nameof(tree));
        }

        public T this[SqlIdentifier id] => _Tree.Get<T>(id);
    }
}
