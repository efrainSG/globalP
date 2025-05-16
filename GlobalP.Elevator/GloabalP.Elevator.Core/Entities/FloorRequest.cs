using GloabalP.Elevator.Core.Enums;

namespace GloabalP.Elevator.Core
{
    public class FloorRequest
    {
        public byte Floor { get; }

        public Direction Direction { get; }

        public FloorRequest(byte floor, Direction direction)
        {
            Floor = floor;
            Direction = direction;
        }
    }
}
