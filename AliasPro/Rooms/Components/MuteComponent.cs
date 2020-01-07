using AliasPro.API.Rooms.Models;
using AliasPro.Utilities;
using System.Collections.Generic;

namespace AliasPro.Rooms.Components
{
    public class MuteComponent
    {
        private readonly IRoom _room;
        private readonly IDictionary<int, int> _mutedPlayers;

        public bool GameStarted = false;

        public MuteComponent(IRoom room)
        {
            _room = room;
            _mutedPlayers = new Dictionary<int, int>();
        }

        public void MutePlayer(int playerId, int time)
        {
            if (_mutedPlayers.ContainsKey(playerId))
                _mutedPlayers[playerId] = time;
            else
                _mutedPlayers.Add(playerId, time);
        }

        public bool PlayerMuted(int playerId)
        {
            bool isMuted = false;
            if (_mutedPlayers.ContainsKey(playerId))
            {
                isMuted = _mutedPlayers[playerId] > (int)UnixTimestamp.Now;

                if (!isMuted)
                    _mutedPlayers.Remove(playerId);
            }
            return isMuted;
        }

        public int MutedTimeLeft(int playerId)
        {
            int timeleft = 0;
            if (_mutedPlayers.ContainsKey(playerId))
                timeleft = _mutedPlayers[playerId] - (int)UnixTimestamp.Now;
            return timeleft;
        }
    }
}
