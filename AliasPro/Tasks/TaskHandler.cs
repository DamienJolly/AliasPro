using AliasPro.API.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace AliasPro.Tasks
{
    public static class TaskHandler
    {
        public static async Task RunTaskAsync(ITask task) => await RunTaskAsyncWithDelay(task, 0);

        public static async Task PeriodicAsyncTaskWithDelay(ITask task, int delay, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await RunTaskAsyncWithDelay(task, delay);
            }
        }

        public static async Task RunTaskAsyncWithDelay(ITask task, int delay)
        {
            await Task.Run(async delegate
            {
                await Task.Delay(delay);
                
                try
                {
                    task.Run();
                }
                catch
                {

                }
            });
        }
    }
}
