using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateSequenceStatementHandler : SchemaBasedStatementHandler<CreateSequenceStatement, SequenceModel>
    {
        protected override SequenceModel CreateModel(SqlIdentifier identifier, SchemaModel schema)
            => new SequenceModel(schema);

        protected override IList<SequenceModel> GetSchemaModelCollection(SchemaModel schema)
            => schema.Sequences;

        protected override SchemaObjectName GetSchemaObjectName(CreateSequenceStatement statement)
            => statement.Name;
    }
}
