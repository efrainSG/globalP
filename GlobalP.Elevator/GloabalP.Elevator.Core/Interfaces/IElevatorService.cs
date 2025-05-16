using GloabalP.Elevator.Core.Enums;

namespace GloabalP.Elevator.Core.Interfaces
{
    public interface IElevatorService
    {
        void CallFromFloor(FloorRequest request);
        void RequestFloor(byte floor);
        void Step();
    }
}