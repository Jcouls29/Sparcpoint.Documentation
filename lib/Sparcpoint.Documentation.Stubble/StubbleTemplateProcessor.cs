using Sparcpoint.Documentation.Abstractions;
using Stubble.Core;
using Stubble.Core.Builders;
using System;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Stubble
{
    public class StubbleTemplateProcessor : ITemplateProcessor<string>
    {
        private readonly StubbleVisitorRenderer _Renderer;

        public StubbleTemplateProcessor()
        {
            _Renderer = new StubbleBuilder().Build();
        }

        public async Task<string> ProcessAsync<TInput>(Template template, TInput inputModel)
            => await _Renderer.RenderAsync(template.Value, inputModel);
    }
}
