using AliasPro.API.Players.Models;
using AliasPro.Players.Packets.Composers;

namespace AliasPro.Players.Cycles
{
    public class PlayerCycle
    {
        private readonly IPlayer _player;

        public PlayerCycle(IPlayer player)
        {
            _player = player;
        }

        public async void Cycle()
        {
            try
            {
                _player.Credits += 1000;
                await _player.Session.SendPacketAsync(new UserCreditsComposer(_player.Credits));
            }
            catch { }
        }
    }
}
