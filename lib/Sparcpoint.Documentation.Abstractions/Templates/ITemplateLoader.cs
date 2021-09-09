using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Abstractions
{
    public interface ITemplateLoader
    {
        Task<Template> LoadAsync<TInput>();
    }
}
