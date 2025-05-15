using GloabalP.Elevator.Core.Interfaces;

namespace GloabalP.Elevator.Core.Strategies
{
    public class FifoDispatchingStrategy : IDispatchingStrategy
    {
        private readonly LinkedList<byte> _requests = new();
        private readonly HashSet<byte> _requestedFloors = new();

        public void AddRequest(byte floor)
        {
            if (floor < 1 || floor > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(floor));
            }

            if (_requestedFloors.Add(floor))
            {
                _requests.AddLast(floor);
            }
        }

        public byte? GetNextRequest()
        {
            if (_requests.Count == 0)
            {
                return null;
            }

            var next = _requests.First.Value;
            _requests.RemoveFirst();
            _requestedFloors.Remove(next);

            return next;
        }

        public IReadOnlyCollection<byte> GetPendingRequests() => _requests.ToList().AsReadOnly();
    }
}
