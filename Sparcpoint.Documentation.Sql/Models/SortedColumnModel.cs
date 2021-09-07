namespace Sparcpoint.Documentation.Sql
{
    public class SortedColumnModel
    {
        public TableColumnModel? Column { get; set; }
        public bool IsAscending { get; set; } = true;

        public override string ToString()
            => $"{Column} {(IsAscending ? "ASC" : "DESC")}";
    }
}
