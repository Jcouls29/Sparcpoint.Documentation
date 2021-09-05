using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public interface ISqlServerStatementHandler<TStatement>
        where TStatement : TSqlStatement
    {
        Task HandleAsync(TStatement statement, ISqlTree tree, SqlScriptGenerator generator);
    }

    public interface ISqlServerStatementHandler
    {
        bool CanHandle(TSqlStatement statement);
        Task HandleAsync(TSqlStatement statement, ISqlTree tree, SqlScriptGenerator generator);
    }
}
