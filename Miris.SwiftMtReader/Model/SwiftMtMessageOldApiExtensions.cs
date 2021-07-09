using System;
using System.Collections.Generic;
using System.Linq;

namespace Miris.SwiftMtReader.Model
{
    public static class SwiftMtMessageOldApiExtensions
    {
        public static string GetMessageType(
            this ISwiftMtMessage mtMessage)
        {
            var appHeaderBlock = GetFileBlock(mtMessage, 2);

            if (string.IsNullOrWhiteSpace(appHeaderBlock))
            {
                throw new ArgumentException(@"Invalid MT message application header block", nameof(mtMessage));
            }

            return appHeaderBlock.Substring(4, 3);
        }

        public static string GetFileBlock(
            this ISwiftMtMessage msg,
            int blockNumber)
        {
            var blockName = Convert.ToString(blockNumber);
            var block = msg.Blocks.Single(b => b.Name == blockName);
            return block.Outer;
        }

        #region ' GetFields(...) '

        public static List<string> GetFields(
            this ISwiftMtMessage msg,
            string tagName,
            string qualifier = null)
            => GetFields(msg.GetBlock4(), tagName, qualifier);

        public static List<string> GetFields(
            this ISetOfTags msg,
            string tagName,
            string qualifier = null)
        {
            return (
                from tag in msg.GetChildren()
                where tag.TagName == tagName
                where qualifier == null || tag.Qualifier == qualifier
                select tag.RawBody
                ).ToList();
        }

        public static List<string> GetFields(
            this ISwiftMtMessage msg,
            string tagName,
            string qualifier,
            string textBlockPath)
            => GetFields(msg.GetBlock4(), tagName, qualifier, textBlockPath);

        public static List<string> GetFields(
            this ISetOfTags msg,
            string tagName,
            string qualifier,
            string textBlockPath)
        {
            IEnumerable<ISetOfTags> tagSets = new[] { msg };
            
            if (textBlockPath != null)
            {
                tagSets = textBlockPath.Split('\\')
                    .Aggregate(tagSets, (input, blockName) 
                    => from set in input
                       from textBlock in set.Tags.OfType<TextBlock>()
                       where textBlock.Link == blockName
                       select textBlock);
            }

            return (
                from set in tagSets
                from tag in set.Tags
                where tag.TagName == tagName
                where qualifier == null || tag.Qualifier == qualifier
                select tag.RawBody
                ).ToList();
            
        }

        #endregion

        public static string TryGetField(
            this ISetOfTags msg,
            string tag,
            string qualifier = null,
            string blockPath = null)
        {
            var fields = GetFields(msg, tag, qualifier, blockPath);

            switch (fields.Count)
            {
                case 0:
                    return null;

                case 1:
                    return fields.Single();

                default:
                    throw new Exception($"More than one tag found: {tag}");
            }
        }

        #region ' GetField(...) '

        public static string GetField(
            this ISwiftMtMessage msg,
            string tag,
            string qualifier = null,
            string blockPath = null)
            => GetField(msg.GetBlock4(), tag, qualifier, blockPath);

        public static string GetField(
            this ISetOfTags msg,
            string tag,
            string qualifier = null,
            string blockPath = null)
        {
            var fields = GetFields(msg, tag, qualifier, blockPath);

            switch (fields.Count)
            {
                case 0: 
                    throw new Exception($"field not found: { tag }");
                case 1:
                    return fields.Single();

                default: 
                    throw new Exception($"More than one tag found: {tag}");
            }
        }

        #endregion

    }
}
