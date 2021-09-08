using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Linq;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateTriggerStatementHandler : ISqlServerStatementHandler<CreateTriggerStatement>
    {
        public void Handle(CreateTriggerStatement statement, ISqlTree tree, SqlScriptGenerator generator)
        {
            var (identifier, schema) = tree.GetIdentifierDetails(statement.Name);

            var trigger = StatementHelpers.FillModel(new TriggerModel(schema)
            {
                IsUpdate = statement.TriggerActions.Any(act => act.TriggerActionType == TriggerActionType.Update),
                IsInsert = statement.TriggerActions.Any(act => act.TriggerActionType == TriggerActionType.Insert),
                IsDelete = statement.TriggerActions.Any(act => act.TriggerActionType == TriggerActionType.Delete),
            }, identifier, statement, generator);

            var targetId = statement.TriggerObject.Name.EnsureBracketQuotes().ToSqlIdentifier();
            TableModel targetTable = tree.Tables[targetId];
            if (targetTable != null)
            {
                trigger.Target = targetTable;
                targetTable.Triggers.Add(trigger);
            } else
            {
                ViewModel targetView = tree.Views[targetId];

                if (targetView == null)
                    throw new System.Exception($"Target not found '{targetId}' for trigger '{identifier}'");

                trigger.Target = targetView;
                targetView.Triggers.Add(trigger);
            }
        }
    }
}
