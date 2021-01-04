using System;
using System.Threading;

namespace Arbitrage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                var output = ArbitrageRunner.Run();
                if (!string.IsNullOrEmpty(output))
                {
                    Console.Clear();
                    Console.WriteLine("{0:HH:mm}", DateTime.Now);
                    Console.WriteLine(output);
                }
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }
    }
}
