using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class DefaultSqlServerStatementHandler : ISqlServerStatementHandler
    {
        private readonly IServiceProvider _Provider;

        public DefaultSqlServerStatementHandler(IServiceProvider provider)
        {
            _Provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public bool CanHandle(TSqlStatement statement)
        {
            object service = GetService(statement);
            return service != null;
        }

        public async Task HandleAsync(TSqlStatement statement, ISqlTree tree, SqlScriptGenerator generator)
        {
            object service = GetService(statement);
            Type serviceType = service.GetType();
            MethodInfo method = serviceType.GetMethod(nameof(ISqlServerStatementHandler<TSqlStatement>.HandleAsync));

            Task result = (Task)method.Invoke(service, new object[] { statement, tree, generator });
            await result;
        }

        private object GetService(TSqlStatement statement)
        {
            Type handlerType = GetHandlerType(statement);
            return _Provider.GetService(handlerType);
        }

        private Type GetHandlerType(TSqlStatement statement)
        {
            Type inputType = statement.GetType();
            return typeof(ISqlServerStatementHandler<>).MakeGenericType(inputType);
        }
    }
}
