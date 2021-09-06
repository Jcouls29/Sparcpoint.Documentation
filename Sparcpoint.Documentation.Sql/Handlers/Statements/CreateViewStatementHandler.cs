using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateViewStatementHandler : ISqlServerStatementHandler<CreateViewStatement>
    {
        public async Task HandleAsync(CreateViewStatement statement, ISqlTree tree, SqlScriptGenerator generator)
        {
            if (statement?.SchemaObjectName?.SchemaIdentifier?.Value == null)
                statement.SchemaObjectName.SchemaIdentifier.Value = "dbo";

            var identifier = statement.SchemaObjectName.ToSqlIdentifier();
            var schema = tree.Schemas[identifier.ToSchemaIdentifier()];

            var view = new ViewModel(schema)
            {
                Identifier = identifier,
                Description = statement.GetDescription(),
                Fragment = statement,
                CreateStatement = generator.Generate(statement)
            };

            tree.Add(view);
            schema.Views.Add(view);
        }
    }
}
