using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Moderation.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationRequestRoomVisitsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ModerationRequestRoomVisitsMessageEvent;
        
        private readonly IPlayerController _playerController;

        public ModerationRequestRoomVisitsEvent(
            IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            //todo: permissions
            if (session.Player.Rank <= 2)
                return;
            
            int playerId = clientPacket.ReadInt();

            IPlayerData player = await _playerController.GetPlayerDataAsync((uint)playerId);
            if (player == null)
                return;

            await session.SendPacketAsync(new ModerationUserRoomVisitsComposer(player,
                await _playerController.GetPlayerRoomVisitsAsync(player.Id)));
        }
    }
}