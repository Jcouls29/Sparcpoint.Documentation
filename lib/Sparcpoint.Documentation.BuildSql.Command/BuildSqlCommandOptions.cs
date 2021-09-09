using System.Collections.Generic;

namespace Sparcpoint.Documentation.BuildSql
{
    public class BuildSqlCommandOptions
    {
        public string Template { get; set; }
        public bool NoIndexPage { get; set; } = false;
        public bool Verbose { get; set; } = false;
        public IEnumerable<string> Input { get; set; }
        public string Output { get; set; }
    }
}

