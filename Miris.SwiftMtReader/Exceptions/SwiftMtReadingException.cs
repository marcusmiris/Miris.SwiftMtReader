using System;

namespace Miris.SwiftMtReader.Exceptions
{
    public class SwiftMtReadingException
        : Exception
    {

        public SwiftMtReadingException(
            string baseMessage,
            int LineNumber,
            int Position)
            : base($"{ baseMessage }. Line Number: { LineNumber }; Position: { Position }")
        {
            this.LineNumber = LineNumber;
            this.Position = Position;
        }

        public SwiftMtReadingException(
            int LineNumber,
            int Position)
            : base($"Unable to read. Line Number: { LineNumber }; Position: { Position }")
        {
            this.LineNumber = LineNumber;
            this.Position = Position;
        }

        public int LineNumber { get; }
        public int Position { get; }
    }
}
