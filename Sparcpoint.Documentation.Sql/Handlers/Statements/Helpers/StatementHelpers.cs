using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Sparcpoint.Documentation.Sql
{
    internal static class StatementHelpers
    {
        public static T FillModel<T>(T model, SqlIdentifier identifier, TSqlStatement statement, SqlScriptGenerator generator)
            where T : SqlModelBase
        {
            model.Identifier = identifier;
            model.Description = statement.GetDescription();
            model.Fragment = statement;
            model.CreateStatement = generator.Generate(statement);

            return model;
        }

        public static (SqlIdentifier identifier, SchemaModel schema) GetIdentifierDetails(this ISqlTree tree, SchemaObjectName name)
        {
            if (name.SchemaIdentifier?.Value == null)
                name.SchemaIdentifier.Value = "dbo";

            var identifier = name.ToSqlIdentifier();
            var schema = tree.Schemas[identifier.ToSchemaIdentifier()];

            return (identifier, schema);
        }
    }
}
