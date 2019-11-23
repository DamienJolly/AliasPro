using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomBotSettingsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RoomBotSettingsMessageEvent;

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null) return;

			if (room.OwnerId != session.Player.Id)
				return;

			int botId = clientPacket.ReadInt();
			if (!room.Entities.TryGetEntityById(botId, out BaseEntity entity))
				return;

			if (!(entity is BotEntity))
				return;

			int settingId = clientPacket.ReadInt();
			await session.SendPacketAsync(new RoomBotSettingsComposer(entity, settingId));
		}
    }
}
