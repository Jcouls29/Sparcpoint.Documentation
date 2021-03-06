using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Linq;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateTableStatementHandler : ISqlServerStatementHandler<CreateTableStatement>
    {
        public void Handle(CreateTableStatement statement, ISqlTree tree, SqlScriptGenerator generator)
        {
            var (identifier, schema) = tree.GetIdentifierDetails(statement.SchemaObjectName);

            var table = StatementHelpers.FillModel(new TableModel(schema), identifier, statement, generator);
            table.Columns = new ColumnList(statement.Definition.GetColumns(table, generator));

            if (statement.Definition.TableConstraints?.Any() ?? false)
            {
                foreach(var constraint in statement.Definition.TableConstraints)
                {
                    NormalizeConstraint(constraint);
                    tree.DeferConstraint(table, constraint);
                }
            }

            tree.Add(table);
            schema.Tables.Add(table);
        }

        private void NormalizeConstraint(ConstraintDefinition definition)
        {
            if (definition is ForeignKeyConstraintDefinition fk)
            {
                fk.Columns.EnsureBracketQuotes();
                fk.ReferencedTableColumns.EnsureBracketQuotes();
            } else if (definition is UniqueConstraintDefinition uq)
            {
                uq.ConstraintIdentifier.EnsureBracketQuotes();

                foreach(var c in uq.Columns)
                {
                    c.Column.EnsureBracketQuotes();
                }
            }
        }
    }
}
