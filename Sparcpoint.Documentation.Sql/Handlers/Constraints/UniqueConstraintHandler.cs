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
                Name = constraint.ConstraintIdentifier.ToSqlIdentifier()
            };

            var columns = new List<TableColumnModel>();
            foreach (var column in constraint.Columns)
                columns.Add(table.GetColumn(generator.Generate(column)));

            primaryKey.Columns = new ColumnList(columns);
            table.PrimaryKeyConstraint = primaryKey;
        }

        private void HandleOther(TableModel table, UniqueConstraintDefinition constraint, ISqlTree tree, SqlScriptGenerator generator)
        {
            throw new NotImplementedException();
        }
    }
}
