namespace LineageServer.Interfaces
{
    public interface IXmlDeserialize
    {
        T Deserialize<T>(string xml);
    }
}
