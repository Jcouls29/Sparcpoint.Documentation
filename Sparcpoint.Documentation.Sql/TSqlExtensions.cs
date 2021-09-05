using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
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
                    Description = def.ScriptTokenStream.GetDescription(def.FirstTokenIndex),
                    DataType = generator.Generate(def.DataType),
                    IsNullable = def.IsNullable(),
                    IsPrimaryKey = def.IsPrimaryKey(),
                    Identity = def.GetIdentity(),
                    DefaultValue = generator.Generate(def.DefaultConstraint?.Expression)
                };
            }).ToArray();
        }

        public static string GetDescription(this IList<TSqlParserToken> tokens, int statementStart)
        {
            const string OPEN_TAG = "@Description";
            const string CLOSE_TAG = "@EndDescription";

            if (statementStart <= 0)
                return string.Empty;

            string comment = string.Empty;
            for(int i = statementStart - 1; i > 0; i--)
            {
                if (tokens[i].TokenType == TSqlTokenType.WhiteSpace)
                    continue;

                if (tokens[i].TokenType == TSqlTokenType.MultilineComment)
                    comment = tokens[i].GetMultilineComment() + '\n' + comment;
                else if (tokens[i].TokenType == TSqlTokenType.SingleLineComment)
                    comment = tokens[i].GetSingleLineComment() + '\n' + comment;
                else
                    break;
            }

            if (string.IsNullOrWhiteSpace(comment))
                return string.Empty;

            // Let's focus on finding the @Description token
            comment = comment.Trim();

            int openTag = comment.IndexOf(OPEN_TAG, StringComparison.OrdinalIgnoreCase);
            if (openTag == -1)
                return string.Empty;

            int firstReturn = comment.IndexOf("\n", openTag);
            int closeTag = comment.IndexOf(CLOSE_TAG, StringComparison.OrdinalIgnoreCase);

            int startIndex = openTag + OPEN_TAG.Length;
            string description = string.Empty;
            if (closeTag == -1)
            {
                // We will pull out a single line description
                if (firstReturn > -1)
                     description = comment.Substring(startIndex, firstReturn - startIndex);
                else
                    description = comment.Substring(startIndex);
            } else
            {
                description = comment.Substring(startIndex, closeTag - startIndex);
            }

            description = description.Trim();

            return description;
        }

        public static string GetMultilineComment(this TSqlParserToken token)
        {
            if (token.TokenType != TSqlTokenType.MultilineComment)
                throw new ArgumentException("Not a multiline comment.");

            return token.Text.Trim('/', '*', ' ');
        }

        public static string GetSingleLineComment(this TSqlParserToken token)
        {
            if (token.TokenType != TSqlTokenType.SingleLineComment)
                throw new ArgumentException("Not a single line comment.");

            return token.Text.TrimStart('-', ' ').Trim();
        }
    }
}
