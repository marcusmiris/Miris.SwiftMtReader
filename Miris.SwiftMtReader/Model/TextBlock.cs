using System.Collections.Generic;
using System.Diagnostics;

namespace Miris.SwiftMtReader.Model
{
    [DebuggerDisplay("Tag='{TagName}'; Link='{Link}'")]
    public class TextBlock
        : ITag
        , ISetOfTags
    {
        string ITag.TagName => "16R";

        string ITag.Qualifier => null;

        string ITag.RawBody => Link;

        public string Link { get; set; }

        public List<ITag> Tags { get; set; } = new List<ITag>();



    }
}
