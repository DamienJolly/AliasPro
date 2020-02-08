using AliasPro.API.Navigator;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Navigator.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Navigator.Packets.Events
{
    internal class RequestNavigatorSettingsEvent : IMessageEvent
    {
        public short Header => Incoming.RequestNavigatorSettingsMessageEvent;

        private readonly INavigatorController _navigatorController;

        public RequestNavigatorSettingsEvent(INavigatorController navigatorController)
        {
            _navigatorController = navigatorController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            await session.SendPacketAsync(new NavigatorEventCategoriesComposer(_navigatorController.TryGetCategoryByView("roomads_view")));
        }
    }
}
