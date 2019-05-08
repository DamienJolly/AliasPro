using AliasPro.API.Rooms;
using AliasPro.API.Server;
using System.Timers;

namespace AliasPro.Server
{
    internal class ServerController : IServerController
    {
        private Timer ServerCycleTimer;
        private IRoomController _roomController;

        public ServerController(IRoomController roomController)
        {
            _roomController = roomController;
        }

        public void Initialize()
        {
            ServerCycleTimer = new Timer();
            ServerCycleTimer.Elapsed += Cycle;
            ServerCycleTimer.AutoReset = true;
            ServerCycleTimer.Interval = 500;
            ServerCycleTimer.Start();
        }

        private void Cycle(object sender, ElapsedEventArgs e)
        {
            try
            {
                _roomController.Cycle();
            }
            catch { }
        }
    }
}
