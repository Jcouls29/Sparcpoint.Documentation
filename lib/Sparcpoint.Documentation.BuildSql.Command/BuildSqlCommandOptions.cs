namespace Sparcpoint.Documentation.BuildSql
{
    public class BuildSqlCommandOptions
    {
        public string Template { get; set; }
        public bool NoIndexPage { get; set; } = false;
        public bool Verbose { get; set; } = false;
        public string Path { get; set; }
        public string Output { get; set; }
    }
}

