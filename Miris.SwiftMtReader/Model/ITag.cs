namespace Miris.SwiftMtReader.Model
{
    public interface ITag
    {
        string TagName { get; }
        string Qualifier { get; }
        string RawBody { get; }
    }
}
