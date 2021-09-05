namespace Sparcpoint.Documentation.Sql
{
    public class TableColumnModel
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public bool IsPrimaryKey { get; set; }
        public string? DefaultValue { get; set; }

        public IdentityColumnModel? Identity { get; set; }
    }
}
