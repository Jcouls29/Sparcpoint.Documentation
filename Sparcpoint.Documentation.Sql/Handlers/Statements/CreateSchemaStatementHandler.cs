using Microsoft.SqlServer.TransactSql.ScriptDom;
using Sparcpoint.Documentation.Abstractions;
using System;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateSchemaStatementHandler : ISqlServerStatementHandler<CreateSchemaStatement>
    {
        public async Task HandleAsync(CreateSchemaStatement statement, ISqlTree tree, SqlScriptGenerator generator)
        {
            tree.Add(new SchemaModel
            {
                Identifier = statement?.Name?.ToSqlIdentifier() ?? throw new Exception("Missing Schema Name"),
                Description = statement.GetDescription(),
                Fragment = statement,
                CreateStatement = generator.Generate(statement),
            });
        }
    }
}
