using Miris.SwiftMtReader.Model;
using System.IO;

namespace Miris.SwiftMtReader.Reader
{
    public interface ISwiftMtMessageReader
    {
        ISwiftMtMessage Read(string rawMtMessage);
        ISwiftMtMessage Read(TextReader textReader);
    }
}
