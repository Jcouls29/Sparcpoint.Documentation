namespace Sparcpoint.Documentation.Sql
{
    public class IdentityColumnModel
    {
        public long Seed { get; set; }
        public long Increment { get; set; }

        public override string ToString()
            => $"IDENTITY({Seed}, {Increment})";
    }
}
