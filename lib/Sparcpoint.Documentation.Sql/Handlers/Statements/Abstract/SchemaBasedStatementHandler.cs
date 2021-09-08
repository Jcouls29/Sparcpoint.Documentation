using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;

namespace Sparcpoint.Documentation.Sql
{
    public abstract class SchemaBasedStatementHandler<TStatement, TModel> : ISqlServerStatementHandler<TStatement>
        where TStatement : TSqlStatement
        where TModel : SqlModelBase
    {
        protected abstract SchemaObjectName GetSchemaObjectName(TStatement statement);
        protected abstract TModel CreateModel(SqlIdentifier identifier, SchemaModel schema);
        protected abstract IList<TModel> GetSchemaModelCollection(SchemaModel schema);

        public void Handle(TStatement statement, ISqlTree tree, SqlScriptGenerator generator)
        {
            if (statement == null)
                throw new ArgumentNullException(nameof(statement));

            if (tree == null)
                throw new ArgumentNullException(nameof(tree));

            if (generator == null)
                throw new ArgumentNullException(nameof(generator));

            var (identifier, schema) = tree.GetIdentifierDetails(GetSchemaObjectName(statement));
            var model = StatementHelpers.FillModel(CreateModel(identifier, schema), identifier, statement, generator);

            tree.Add(model);
            GetSchemaModelCollection(schema).Add(model);
        }
    }
}
