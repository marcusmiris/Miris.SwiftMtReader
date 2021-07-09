using System.Reflection;

namespace Miris.SwiftMtReader.Mapping
{
    public interface ISwiftMtCollectionMap
    {
        PropertyInfo PropertyInfo { get; }

        string BlockPath { get; }

        ISwiftMtClassMap ItemMap { get; }
    }
}
