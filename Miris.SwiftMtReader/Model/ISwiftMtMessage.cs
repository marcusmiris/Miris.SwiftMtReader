using Miris.SwiftMtReader.Model.FileBlocks;
using System.Collections.Generic;

namespace Miris.SwiftMtReader.Model
{
    public interface ISwiftMtMessage
    {
        IList<IFileBlock> Blocks { get; }
    }
}
