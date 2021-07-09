using System;
using System.Reflection;

namespace Miris.SwiftMtReader.Mapping
{
    public class SwiftMtCollectionMap<TItem>
        : ISwiftMtCollectionMap
    {
        public SwiftMtCollectionMap(PropertyInfo propertyInfo, string blockPath, SwiftMtClassMap<TItem> itemMap)
        {
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));
            if (itemMap == null) throw new ArgumentNullException(nameof(itemMap));

            PropertyInfo = propertyInfo;
            BlockPath = blockPath;
            ItemMap = itemMap;
        }

        public PropertyInfo PropertyInfo { get; private set; }

        public string BlockPath { get; private set; }

        public SwiftMtClassMap<TItem> ItemMap { get; private set; }

        ISwiftMtClassMap ISwiftMtCollectionMap.ItemMap => ItemMap;
    }
}
