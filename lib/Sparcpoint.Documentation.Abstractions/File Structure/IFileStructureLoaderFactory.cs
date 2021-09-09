namespace Sparcpoint.Documentation.Abstractions
{
    public interface IFileStructureLoaderFactory<TMarker>
    {
        IFileStructureLoader FromPath(string path);
    }
}
