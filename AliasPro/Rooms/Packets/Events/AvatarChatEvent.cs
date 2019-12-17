using AliasPro.API.Chat;
using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Interaction;
using AliasPro.Network.Events.Headers;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Events
{
    public class AvatarChatEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.AvatarChatMessageEvent;

        private readonly IChatController _chatController;

        public AvatarChatEvent(IChatController chatController)
        {
            _chatController = chatController;
        }

        public void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;

            string text = clientPacket.ReadString();
            int colour = clientPacket.ReadInt();

            session.Entity.Unidle();

            if (!_chatController.HandleCommand(session, text))
            {
				IList<BaseEntity> targetEntities = null;
				if (session.Entity.Room.RoomGrid.TryGetRoomTile(session.Entity.Position.X, session.Entity.Position.Y, out IRoomTile tile))
				{
					IItem topItem = tile.TopItem;

					if (topItem != null && topItem.Interaction is InteractionTent interaction)
                        targetEntities = interaction.TentEntities;
                }

				room.OnChat(text, colour, session.Entity, targetEntities);
            }
        }
    }
}
