using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateStoredProcedureStatementHandler : ISqlServerStatementHandler<CreateProcedureStatement>
    {
        public async Task HandleAsync(CreateProcedureStatement statement, ISqlTree tree, SqlScriptGenerator generator)
        {
            if (statement?.ProcedureReference?.Name?.SchemaIdentifier?.Value == null)
                statement.ProcedureReference.Name.SchemaIdentifier.Value = "dbo";

            var identifier = statement.ProcedureReference.Name.ToSqlIdentifier();
            var schema = tree.Schemas[identifier.ToSchemaIdentifier()];

            var procedure = new StoredProcedureModel(schema)
            {
                Identifier = identifier,
                Description = statement.GetDescription(),
                Fragment = statement,
                CreateStatement = generator.Generate(statement)
            };

            tree.Add(procedure);
            schema.StoredProcedures.Add(procedure);
        }
    }
}
