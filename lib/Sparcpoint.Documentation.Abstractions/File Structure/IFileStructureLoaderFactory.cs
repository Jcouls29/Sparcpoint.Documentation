namespace Sparcpoint.Documentation.Abstractions
{
    public interface IFileStructureLoaderFactory<TMarker>
    {
        public IFileStructureLoader FromPath(string path);
    }
}
