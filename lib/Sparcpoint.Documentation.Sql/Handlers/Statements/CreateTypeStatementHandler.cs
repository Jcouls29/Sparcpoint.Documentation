using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class CreateTypeStatementHandler : SchemaBasedStatementHandler<CreateTypeUddtStatement, DataTypeModel>
    {
        protected override DataTypeModel CreateModel(SqlIdentifier identifier, SchemaModel schema)
            => new DataTypeModel(schema);

        protected override IList<DataTypeModel> GetSchemaModelCollection(SchemaModel schema)
            => schema.DataTypes;

        protected override SchemaObjectName GetSchemaObjectName(CreateTypeUddtStatement statement)
            => statement.Name;
    }
}
