using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateFunctionStatementHandler : SchemaBasedStatementHandler<CreateFunctionStatement, FunctionModel>
    {
        protected override FunctionModel CreateModel(SqlIdentifier identifier, SchemaModel schema)
            => new FunctionModel(schema);

        protected override IList<FunctionModel> GetSchemaModelCollection(SchemaModel schema)
            => schema.Functions;

        protected override SchemaObjectName GetSchemaObjectName(CreateFunctionStatement statement)
            => statement.Name;
    }
}
