using AliasPro.API.Navigator;
using AliasPro.API.Navigator.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Navigator.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Sessions;
using System.Collections.Generic;

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
            if (!_navigatorController.TryGetCategories("hotel_view", out ICollection<INavigatorCategory> categories)) return;

            await session.SendPacketAsync(new UserFlatCatsComposer(categories, session.Player.Rank));
        }
    }
}
