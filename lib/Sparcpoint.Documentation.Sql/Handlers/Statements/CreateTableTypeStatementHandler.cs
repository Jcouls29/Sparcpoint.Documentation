using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateTableTypeStatementHandler : ISqlServerStatementHandler<CreateTypeTableStatement>
    {
        public void Handle(CreateTypeTableStatement statement, ISqlTree tree, SqlScriptGenerator generator)
        {
            var (identifier, schema) = tree.GetIdentifierDetails(statement.Name);

            var tableType = StatementHelpers.FillModel(new TableTypeModel(schema), identifier, statement, generator);
            tableType.Columns = new ColumnList(statement.Definition.GetColumns(tableType, generator));

            tree.Add(tableType);
            schema.TableTypes.Add(tableType);
        }
    }
}
