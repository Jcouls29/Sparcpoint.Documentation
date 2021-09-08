using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public interface ISqlTreeRenderer
    {
        Task RenderAsync(IReadOnlySqlTree tree);
    }
}
