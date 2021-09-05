using System.Collections.Generic;

namespace Sparcpoint.Documentation.Sql
{
    public interface IReadOnlySqlIndexer<T> : IEnumerable<T>
        where T : ISqlModel
    {
        T this[SqlIdentifier id] { get; }
    }
}
