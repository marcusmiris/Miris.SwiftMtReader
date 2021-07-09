using Miris.SwiftMtReader.Exceptions;
using Miris.SwiftMtReader.Internals;
using System;
using System.Text;

namespace Miris.SwiftMtReader.Extensions
{
    internal static class ExtendedReaderExtensions
    {
        public static void MoveToNext(
            this ExtendedTextReader reader,
            char[] target)
        {
            do
            {
                reader.ReadNextChar();

            } while (Array.IndexOf(target, reader.CurrentPositionChar) == -1);
        }

        public static string ReadToChar(
            this ExtendedTextReader reader,
            char destination)
        {
            if (!reader.TryReadToChar(destination, out var read))
            {
                throw new SwiftMtReadingException(reader.CurrentLineNumber, reader.CurrentPosition);
            }
            return read;
        }

        public static bool TryReadToChar(
            this ExtendedTextReader reader,
            char destination,
            out string read)
        {
            var readSB = new StringBuilder();

            while (reader.TryReadChar(out var nextChar))
            {
                if (nextChar == destination)
                {
                    read = readSB.ToString();
                    return true;
                }

                readSB.Append(nextChar);
            }

            read = readSB.Length == 0 ? null : readSB.ToString();
            return false;
        }
    }
}
