namespace GloabalP.Elevator.Core.Interfaces
{
    public interface IDispatchingStrategy
    {
        LinkedList<byte> Requests { get; }
        void AddRequest(byte floor);
        byte? GetNextRequest();
        IReadOnlyCollection<byte> GetPendingRequests();
    }
}
