﻿using AliasPro.API.Chat;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Types;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class AvatarShoutEvent : IMessageEvent
    {
        public short Header => Incoming.AvatarShoutMessageEvent;

        private readonly IChatController _chatController;

        public AvatarShoutEvent(IChatController chatController)
        {
            _chatController = chatController;
        }

        public Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null)
                return Task.CompletedTask;

            string text = clientPacket.ReadString();
            int colour = clientPacket.ReadInt();

            session.Entity.Unidle();

            if (!_chatController.HandleCommand(session, text))
            {
                room.OnChat(text, colour, session.Entity, RoomChatType.SHOUT);
            }

            return Task.CompletedTask;
        }
    }
}
