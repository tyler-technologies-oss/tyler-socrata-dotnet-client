namespace Socrata.Abstractions
{
    public interface ISocrataClient
    {
        Resource GetResource(string id);
        bool ValidateConnection();
    }
}
