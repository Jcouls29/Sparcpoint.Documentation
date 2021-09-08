using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Linq;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateIndexStatementHandler : ISqlServerStatementHandler<CreateIndexStatement>
    {
        public void Handle(CreateIndexStatement statement, ISqlTree tree, SqlScriptGenerator generator)
        {
            TableModel table = tree.Tables[statement.OnName];
            
            TableIndexModel index = StatementHelpers.FillModel(new TableIndexModel(), statement.Name, statement, generator);
            index.Columns = statement.Columns.Select(c =>
            {
                c.Column.EnsureBracketQuotes();

                bool isAscending = c.SortOrder != SortOrder.Descending;
                string columnName = generator.Generate(c.Column);

                return new SortedColumnModel
                {
                    IsAscending = isAscending,
                    Column = table.GetColumn(columnName)
                };
            }).ToArray();

            if (statement.IncludeColumns?.Any() ?? false)
            {
                foreach (var c in statement.IncludeColumns)
                    c.EnsureBracketQuotes();

                index.IncludeColumns = new ColumnList(statement.IncludeColumns.Select(c => table.GetColumn(generator.Generate(c))));
            }

            if (statement.FilterPredicate != null)
            {
                index.Filter = generator.Generate(statement.FilterPredicate);
            }

            if (statement.Unique)
                table.UniqueIndices.Add(index);
            else
                table.Indices.Add(index);
        }
    }
}
