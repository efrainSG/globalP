using GloabalP.Elevator.Core;
using GloabalP.Elevator.Core.Enums;
using GloabalP.Elevator.Core.Interfaces;
using GloabalP.Elevator.Core.Services;
using GloabalP.Elevator.Core.Strategies;
using GloabalP.Elevator.Infrastructure.Logging;

namespace GlobalP.Elevator.ConsoleApp
{
    public static class Program
    {
        static void Main(string[] args)
        {
            ILogger logger = new Log4NetLogger();

            var _elevator = new GloabalP.Elevator.Core.Entities.Elevator();
            var _dispatchingStrategy = new FifoDispatchingStrategy();
            var _elevatorService = new ElevatorService(_elevator, _dispatchingStrategy, logger);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Elevator Simulator ===");
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine($"\tCurrent status:\n{_elevator.ToString().Replace(", ","\n")}\n");
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("1. Call elevator from floor (external button)");
                Console.WriteLine("2. Request floor from inside elevator");
                Console.WriteLine("3. Execute elevator step");
                Console.WriteLine("4. Show pending requests");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        CallFromFloor(_elevatorService, logger);
                        break;
                    case "2":
                        RequestFromInside(_elevatorService, logger);
                        break;
                    case "3":
                        _elevatorService.Step();
                        break;
                    case "4":
                        ShowPendingRequests(_dispatchingStrategy, logger);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press ENTER to continue...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        static void CallFromFloor(IElevatorService _service, ILogger log)
        {
            Console.Write("Enter calling floor (1-5): ");
            byte floor = byte.Parse(Console.ReadLine());
            Direction dir = Direction.Idle;
            if (floor == 1)
            {
                dir = Direction.Up;
            }
            else if (floor == 5)
            {
                dir = Direction.Down;
            }
            else
            {
                Console.Write("Direction (U/D): ");
                var d = Console.ReadLine().ToUpper();
                dir = d == "U" ? Direction.Up : Direction.Down;
            }

            try
            {
                var request = new FloorRequest(floor, dir);
                _service.CallFromFloor(request);
                log.Info($"Request registered. {request.ToString()}");
                Console.WriteLine("Request registered.");
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
            finally
            {
                Console.WriteLine("Press ENTER to continue...");
                Console.ReadLine();
            }
        }

        static void RequestFromInside(IElevatorService _service, ILogger log)
        {
            Console.Write("Enter destination floor (1-5): ");
            byte floor = byte.Parse(Console.ReadLine());

            try
            {
                _service.RequestFloor(floor);
                log.Info($"Request registered from floor {floor}.");
                Console.WriteLine("Floor requested.");
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
        }

        static void ShowPendingRequests(IDispatchingStrategy strategy, ILogger log)
        {
            Console.WriteLine("Pending requests:");
            foreach (var r in strategy.GetPendingRequests())
            {
                Console.WriteLine($" - Floor {r}");
            }
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
