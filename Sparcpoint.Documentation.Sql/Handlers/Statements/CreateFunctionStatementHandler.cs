using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateFunctionStatementHandler : ISqlServerStatementHandler<CreateFunctionStatement>
    {
        public async Task HandleAsync(CreateFunctionStatement statement, ISqlTree tree, SqlScriptGenerator generator)
        {
            if (statement?.Name?.SchemaIdentifier?.Value == null)
                statement.Name.SchemaIdentifier.Value = "dbo";

            var identifier = statement.Name.ToSqlIdentifier();
            var schema = tree.Schemas[identifier.ToSchemaIdentifier()];

            var view = new FunctionModel(schema)
            {
                Identifier = identifier,
                Description = statement.GetDescription(),
                Fragment = statement,
                CreateStatement = generator.Generate(statement)
            };

            tree.Add(view);
            schema.Functions.Add(view);
        }
    }
}
