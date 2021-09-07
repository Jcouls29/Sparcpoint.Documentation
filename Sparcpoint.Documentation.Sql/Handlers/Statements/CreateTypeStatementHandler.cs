using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateTypeStatementHandler : ISqlServerStatementHandler<CreateTypeUddtStatement>
    {
        public async Task HandleAsync(CreateTypeUddtStatement statement, ISqlTree tree, SqlScriptGenerator generator)
        {
            if (statement?.Name?.SchemaIdentifier?.Value == null)
                statement.Name.SchemaIdentifier.Value = "dbo";

            var identifier = statement.Name.ToSqlIdentifier();
            var schema = tree.Schemas[identifier.ToSchemaIdentifier()];

            var dataType = new DataTypeModel(schema)
            {
                Identifier = identifier,
                Description = statement.GetDescription(),
                Fragment = statement,
                CreateStatement = generator.Generate(statement)
            };

            tree.Add(dataType);
            schema.DataTypes.Add(dataType);
        }
    }
}
