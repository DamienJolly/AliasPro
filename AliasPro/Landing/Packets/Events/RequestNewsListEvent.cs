using AliasPro.API.Landing;
using AliasPro.API.Landing.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Landing.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Sessions;
using System.Collections.Generic;

namespace AliasPro.Landing.Packets.Events
{
    public class RequestNewsListEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestNewsListMessageEvent;

        private readonly ILandingController _landingController;

        public RequestNewsListEvent(ILandingController landingController)
        {
            _landingController = landingController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IList<IArticle> artiles = await _landingController.GetNewsArticlesAsync();
            await session.SendPacketAsync(new NewsListComposer(artiles));
        }
    }
}