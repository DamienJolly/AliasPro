using AliasPro.API.Navigator;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Navigator.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Navigator.Packets.Events
{
    internal class RequestUserFlatCatsEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.RequestUserFlatCatsMessageEvent;

        private readonly INavigatorController _navigatorController;

        public RequestUserFlatCatsEvent(INavigatorController navigatorController)
        {
            _navigatorController = navigatorController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            await session.SendPacketAsync(new UserFlatCatsComposer(_navigatorController.TryGetCategoryByView("hotel_view"), session.Player.Rank));
        }
    }
}
