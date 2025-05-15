using GloabalP.Elevator.Core.Enums;
using GloabalP.Elevator.Core.Interfaces;

namespace GloabalP.Elevator.Core.Entities
{
    public class Elevator: IElevator
    {
        public byte CurrentFloor { get; set; } = 1;
        public Direction Direction { get; set; }
        public DoorState DoorState { get; set; }

        public Elevator() { }

        public void OpenDoors() => DoorState = DoorState.Open;
        public void CloseDoors() => DoorState = DoorState.Closed;
    }
}
