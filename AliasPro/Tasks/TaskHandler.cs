using System;
using System.Threading;
using System.Threading.Tasks;

namespace AliasPro.Tasks
{
    public static class TaskHandler
    {
        public static async Task RunTaskAsync(ITask task) => await Task.Run(() => task.Run());
        
        public static async Task RunTaskAsyncWithDelay(ITask task, int delay)
        {
            await Task.Run(async delegate
            {
                await Task.Delay(delay);
                task.Run();
            });
        }

        public static async Task PeriodicAsyncTaskWithDelay(ITask task, int delay, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await RunTaskAsyncWithDelay(task, delay);
            }
        }
    }
}
