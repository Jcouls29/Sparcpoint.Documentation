using Microsoft.SqlServer.TransactSql.ScriptDom;
using Sparcpoint.Documentation.Abstractions;
using System;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateSchemaStatementHandler : ISqlServerStatementHandler<CreateSchemaStatement>
    {
        public void Handle(CreateSchemaStatement statement, ISqlTree tree, SqlScriptGenerator generator)
            => tree.Add(StatementHelpers.FillModel(new SchemaModel(), statement.Name, statement, generator));
    }
}
