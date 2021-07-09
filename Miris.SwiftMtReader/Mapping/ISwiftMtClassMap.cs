using System.Collections.Generic;

namespace Miris.SwiftMtReader.Mapping
{
    public interface ISwiftMtClassMap
    {
        IList<SwiftMtPropertyMap> MappedProperties { get; }
        IList<ISwiftMtCollectionMap> MappedCollections { get; }
    }
}
