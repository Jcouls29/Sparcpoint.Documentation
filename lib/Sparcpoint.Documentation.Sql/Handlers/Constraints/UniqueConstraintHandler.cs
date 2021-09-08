using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;

namespace Sparcpoint.Documentation.Sql
{
    public class UniqueConstraintHandler : ISqlServerConstraintHandler<UniqueConstraintDefinition>
    {
        public void Handle(TableModel table, UniqueConstraintDefinition constraint, ISqlTree tree, SqlScriptGenerator generator)
        {
            if (constraint.IsPrimaryKey)
                HandlePrimaryKey(table, constraint, tree, generator);
            else
                HandleOther(table, constraint, tree, generator);
        }

        private void HandlePrimaryKey(TableModel table, UniqueConstraintDefinition constraint, ISqlTree tree, SqlScriptGenerator generator)
        {
            var primaryKey = new PrimaryKeyConstraint
            {
                Name = constraint.ConstraintIdentifier.ToSqlIdentifier(),
                Description = constraint.GetDescription(),
            };

            var columns = new List<TableColumnModel>();
            foreach (var column in constraint.Columns)
            {
                var foundColumn = table.GetColumn(generator.Generate(column));
                foundColumn.IsPrimaryKey = true;

                columns.Add(foundColumn);
            }

            primaryKey.Columns = new ColumnList(columns);
            table.PrimaryKeyConstraint = primaryKey;
        }

        private void HandleOther(TableModel table, UniqueConstraintDefinition constraint, ISqlTree tree, SqlScriptGenerator generator)
        {
            throw new NotImplementedException();
        }
    }
}
