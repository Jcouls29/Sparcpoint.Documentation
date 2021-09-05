using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Linq;

namespace Sparcpoint.Documentation.Sql
{
    internal static class TSqlExtensions
    {
        public static bool IsNullable(this ColumnDefinition definition)
        {
            var constraint = definition.Constraints.FirstOrDefault(c => c is NullableConstraintDefinition) as NullableConstraintDefinition;
            if (constraint != null)
                return constraint.Nullable;

            return false;
        }

        public static bool IsPrimaryKey(this ColumnDefinition definition)
        {
            var constraint = definition.Constraints.FirstOrDefault(c => c is UniqueConstraintDefinition) as UniqueConstraintDefinition;
            if (constraint != null)
                return constraint.IsPrimaryKey;

            return false;
        }

        public static IdentityColumnModel? GetIdentity(this ColumnDefinition definition)
        {
            if (definition.IdentityOptions == null)
                return null;

            return new IdentityColumnModel
            {
                Seed = long.Parse((definition.IdentityOptions.IdentitySeed as IntegerLiteral)?.Value ?? "1"),
                Increment = long.Parse((definition.IdentityOptions.IdentityIncrement as IntegerLiteral)?.Value ?? "1")
            };
        }

        public static string? Generate(this SqlScriptGenerator generator, TSqlFragment fragment)
        {
            if (fragment == null || generator == null)
                return null;

            generator.GenerateScript(fragment, out string script);
            return script;
        }

        public static SqlIdentifier ToSqlIdentifier(this Identifier name, Identifier? schema = null)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return new SqlIdentifier(name.Value, schema?.Value);
        }

        public static SqlIdentifier ToSqlIdentifier(this SchemaObjectName name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return new SqlIdentifier(name.BaseIdentifier.Value, name.SchemaIdentifier?.Value);
        }

        public static TableColumnModel[] GetColumns(this TableDefinition definition, ISqlModel parentReference, SqlScriptGenerator generator)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            if (parentReference == null)
                throw new ArgumentNullException(nameof(parentReference));

            if (generator == null)
                throw new ArgumentNullException(nameof(generator));

            return definition.ColumnDefinitions.Select(def =>
            {
                return new TableColumnModel(parentReference)
                {
                    Name = generator.Generate(def.ColumnIdentifier),
                    DataType = generator.Generate(def.DataType),
                    IsNullable = def.IsNullable(),
                    IsPrimaryKey = def.IsPrimaryKey(),
                    Identity = def.GetIdentity(),
                    DefaultValue = generator.Generate(def.DefaultConstraint?.Expression)
                };
            }).ToArray();
        }
    }
}
