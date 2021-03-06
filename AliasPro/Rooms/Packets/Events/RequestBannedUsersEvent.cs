﻿using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestBannedUsersEvent : IMessageEvent
    {
        public short Header => Incoming.RequestBannedUsersMessageEvent;

        private readonly IRoomController _roomController;

        public RequestBannedUsersEvent(
			IRoomController roomController)
        {
            _roomController = roomController;
        }

		public async Task RunAsync(
			ISession session,
			ClientMessage message)
		{
            int roomId = message.ReadInt();

            if (!_roomController.TryGetRoom((uint)roomId, out IRoom room))
                return;

            await session.SendPacketAsync(new RoomBannedUsersComposer((int)room.Id, room.Bans.BannedPlayers));
        }
    }
}
