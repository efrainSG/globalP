using GloabalP.Elevator.Core.Enums;
using GloabalP.Elevator.Core.Interfaces;

namespace GloabalP.Elevator.Core.Services
{
    public class ElevatorService
    {
        private readonly Entities.Elevator _elevator;
        private readonly IDispatchingStrategy _dispatchingStrategy;
        private readonly ILogger _logger;

        public ElevatorService(Entities.Elevator elevator, IDispatchingStrategy dispatchingStrategy, ILogger logger)
        {
            _elevator = elevator;
            _dispatchingStrategy = dispatchingStrategy;
            _logger = logger;
        }

        public void RequestFloor(byte floor)
        {
            _logger.Info($"Internal request received to {floor} floor.");
            _dispatchingStrategy.AddRequest(floor);
        }

        public void CallFromFloor(byte floor, Direction direction)
        {
            _logger.Info($"External request from {floor} to {direction}");
            _dispatchingStrategy.AddRequest(floor);
        }

        public void Step()
        {
            var nextTarget = _dispatchingStrategy.GetNextRequest();
            if (nextTarget.HasValue)
            {
                MoveTowards(nextTarget.Value);
            }
        }

        private void MoveTowards(byte target)
        {
            if (_elevator.CurrentFloor == target)
            {
                _elevator.DoorState = DoorState.Open;
                _logger.Info($"Elevator opens doors on {target} floor.");
                _elevator.DoorState = DoorState.Closed;
            }
            else if (_elevator.CurrentFloor < target)
            {
                _elevator.CurrentFloor++;
                _logger.Info($"Elevator going up to {target} floor.");
            }
            else
            {
                _elevator.CurrentFloor--;
                _logger.Info($"Elevator going down to {target} floor.");
            }
        }
    }
}
