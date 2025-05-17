using GloabalP.Elevator.Core.Enums;
using GloabalP.Elevator.Core.Interfaces;
using GloabalP.Elevator.Core.Services;
using GloabalP.Elevator.Core.Strategies;
using GloabalP.Elevator.Infrastructure.Logging;

namespace GloabalP.Elevator.Tests
{
    [TestFixture]
    public class ElevatorTests
    {

        private Core.Entities.Elevator _elevator;
        private ElevatorService _service;
        private IDispatchingStrategy _dispatcher;
        private ILogger _logger;

        [SetUp]
        public void Setup()
        {
            _elevator = new Core.Entities.Elevator();
            _dispatcher = new FifoDispatchingStrategy();
            _logger = new Log4NetLogger();
            _service = new ElevatorService(_elevator, _dispatcher, _logger);
        }

        [Test]
        public void ElevatorShouldNotGoBelowFirstFloorOrAboveFifthFloor()
        {
            _elevator.CurrentFloor = 1;
            _elevator.Direction = Direction.Down;

            _service.Step();
            Assert.AreEqual(1, _elevator.CurrentFloor);

            _elevator.CurrentFloor = 5;
            _elevator.Direction = Direction.Up;

            _service.Step();
            Assert.AreEqual(5, _elevator.CurrentFloor);
        }

        [Test]
        public void RequestFloor_ShouldAddRequestToDispatchingStrategy()
        {
            _service.RequestFloor(3);
            Assert.Contains(3, _dispatcher.Requests);
        }

        [Test]
        public void ElevatorMovesTowradRequestedFloor_StepByStep()
        {
            _service.RequestFloor(3);

            _service.Step();
            Assert.AreEqual(2, _elevator.CurrentFloor);
            Assert.AreEqual(Direction.Up, _elevator.Direction);
            Console.WriteLine(_elevator.ToString());

            _service.Step();

            Assert.AreEqual(3, _elevator.CurrentFloor);
            Assert.AreEqual(Direction.Idle, _elevator.Direction);
            Assert.AreEqual(DoorState.Open, _elevator.DoorState);
        }

        [Test]
        public void ElevatorShouldOpenDoorsAtTargetFloor()
        {
            _service.RequestFloor(2);
            _service.Step();

            Assert.AreEqual(2, _elevator.CurrentFloor);
            Assert.AreEqual(DoorState.Open, _elevator.DoorState);
        }

        [Test]
        public void RequestOutsideValidFloorRange_ShouldThrowOrIgnore()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _service.RequestFloor(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => _service.RequestFloor(6));
        }

        [Test]
        public void DuplicateRequestsAreIgnored()
        {
            _service.RequestFloor(4);
            _service.RequestFloor(4);

            Assert.AreEqual(1, _dispatcher.Requests.Count);
        }

        [Test]
        public void RequestsAreServedInArrivalOrder_ByDefaultStrategy()
        {
            _service.RequestFloor(4);
            _service.RequestFloor(2);
            _service.RequestFloor(5);

            Assert.AreEqual(4, _dispatcher.GetNextRequest());
            Assert.AreEqual(2, _dispatcher.GetNextRequest());
        }

        [Test]
        public void DoorsClose_WhenElevatorStartsMoving()
        {
            _service.RequestFloor(3);
            _service.Step();

            Assert.AreEqual(DoorState.Closed, _elevator.DoorState);
        }

        [Test]
        public void ElevatorRemainsIdle_WhenNoRequests()
        {
            _service.Step();

            Assert.AreEqual(Direction.Idle, _elevator.Direction);
            Assert.AreEqual(DoorState.Closed, _elevator.DoorState);
            Assert.AreEqual(1, _elevator.CurrentFloor);
        }

        [Test]
        public void RequestFromCurrentFloor_ShouldOpenDoorsImmediately()
        {
            _elevator.CurrentFloor = 3;
            _service.RequestFloor(3);
            _service.Step();

            Assert.AreEqual(DoorState.Open, _elevator.DoorState);
            Assert.AreEqual(Direction.Idle, _elevator.Direction);
        }

        [Test]
        public void ExternalCall_ShouldBeProcessed()
        {
            _service.CallFromFloor(new (4, Direction.Down));
            Assert.Contains(4, _dispatcher.Requests);
        }

        [Test]
        public void ElevatorReachesTopFloor_AfterMultipleSteps()
        {
            _service.RequestFloor(5);

            _service.Step();
            _service.Step();
            _service.Step();
            _service.Step();

            Assert.AreEqual(5, _elevator.CurrentFloor);
            Assert.AreEqual(DoorState.Open, _elevator.DoorState);
            Assert.AreEqual(Direction.Idle, _elevator.Direction);
        }

        [Test]
        public void DoorsRemainClosed_WithoutAction()
        {
            _service.Step();
            Assert.AreEqual(DoorState.Closed, _elevator.DoorState);
        }

    }
}