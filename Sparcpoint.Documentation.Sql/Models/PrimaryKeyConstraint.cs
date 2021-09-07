namespace Sparcpoint.Documentation.Sql
{
    public class PrimaryKeyConstraint
    {
        public SqlIdentifier Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public ColumnList Columns { get; set; } = new ColumnList();
    }
}
