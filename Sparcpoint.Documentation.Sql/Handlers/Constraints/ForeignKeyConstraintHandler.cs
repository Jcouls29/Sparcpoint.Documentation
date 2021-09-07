using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Sparcpoint.Documentation.Sql
{
    public class ForeignKeyConstraintHandler : ISqlServerConstraintHandler<ForeignKeyConstraintDefinition>
    {
        public void Handle(TableModel table, ForeignKeyConstraintDefinition constraint, ISqlTree tree, SqlScriptGenerator generator)
        {
            var fk = new ForeignKeyReference
            {
                Name = constraint.ConstraintIdentifier,
                Description = constraint.GetDescription(),
                Fragment = constraint,
                DeleteAction = constraint.DeleteAction,
                UpdateAction = constraint.UpdateAction
            };
            fk.TargetTable = tree.Tables[constraint.ReferenceTableName.ToSqlIdentifier()];

            // Local Columns
            List<TableColumnModel> localColumns = new List<TableColumnModel>();
            foreach(var column in constraint.Columns)
            {
                localColumns.Add(table.GetColumn(generator.Generate(column)));
            }
            fk.LocalColumns = new ColumnList(localColumns);

            // Foreign / Target Columns
            List<TableColumnModel> targetColumns = new List<TableColumnModel>();
            foreach (var column in constraint.ReferencedTableColumns)
            {
                targetColumns.Add(fk.TargetTable.GetColumn(generator.Generate(column)));
            }
            fk.ForeignColumns = new ColumnList(targetColumns);

            table.ForeignKeys.Add(fk);
        }
    }
}
