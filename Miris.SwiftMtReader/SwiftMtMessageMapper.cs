using Miris.SwiftMtReader.Mapping;
using Miris.SwiftMtReader.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Miris.SwiftMtReader
{

    public class SwiftMtMessageParser<TMsg>
        : ISwiftMtMessageMapper<TMsg>
        where TMsg : new()
    {
        private readonly SwiftMtClassMap<TMsg> mapping;


        #region ctor 

        public SwiftMtMessageParser(
            SwiftMtClassMap<TMsg> mapping)
        {
            this.mapping = mapping;
        }

        #endregion

        #region ISwiftMtMessageMapper<TMsg>

        public TMsg MapFrom(ISwiftMtMessage mtMessage)
            => (TMsg)MapFrom(
                mtMessage.GetBlock4(),
                typeof(TMsg),
                mapping);

        #endregion

        #region ' internals '

        private object MapFrom(ISetOfTags mtMessage, Type objectType, ISwiftMtClassMap classMap)
        {
            var obj = Activator.CreateInstance(objectType);

            foreach (var propMap in classMap.MappedProperties)
            {
                // get value
                var propertyValue = mtMessage.TryGetField(
                    propMap.SourceInfo.Tag,
                    propMap.SourceInfo.Qualifier,
                    propMap.SourceInfo.BlockPath);

                if (propMap.Transformer != null)
                {
                    propertyValue = propMap.Transformer(propertyValue);
                }

                // set value
                propMap.PropertyInfo.SetValue(obj, propertyValue);
            }

            foreach (var propMap in classMap.MappedCollections)
            {
                // instantiate collection
                var list = (ICollection)propMap.PropertyInfo.GetValue(obj);
                if (list == null)
                {
                    list = InstantiateCollection(propMap.PropertyInfo);
                    propMap.PropertyInfo.SetValue(obj, list);
                }

                // read mt blocks
                var mtBlocks = mtMessage.GetTextBlocks(propMap.BlockPath);

                // parse each block
                var itemType = list.GetType().GetGenericArguments()[0];
                var add = typeof(ICollection<>).MakeGenericType(itemType).GetMethod("Add");
                foreach (var block in mtBlocks)
                {
                    var parsedItem = MapFrom(block, itemType, propMap.ItemMap);
                    add.Invoke(list, new[] { parsedItem });
                }
            }

            return obj;
        }

        private ICollection InstantiateCollection(PropertyInfo pi)
        {
            var isGeneric = pi.PropertyType.GetGenericArguments().Length > 0;
            if (isGeneric)
            {
                Type type = pi.PropertyType.GetGenericArguments()[0];
                Type listType = typeof(List<>).MakeGenericType(type);

                return (ICollection)Activator.CreateInstance(listType);
            }
            else
            {
                throw new NotImplementedException("No support for non-generic collections");
            }
        }

        #endregion

    }
}
