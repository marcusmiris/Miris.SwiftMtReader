using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Miris.SwiftMtReader.Mapping
{
    public class SwiftMtClassMap<T>
        : ISwiftMtClassMap
    {
        public IList<SwiftMtPropertyMap> MappedProperties { get; } = new List<SwiftMtPropertyMap>();
        public IList<ISwiftMtCollectionMap> MappedCollections { get; } = new List<ISwiftMtCollectionMap>();


        public SwiftMtClassMap<T> Map(
            Expression<Func<T, string>> propertyExpression,
            string tag, string qualifier = null, string blockPath = null,
            Func<string, string> transformer = null)
        {
            if (!(propertyExpression.Body is MemberExpression memberExpression && memberExpression.Member is PropertyInfo propertyInfo))
            {
                throw new InvalidOperationException($"Not a valid property expression supplied: `{ propertyExpression.ToString() }`.");
            }

            var mapping = new SwiftMtPropertyMap(propertyInfo, (tag, qualifier, blockPath), transformer);

            MappedProperties.Add(mapping);

            return this;
        }


        public SwiftMtClassMap<T> Map<TItem>(
            Expression<Func<T, IEnumerable<TItem>>> propertyExpression,
            string blockPath,
            Action<SwiftMtClassMap<TItem>> mapper)
        {
            if (!(propertyExpression.Body is MemberExpression memberExpression && memberExpression.Member is PropertyInfo propertyInfo))
            {
                throw new InvalidOperationException($"Not a valid property expression supplied: `{ propertyExpression.ToString() }`.");
            }

            var itemMap = new SwiftMtClassMap<TItem>();
            mapper(itemMap);

            var mapping = new SwiftMtCollectionMap<TItem>(
                propertyInfo,
                blockPath,
                itemMap);

            MappedCollections.Add(mapping);

            return this;
        }

    }
}
