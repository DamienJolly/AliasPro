using AliasPro.API.Landing;
using AliasPro.API.Landing.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Landing.Packets.Composers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Landing.Packets.Events
{
    public class RequestNewsListEvent : IMessageEvent
    {
        public short Header => Incoming.RequestNewsListMessageEvent;

        private readonly ILandingController _landingController;

        public RequestNewsListEvent(ILandingController landingController)
        {
            _landingController = landingController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IList<IArticle> artiles = await _landingController.GetNewsArticlesAsync();
            await session.SendPacketAsync(new NewsListComposer(artiles));
        }
    }
}