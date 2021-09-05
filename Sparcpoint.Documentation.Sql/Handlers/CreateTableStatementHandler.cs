using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateTableStatementHandler : ISqlServerStatementHandler<CreateTableStatement>
    {
        public async Task HandleAsync(string source, CreateTableStatement statement, ISqlTree tree, SqlScriptGenerator generator)
        {
            if (statement?.SchemaObjectName?.SchemaIdentifier?.Value == null)
                statement.SchemaObjectName.SchemaIdentifier.Value = "dbo";

            var table = new TableModel
            {
                Identifier = statement.SchemaObjectName.ToSqlIdentifier(),
                Description = string.Empty,
                Fragment = statement,
                Columns = GetColumns(statement.Definition.ColumnDefinitions, generator)
            };

            // TODO: Table Indexes

            // TODO: Table Constraints

            tree.Add(table);
        }

        private TableColumnModel[] GetColumns(IList<ColumnDefinition> definitions, SqlScriptGenerator generator)
        {
            return definitions.Select(def =>
            {
                return new TableColumnModel
                {
                    Name = def.ColumnIdentifier.Value,
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
