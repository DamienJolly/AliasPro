using AliasPro.API.Groups;
using AliasPro.API.Players;
using AliasPro.API.Rooms;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace AliasPro.Tasks
{
	public class TaskHandler
	{
		private readonly ILogger<TaskHandler> logger;
		private readonly IRoomController roomController;
		private readonly IPlayerController playerController;
		private readonly IGroupController groupController;

		private Timer GameCycleTimer;

		public TaskHandler(
			ILogger<TaskHandler> logger,
			IRoomController roomController,
			IPlayerController playerController,
			IGroupController groupController)
		{
			this.logger = logger;
			this.roomController = roomController;
			this.playerController = playerController;
			this.groupController = groupController;
		}

		public Task ExecuteTask(ITask executeTask, int delay = 0)
		{
			return Task.Run(async () =>
			{
				try
				{
					if (delay != 0)
					{
						await Task.Delay(TimeSpan.FromMilliseconds(delay));
					}
					executeTask.Run();
				}
				catch (Exception e)
				{
					logger.LogError("There was an error in a Task: " + e.ToString());
				}
			});
		}

		public void StartGameCycle()
		{
			GameCycleTimer = new Timer();
			GameCycleTimer.Elapsed += Cycle;
			GameCycleTimer.AutoReset = true;
			GameCycleTimer.Interval = 500;
			GameCycleTimer.Start();
		}

		private void Cycle(object sender, ElapsedEventArgs eventArgs)
		{
			try
			{
				roomController.Cycle();
				playerController.Cycle();
				groupController.Cycle();
			}
			catch (Exception e)
			{
				logger.LogError("There was an error in the Game Cycle: " + e.ToString());
			}
		}
	}
}
