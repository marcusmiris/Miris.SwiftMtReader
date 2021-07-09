using System.Diagnostics;

namespace Miris.SwiftMtReader.Model
{
    [DebuggerDisplay("Tag='{TagName}'; Qualifier='{Qualifier}'")]
    public class Tag
        : ITag
    {
        public string TagName { get; set; }
        public string Qualifier { get; set; }
        public string RawBody { get; set; }
    }
}
