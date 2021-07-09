namespace Miris.SwiftMtReader.Model.FileBlocks
{
    public class GenericFileBlock
        : IFileBlock
    {

        public GenericFileBlock(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public string RawBody { get; internal set; }

        public string Outer => $"{{{Name}:{RawBody}}}";
    }
}
