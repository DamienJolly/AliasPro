using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Navigator.Models;
using AliasPro.Navigator.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Sessions;
using System.Collections.Generic;

namespace AliasPro.Navigator.Packets.Events
{
    internal class RequestNavigatorFlatsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestNavigatorFlatsMessageEvent;

        private readonly INavigatorController _navigatorController;

        public RequestNavigatorFlatsEvent(INavigatorController navigatorController)
        {
            _navigatorController = navigatorController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            if (!_navigatorController.TryGetCategories("roomads_view", out ICollection<INavigatorCategory> categories)) return;
            
            await session.SendPacketAsync(new NavigatorFlatCatsComposer(categories, session.Player.Rank));
        }
    }
}
