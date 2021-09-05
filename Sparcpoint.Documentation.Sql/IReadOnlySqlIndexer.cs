namespace Sparcpoint.Documentation.Sql
{
    public interface IReadOnlySqlIndexer<T>
        where T : ISqlModel
    {
        T this[SqlIdentifier id] { get; }
    }
}
