using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Linq;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateTableStatementHandler : ISqlServerStatementHandler<CreateTableStatement>
    {
        public async Task HandleAsync(CreateTableStatement statement, ISqlTree tree, SqlScriptGenerator generator)
        {
            if (statement?.SchemaObjectName?.SchemaIdentifier?.Value == null)
                statement.SchemaObjectName.SchemaIdentifier.Value = "dbo";

            var identifier = statement.SchemaObjectName.ToSqlIdentifier();
            var schema = tree.Schemas[identifier.ToSchemaIdentifier()];

            var table = new TableModel(schema)
            {
                Identifier = identifier,
                Description = statement.GetDescription(),
                Fragment = statement,
                CreateStatement = generator.Generate(statement),
            };

            table.Columns = statement.Definition.GetColumns(table, generator);

            if (statement.Definition.TableConstraints?.Any() ?? false)
            {
                foreach(var constraint in statement.Definition.TableConstraints)
                {
                    tree.DeferConstraint(table, constraint);
                }
            }

            tree.Add(table);
            schema.Tables.Add(table);
        }
    }
}
