using System.Threading.Tasks;
using System.Collections.Generic;

namespace AliasPro.Landing.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;
    using Models;

    public class RequestNewsListEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestNewsListMessageEvent;

        private readonly ILandingController _landingController;

        public RequestNewsListEvent(ILandingController landingController)
        {
            _landingController = landingController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IList<IArticles> artiles = await _landingController.GetNewsArticlesAsync();
            await session.SendPacketAsync(new NewsListComposer(artiles));
        }
    }
}