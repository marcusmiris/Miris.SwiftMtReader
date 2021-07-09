using Miris.SwiftMtReader.Model;

namespace Miris.SwiftMtReader
{
    public interface ISwiftMtMessageMapper<out TMsg>
    {
        TMsg MapFrom(ISwiftMtMessage mtMessage);
    }
}
