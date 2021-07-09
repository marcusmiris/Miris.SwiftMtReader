using Miris.SwiftMtReader.Exceptions;
using Miris.SwiftMtReader.Extensions;
using Miris.SwiftMtReader.Internals;
using Miris.SwiftMtReader.Model;
using Miris.SwiftMtReader.Model.FileBlocks;
using System;
using System.IO;
using System.Text;

namespace Miris.SwiftMtReader.Reader
{
    public class SwiftMtMessageReader
        : ISwiftMtMessageReader
    {

        #region ' ISwiftMtMessageReader '

        public ISwiftMtMessage Read(string rawMtMessage)
            => Read(new StringReader(rawMtMessage));

        public ISwiftMtMessage Read(TextReader textReader)
        {
            var reader = new ExtendedTextReader(textReader);

            var result = new SwiftMtMessage();

            do
            {
                if (!reader.TryReadChar(out var readChar)) continue;

                switch (readChar)
                {
                    case '{':
                        var block = ReadBlock(reader);
                        result.Blocks.Add(block);
                        break;

                    case ' ': throw new NotImplementedException();
                    default: throw new NotImplementedException();
                };
            } while (reader.CurrentPositionChar.HasValue);

            return result;
        }

        #endregion


        private IFileBlock ReadBlock(ExtendedTextReader reader)
        {
            var blockNumber = reader.ReadToChar(':');
            switch (blockNumber)
            {
                case "1":
                case "2":
                case "5":
                    return ReadBlockN(reader, blockNumber);

                case "4":
                    return ReadBlock4(reader);

                default:
                    throw new SwiftMtReadingException(
                        $"Unexpected block `{ blockNumber }`",
                        reader.CurrentLineNumber,
                        reader.CurrentPosition);
            }

            throw new NotImplementedException();
        }

        private IFileBlock ReadBlockN(ExtendedTextReader reader, string name)
        {
            var block = new GenericFileBlock(name);
            var rawBodyBuffer = new StringBuilder();

            while (reader.TryReadChar(out var c))
            {
                if (c == '}')
                {
                    block.RawBody = rawBodyBuffer.ToString();
                    return block;
                }

                rawBodyBuffer.Append(c);

                if (c == '{') // inner block
                {
                    var subBlockName = reader.ReadToChar(':');
                    rawBodyBuffer.Append(subBlockName);
                    rawBodyBuffer.Append(':');
                    rawBodyBuffer.Append(ReadBlockN(reader, subBlockName));
                    rawBodyBuffer.Append('}');
                }

            }

            return block;
        }

        private IFileBlock ReadBlock4(ExtendedTextReader reader)
        {
            var block4 = new FileBlock4();

            reader.MoveToNext(new[] { ':', '-' });

            while (reader.CurrentPositionChar.HasValue)
            {
                switch (reader.CurrentPositionChar)
                {
                    case ':':
                        block4.Tags.Add(ReadTag(reader));
                        break;

                    case '-':   // block4 closing...
                        var closingChar = reader.ReadNextChar();  // read last '}' char
                        if (closingChar != '}')
                        {
                            throw new SwiftMtReadingException("Expected `}` char", reader.CurrentLineNumber, reader.CurrentPosition);
                        }
                        return block4;

                    default:
                        throw new SwiftMtReadingException(
                            $"Unexpected char `{ reader.CurrentPositionChar }` ({ Convert.ToInt32(reader.CurrentPositionChar) }).",
                            reader.CurrentLineNumber,
                            reader.CurrentPosition);
                }
            }

            throw new SwiftMtReadingException(
                $"Ending of Block #4 not found",
                reader.CurrentLineNumber,
                reader.CurrentPosition);
        }

        private ITag ReadTag(ExtendedTextReader reader)
        {
            // ler tagName
            var tagName = reader.ReadToChar(':');

            switch (tagName)
            {
                case "16R":
                    return Read16rTag(reader);

                default:
                    return ReadTag(reader, tagName);
            }

            throw new NotImplementedException();
        }

        private TextBlock Read16rTag(ExtendedTextReader reader)
        {
            var resultGroup = new TextBlock()
            {
                Link = reader.ReadToChar(':').Trim()
            };

            ITag readTag;
            while ((readTag = ReadTag(reader)) != null)
            {
                if (readTag.TagName == "16S")
                {
                    var _16sLink = readTag.RawBody;
                    if (_16sLink != resultGroup.Link)
                    {
                        throw new SwiftMtReadingException(
                            $"Missing group ending tag `{ resultGroup.Link }`",
                            reader.CurrentLineNumber,
                            reader.CurrentPosition);
                    }

                    return resultGroup;  // tag read successfully.
                }

                resultGroup.Tags.Add(readTag);
            }

            throw new SwiftMtReadingException(
                $"Missing group ending tag `{ resultGroup.Link }`",
                reader.CurrentLineNumber,
                reader.CurrentPosition);
        }

        private Tag ReadTag(
            ExtendedTextReader reader,
            string tagName)
        {
            var result = new Tag()
            {
                TagName = tagName,
            };

            var firstTagBodyChar = reader.ReadNextChar();
            if (firstTagBodyChar == ':')
            {
                result.Qualifier = reader.ReadToChar('/');

                result.RawBody = reader.ReadNextChar() == '/'
                        ? ReadToTheEndOfTheTag(reader)
                        : reader.CurrentPositionChar + ReadToTheEndOfTheTag(reader);
            }
            else
            {
                result.RawBody
                    = reader.CurrentPositionChar
                    + ReadToTheEndOfTheTag(reader);
            }

            return result;
        }

        private string ReadToTheEndOfTheTag(ExtendedTextReader reader)
        {
            var sb = new StringBuilder();
            var hasValidCharInTheLine = true;

            while (reader.TryReadChar(out var c))
            {
                var endOfTheTag = c == ':' || !hasValidCharInTheLine && c == '-';
                if (endOfTheTag)
                {
                    break;
                }

                if (!hasValidCharInTheLine && c != ' ' && c != '\t')
                {
                    hasValidCharInTheLine = true;
                }

                if (c == '\n') hasValidCharInTheLine = false;

                sb.Append(c);
            }

            return sb.Length == 0 ? null : sb.ToString().Trim();
        }


    }
}
