namespace Ollert.Logic.Interfaces
{
    public interface IAppContext
    {
        object ClientInfo { get; }
        string CorrelationId { get; }
    }
}
