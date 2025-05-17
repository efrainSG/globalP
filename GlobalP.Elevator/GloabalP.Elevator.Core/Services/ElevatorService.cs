using GloabalP.Elevator.Core.Enums;
using GloabalP.Elevator.Core.Interfaces;

namespace GloabalP.Elevator.Core.Services
{
    public class ElevatorService : IElevatorService
    {
        private readonly Entities.Elevator _elevator;
        private readonly IDispatchingStrategy _dispatchingStrategy;
        private readonly ILogger _logger;
        private byte? _nextTarget;
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

        public void CallFromFloor(FloorRequest request)
        {
            _logger.Info($"External request from {request.Floor} to {request.Direction}");
            _dispatchingStrategy.AddRequest(request.Floor);
        }

        public void Step()
        {
            if (_nextTarget == null)
            {
                _nextTarget = _dispatchingStrategy.GetNextRequest();
            }

            if (_nextTarget.HasValue)
            {
                MoveTowards(_nextTarget.Value);
            }
        }

        private void MoveTowards(byte target)
        {
            if (_elevator.CurrentFloor < target)
            {
                _elevator.CurrentFloor++;
                _elevator.Direction = Direction.Up;
                _logger.Info($"Elevator going up to {target} floor from {_elevator.CurrentFloor} floor.");
            }

            else if (_elevator.CurrentFloor > target)
            {
                _elevator.CurrentFloor--;
                _elevator.Direction = Direction.Down;
                _logger.Info($"Elevator going down to {target} floor from {_elevator.CurrentFloor} floor.");
            }

            if (_elevator.CurrentFloor == target)
            {
                _elevator.DoorState = DoorState.Open;
                _logger.Info($"Elevator opens doors on {target} floor.");
                _elevator.Direction = Direction.Idle;
                _nextTarget = null;
            }
        }
    }
}
