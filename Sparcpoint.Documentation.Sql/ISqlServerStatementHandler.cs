using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public interface ISqlServerStatementHandler<TStatement>
        where TStatement : TSqlStatement
    {
        Task HandleAsync(string source, TStatement statement, ISqlTree tree, SqlScriptGenerator generator);
    }

    public interface ISqlServerStatementHandler
    {
        bool CanHandle(TSqlStatement statement);
        Task HandleAsync(string source, TSqlStatement statement, ISqlTree tree, SqlScriptGenerator generator);
    }
}
