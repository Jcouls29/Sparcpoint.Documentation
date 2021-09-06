using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateSequenceStatementHandler : ISqlServerStatementHandler<CreateSequenceStatement>
    {
        public async Task HandleAsync(CreateSequenceStatement statement, ISqlTree tree, SqlScriptGenerator generator)
        {
            if (statement?.Name?.SchemaIdentifier?.Value == null)
                statement.Name.SchemaIdentifier.Value = "dbo";

            var identifier = statement.Name.ToSqlIdentifier();
            var schema = tree.Schemas[identifier.ToSchemaIdentifier()];

            var sequence = new SequenceModel(schema)
            {
                Identifier = identifier,
                Description = statement.GetDescription(),
                Fragment = statement,
                CreateStatement = generator.Generate(statement)
            };

            tree.Add(sequence);
            schema.Sequences.Add(sequence);
        }
    }
}
