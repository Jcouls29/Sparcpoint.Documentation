using Sparcpoint.Documentation.Abstractions;
using System;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public interface ISqlTreeRenderer
    {
        event EventHandler<FileSavedEventArgs> FileSaved;

        Task RenderAsync(IReadOnlySqlTree tree);
    }
}
