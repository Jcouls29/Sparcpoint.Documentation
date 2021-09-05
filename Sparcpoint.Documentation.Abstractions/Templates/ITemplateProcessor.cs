using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Abstractions
{
    public interface ITemplateProcessor<TOutput>
    {
        Task<TOutput> ProcessAsync<TInput>(Template template, TInput inputModel);
    }
}
