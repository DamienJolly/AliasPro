using AliasPro.API.Tasks;
using System;
using System.Threading.Tasks;

namespace AliasPro.Tasks
{
	sealed class TaskManager
	{
		public static Task ExecuteTask(ITask executeTask, int delay = 0)
		{
			if (delay == 0)
				return Task.Run(() => executeTask.Run());

			return Task.Run(async () =>
			{
				await Task.Delay(TimeSpan.FromMilliseconds(delay));
				executeTask.Run();
			});
		}
	}
}
