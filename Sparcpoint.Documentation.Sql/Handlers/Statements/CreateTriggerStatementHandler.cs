using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Linq;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateTriggerStatementHandler : ISqlServerStatementHandler<CreateTriggerStatement>
    {
        public async Task HandleAsync(CreateTriggerStatement statement, ISqlTree tree, SqlScriptGenerator generator)
        {
            if (statement?.Name?.SchemaIdentifier?.Value == null)
                statement.Name.SchemaIdentifier.Value = "dbo";

            var identifier = statement.Name.ToSqlIdentifier();
            var schema = tree.Schemas[identifier.ToSchemaIdentifier()];

            var trigger = new TriggerModel(schema)
            {
                Identifier = identifier,
                CreateStatement = generator.Generate(statement),
                Fragment = statement,
                Description = statement.GetDescription(),
                IsUpdate = statement.TriggerActions.Any(act => act.TriggerActionType == TriggerActionType.Update),
                IsInsert = statement.TriggerActions.Any(act => act.TriggerActionType == TriggerActionType.Insert),
                IsDelete = statement.TriggerActions.Any(act => act.TriggerActionType == TriggerActionType.Delete),
            };

            var targetId = statement.TriggerObject.Name.ToSqlIdentifier();
            TableModel targetTable = tree.Tables[targetId];
            if (targetTable != null)
            {
                trigger.Target = targetTable;
                targetTable.Triggers.Add(trigger);
            } else
            {
                ViewModel targetView = tree.Views[targetId];

                if (targetView == null)
                    throw new System.Exception($"Target not found '{targetId}'");

                trigger.Target = targetView;
                targetView.Triggers.Add(trigger);
            }
        }
    }
}
