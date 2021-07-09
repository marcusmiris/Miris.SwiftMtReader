using System.Collections.Generic;

namespace Miris.SwiftMtReader.Model.FileBlocks
{
    public class FileBlock4
        : IFileBlock
        , ISetOfTags
    {
        public string Name { get; } = "4";

        public List<ITag> Tags { get; set; } = new List<ITag>();

        public string Outer => throw new System.NotImplementedException();
    }
}
