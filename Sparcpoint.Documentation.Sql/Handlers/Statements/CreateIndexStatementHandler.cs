using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Linq;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateIndexStatementHandler : ISqlServerStatementHandler<CreateIndexStatement>
    {
        public async Task HandleAsync(CreateIndexStatement statement, ISqlTree tree, SqlScriptGenerator generator)
        {
            var name = statement.Name;
            var columns = statement.Columns;
            var isUnique = statement.Unique;

            TableModel table = tree.Tables[statement.OnName];

            TableIndexModel index = new TableIndexModel
            {
                Identifier = new SqlIdentifier(statement.Name.Value),
                Fragment = statement,
                Columns = statement.Columns.Select(c =>
                {
                    bool isAscending = c.SortOrder != SortOrder.Descending;
                    string columnName = generator.Generate(c.Column);

                    return new SortedColumnModel
                    {
                        IsAscending = isAscending,
                        Column = table.GetColumn(columnName)
                    };
                }).ToArray(),
            };

            if (statement.IncludeColumns?.Any() ?? false)
            {
                index.IncludeColumns = new ColumnList(statement.IncludeColumns.Select(c => table.GetColumn(generator.Generate(c))));
            }

            if (statement.FilterPredicate != null)
            {
                index.Filter = generator.Generate(statement.FilterPredicate);
            }

            index.CreateStatement = generator.Generate(statement);

            if (isUnique)
                table.UniqueIndices.Add(index);
            else
                table.Indices.Add(index);
        }
    }
}
