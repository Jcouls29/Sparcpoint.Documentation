using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateTableTypeStatementHandler : ISqlServerStatementHandler<CreateTypeTableStatement>
    {
        public async Task HandleAsync(CreateTypeTableStatement statement, ISqlTree tree, SqlScriptGenerator generator)
        {
            if (statement?.Name?.SchemaIdentifier?.Value == null)
                statement.Name.SchemaIdentifier.Value = "dbo";

            var identifier = statement.Name.ToSqlIdentifier();
            var schema = tree.Schemas[identifier.ToSchemaIdentifier()];

            var tableType = new TableTypeModel(schema)
            {
                Identifier = identifier,
                Description = string.Empty,
                Fragment = statement
            };

            tableType.Columns = statement.Definition.GetColumns(tableType, generator);

            tree.Add(tableType);
            schema.TableTypes.Add(tableType);
        }
    }
}
