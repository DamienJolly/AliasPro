using AliasPro.API.Tasks;
using System;
using System.Threading.Tasks;

namespace AliasPro.Tasks
{
	sealed class TaskManager
	{
		public static Task ExecuteTask(ITask executeTask, int delay = 0)
		{
			return Task.Run(async () =>
			{
				try
				{
					if (delay != 0)
						await Task.Delay(TimeSpan.FromMilliseconds(delay));
					executeTask.Run();
				}
				catch (Exception e)
				{
					Console.WriteLine("Task Error! " + e.Message);
				}
			});
		}
	}
}
