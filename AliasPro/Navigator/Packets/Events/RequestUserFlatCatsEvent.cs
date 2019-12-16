﻿using AliasPro.API.Navigator;
using AliasPro.API.Navigator.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Navigator.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Navigator.Packets.Events
{
    internal class RequestUserFlatCatsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestUserFlatCatsMessageEvent;

        private readonly INavigatorController _navigatorController;

        public RequestUserFlatCatsEvent(INavigatorController navigatorController)
        {
            _navigatorController = navigatorController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            await session.SendPacketAsync(new UserFlatCatsComposer(_navigatorController.TryGetCategoryByView("hotel_view"), session.Player.Rank));
        }
    }
}
