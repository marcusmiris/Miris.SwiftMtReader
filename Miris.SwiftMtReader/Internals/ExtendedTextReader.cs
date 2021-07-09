using Miris.SwiftMtReader.Exceptions;
using Miris.SwiftMtReader.Extensions;
using System;
using System.IO;

namespace Miris.SwiftMtReader.Internals
{
    internal class ExtendedTextReader
    {
        public TextReader Reader { get; }
        public int CurrentPosition { get; private set; } = 0;
        public int CurrentLineNumber { get; private set; } = 1;
        public char? CurrentPositionChar { get; private set;  }

        public ExtendedTextReader(TextReader reader)
        {
            this.Reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        public char ReadNextChar() 
        {
            if (!this.TryReadChar(out var c))
            {
                throw new SwiftMtReadingException(CurrentLineNumber, CurrentPosition);
            }
            return c.Value;
        }

        public bool TryReadChar(out char? c)
        {
            var success = Reader.TryReadChar(out c);
            CurrentPositionChar = c;

            if (success)
            {
                CurrentPosition++;
                if (c == '\n')
                {
                    CurrentLineNumber++;
                }
            }

            return success;
        }

    }
}
