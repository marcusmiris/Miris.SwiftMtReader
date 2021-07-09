namespace Miris.SwiftMtReader.Internals
{
    internal interface IExtendedTextReader
    {
        char ReadNextChar();
        bool TryReadChar(out char? c);
    }
}
