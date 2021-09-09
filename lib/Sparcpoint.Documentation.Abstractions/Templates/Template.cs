using System.IO;

namespace Sparcpoint.Documentation.Abstractions
{
    public class Template
    {
        public string Value { get; set; }
        public string Source { get; set; }

        public string FileExtension
        {
            get
            {
                if (Source != null)
                    return System.IO.Path.GetExtension(Source) ?? ".txt";

                return ".txt";
            }
        }
    }
}
