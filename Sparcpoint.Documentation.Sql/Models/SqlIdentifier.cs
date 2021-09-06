using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;

namespace Sparcpoint.Documentation.Sql
{
    public struct SqlIdentifier : IEquatable<SqlIdentifier>
    {
        public SqlIdentifier(string name, string? schema = null)
        {
            Schema = schema;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string? Schema { get; set; }
        public string Name { get; set; }

        public SqlIdentifier ToSchemaIdentifier()
            => new SqlIdentifier(Schema ?? throw new InvalidOperationException("Schema is not defined."));

        public bool Equals(SqlIdentifier other)
            => string.Equals(Schema, other.Schema) && string.Equals(Name, other.Name);

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is SqlIdentifier))
                return false;

            return Equals((SqlIdentifier)obj);
        }

        public override int GetHashCode()
            => HashCode.Combine(Schema, Name);

        public string SchemaString
            => $"[{Schema ?? "dbo"}]";

        public string NameString
            => $"[{Name}]";

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Schema))
                return $"[{Name}]";

            return $"[{Schema ?? "dbo"}].[{Name}]";
        }

        public static bool operator ==(SqlIdentifier left, SqlIdentifier right)
            => left.Equals(right);

        public static bool operator !=(SqlIdentifier left, SqlIdentifier right)
            => !right.Equals(right);

        public static implicit operator SqlIdentifier(SchemaObjectName value)
            => value.ToSqlIdentifier();

        public static implicit operator SqlIdentifier(Identifier value)
            => value.ToSqlIdentifier();
    }
}
