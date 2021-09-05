using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;
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
                Identifier = statement.SchemaObjectName.ToSqlIdentifier(),
                Description = string.Empty,
                Fragment = statement
            };

            var columns = GetColumns(table, statement.Definition.ColumnDefinitions, generator);
            table.Columns = columns;

            // TODO: Table Indexes

            // TODO: Table Constraints
            if (statement.Definition.TableConstraints?.Any() ?? false)
            {
                foreach(var constraint in statement.Definition.TableConstraints)
                {
                    tree.DeferConstraint(constraint);
                }
            }

            tree.Add(table);
            schema.Tables.Add(table);
        }

        private TableColumnModel[] GetColumns(TableModel table, IList<ColumnDefinition> definitions, SqlScriptGenerator generator)
        {
            return definitions.Select(def =>
            {
                return new TableColumnModel(table)
                {
                    Name = generator.Generate(def.ColumnIdentifier),
                    DataType = generator.Generate(def.DataType),
                    IsNullable = def.IsNullable(),
                    IsPrimaryKey = def.IsPrimaryKey(),
                    Identity = def.GetIdentity(),
                    DefaultValue = generator.Generate(def.DefaultConstraint?.Expression)
                };
            }).ToArray();
        }
    }
}
