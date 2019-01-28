using System;
using System.Threading.Tasks;

namespace AliasPro
{
    public class Program
    {
        /// <summary>
		/// Boot up the application
		/// </summary>
        private async Task Boot()
        {
            Console.WriteLine("Testing.. Hello World!");

            while (true)
            {
                if (Console.ReadKey(false).Key == ConsoleKey.Enter)
                {
                    await DisposeAsync();
                }
            }
        }

        /// <summary>
		/// Dispose off the application
		/// </summary>
        private async Task DisposeAsync()
        {
            Environment.Exit(0);
        }

        /// <summary>
		/// Entry point for the application
		/// </summary>
        public static async Task Main()
        {
            await new Program().Boot();
        }
    }
}
