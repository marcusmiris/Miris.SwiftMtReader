using System;
using System.IO;
using System.Text;

namespace Miris.SwiftMtReader.Extensions
{
    public static class TextReaderExtensions
    {
        public static bool TryReadChar(
            this TextReader reader,
            out char? read)
        {
            var readByte = reader.Read();
            if (readByte == -1)
            {
                read = null;
                return false;
            }

            read = Convert.ToChar(readByte);
            return true;
        }
    }
        
}
