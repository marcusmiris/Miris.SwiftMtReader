using Miris.SwiftMtReader.Model.FileBlocks;
using System.Collections.Generic;
using System.Linq;

namespace Miris.SwiftMtReader.Model
{
    public static class SwiftMtMessageExtensions
    {

        public static FileBlock4 GetBlock4(this ISwiftMtMessage msg) 
            => msg.Blocks.OfType<FileBlock4>().SingleOrDefault();

        public static List<TextBlock> GetTextBlocks(
            this ISetOfTags msg,
            string textBlockPath)
        {
            IEnumerable<ISetOfTags> tagSets = new[] { msg };

            tagSets = textBlockPath.Split('\\')
                .Aggregate(tagSets, (input, textBlockName) =>
                    from set in input
                    from textBlock in set.Tags.OfType<TextBlock>()
                    where textBlock.Link == textBlockName
                    select textBlock
                );

            return tagSets.Cast<TextBlock>().ToList();
        }

    }
}
