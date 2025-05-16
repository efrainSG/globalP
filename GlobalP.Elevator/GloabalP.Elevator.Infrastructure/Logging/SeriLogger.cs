using GloabalP.Elevator.Core.Interfaces;

namespace GloabalP.Elevator.Infrastructure
{
    public class SeriLogger : ILogger
    {
        public void Error(string message)
        {
            Console.WriteLine($"ERR: {message}");
        }

        public void Info(string message)
        {
            Console.WriteLine($"INFO: {message}");
        }
    }
}
