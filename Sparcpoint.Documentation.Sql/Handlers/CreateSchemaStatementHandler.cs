using Microsoft.SqlServer.TransactSql.ScriptDom;
using Sparcpoint.Documentation.Abstractions;
using System;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateSchemaStatementHandler : ISqlServerStatementHandler<CreateSchemaStatement>
    {
        public async Task HandleAsync(string source, CreateSchemaStatement statement, ISqlTree tree, SqlScriptGenerator generator)
        {
            tree.Add(new SchemaModel
            {
                Identifier = statement?.Name?.ToSqlIdentifier() ?? throw new Exception("Missing Schema Name"),
                Description = string.Empty,
                Fragment = statement
            });
        }
    }

    public class CreateIndexStatementHandler : ISqlServerStatementHandler<CreateIndexStatement>
    {
        public async Task HandleAsync(string source, CreateIndexStatement statement, ISqlTree tree, SqlScriptGenerator generator)
        {
            var name = statement.Name;
            var columns = statement.Columns;
            var isUnique = statement.Unique;

            TableModel table = tree.Tables[statement.OnName];


        }
    }
}
