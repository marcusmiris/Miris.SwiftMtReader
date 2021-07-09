using System.Collections.Generic;

namespace Miris.SwiftMtReader.Model
{
    public static class SetOfTagsExtensions
    {
        public static IEnumerable<ITag> GetChildren(
            this ISetOfTags setOfTags)
        {
            foreach (var tag in setOfTags.Tags)
            {
                if (tag is ISetOfTags subSet)
                {
                    foreach (var subTag in GetChildren(subSet)) yield return subTag;
                }
                else
                {
                    yield return tag;
                }
            }
        }
    }
}
