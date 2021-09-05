namespace Sparcpoint.Documentation.Sql
{
    public class PrimaryKeyConstraint
    {
        public SqlIdentifier Name { get; set; }
        public ColumnList Columns { get; set; }
    }
}
