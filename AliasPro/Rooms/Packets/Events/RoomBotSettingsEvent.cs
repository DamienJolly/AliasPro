using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomBotSettingsEvent : IMessageEvent
    {
        public short Header => Incoming.RoomBotSettingsMessageEvent;

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null) 
				return;

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
