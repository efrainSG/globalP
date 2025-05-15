namespace GloabalP.Elevator.Core.Interfaces
{
    public interface IDispatchingStrategy
    {
        void AddRequest(byte floor);
        byte? GetNextRequest();
        IReadOnlyCollection<byte> GetPendingRequests();
    }
}
