using GloabalP.Elevator.Core.Enums;

namespace GloabalP.Elevator.Core.Interfaces
{
    public interface IElevatorService
    {
        void CallFromFloor(byte floor, Direction direction);
        void RequestFloor(byte floor);
        void Step();
    }
}