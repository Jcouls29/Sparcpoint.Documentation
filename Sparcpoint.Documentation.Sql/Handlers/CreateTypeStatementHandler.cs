using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateTypeStatementHandler : ISqlServerStatementHandler<CreateTypeTableStatement>
    {
        public async Task HandleAsync(CreateTypeTableStatement statement, ISqlTree tree, SqlScriptGenerator generator)
        {
            
        }
    }
}
