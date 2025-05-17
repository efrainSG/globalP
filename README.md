# globalP

## Global Payment exercise

For this excercise, a solution has been created following the SOLID principles keeping the code clean, clear, organized and easy to be upgraded.

The solution has the following projects: ConsoleApp, Core, Infrastructure, Tests.

### ConsoleApp
Is the front-end project. At this moment, it's easy to execute and provide the necessary options to test by hand the elevator's functionality.
- *Program.cs* 

### Core
Contains entities, enumerations, interfaces, services and dispatching strategies to process the requests received by the elevator. Here it's possible to add more strategies.
- *Entities:* Elevator [CurrentFloor, Direction, DoorState, Elevator(), OpenDoors(), CloseDoors(), ToString()], FloorRequest [Floor, Direction, FloorRequest(floor, direction)].
- *Enums:* Direction = (Up, Down, Idle), DoorState = (Open, Closed).
- *Intefaces:* IDispatchingStrategy [Requests, Addrequest(floor), GetNextRequest(), GetPeningRequests()], IElevator [OpenDoors(), CloseDoors()], IelevatorService[CallFromFloor(floor, direction), RequestFloor(floor), Step()], ILogger[Info(), Error()].
- *Services:* ElevatorService[Implements IElevatorService, contains readonlies "_elevator, _dispatchingStrategy, _logger, _nextTarget", MoveTowards(floor)].
- *Strategies:* FIfoDispatchingStrategy[Implements IDispatcherStrategy, contains readonlies _requests as LInkedList and _requestedFloors as HashSet].

### Infrastructure
Here resides the log components that helps monitoring the elevator functionality. Also allows to implement additional loggers.
- *Logging:* Log4NetLogger, SeriLogger.

### Tests
The unit tests. It helps by automating most of the test cases.
- *ElevatorTests.cs*

## Repository configuration
A basic rulset was added to the repostory. This rule ensures that the build procress and the executed automated tests must be succeed before merging the Pull Request to the MAIN branch.