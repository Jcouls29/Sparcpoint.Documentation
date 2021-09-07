using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateStoredProcedureStatementHandler : SchemaBasedStatementHandler<CreateProcedureStatement, StoredProcedureModel>
    {
        protected override StoredProcedureModel CreateModel(SqlIdentifier identifier, SchemaModel schema)
            => new StoredProcedureModel(schema);

        protected override IList<StoredProcedureModel> GetSchemaModelCollection(SchemaModel schema)
            => schema.StoredProcedures;

        protected override SchemaObjectName GetSchemaObjectName(CreateProcedureStatement statement)
            => statement.ProcedureReference.Name;
    }
}
