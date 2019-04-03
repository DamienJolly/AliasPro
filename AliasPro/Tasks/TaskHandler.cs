using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

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
        
        public static ActionBlock<DateTimeOffset> PeriodicTaskWithDelay(
            Action<DateTimeOffset> action, CancellationToken cancellationToken, int delay)
        {
            ActionBlock<DateTimeOffset> block = null;
            block = new ActionBlock<DateTimeOffset>(async now => {
                action(now);
                await Task.Delay(TimeSpan.FromMilliseconds(delay)).
                    ConfigureAwait(false);
                block.Post(DateTimeOffset.Now);
            }, new ExecutionDataflowBlockOptions
            {
                CancellationToken = cancellationToken
            });

            return block;
        }
    }
}
