using Miris.SwiftMtReader.Model.FileBlocks;
using System.Collections.Generic;

namespace Miris.SwiftMtReader.Model
{
    public class SwiftMtMessage
        : ISwiftMtMessage
        , ISetOfTags
    {
        public IList<IFileBlock> Blocks { get; } = new List<IFileBlock>();

        List<ITag> ISetOfTags.Tags => this.GetBlock4().Tags;
    }
}
