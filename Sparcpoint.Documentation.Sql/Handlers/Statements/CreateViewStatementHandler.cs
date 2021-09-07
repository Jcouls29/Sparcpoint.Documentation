using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateViewStatementHandler : SchemaBasedStatementHandler<CreateViewStatement, ViewModel>
    {
        protected override ViewModel CreateModel(SqlIdentifier identifier, SchemaModel schema)
            => new ViewModel(schema);

        protected override IList<ViewModel> GetSchemaModelCollection(SchemaModel schema)
            => schema.Views;

        protected override SchemaObjectName GetSchemaObjectName(CreateViewStatement statement)
            => statement.SchemaObjectName;
    }
}
