using System;
using System.Reflection;

namespace Miris.SwiftMtReader.Mapping
{
    public class SwiftMtPropertyMap
    {
        internal PropertyInfo PropertyInfo { get; }

        internal (string Tag, string Qualifier, string BlockPath) SourceInfo { get; private set; }

        internal Func<string, string> Transformer { get; private set; }


        #region ctor 

        internal SwiftMtPropertyMap(
            PropertyInfo propertyInfo,
            (string Tag, string Qualifier, string BlockPath) sourceInfo,
            Func<string, string> transformer)
        {
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

            PropertyInfo = propertyInfo;
            SourceInfo = sourceInfo;
            Transformer = transformer;
        }

        #endregion

    }
}
