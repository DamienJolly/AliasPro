using AliasPro.API.Navigator;
using AliasPro.API.Navigator.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Navigator.Models;
using AliasPro.Navigator.Packets.Composers;
using AliasPro.Network.Events.Headers;
using System.Collections.Generic;

namespace AliasPro.Navigator.Packets.Events
{
    internal class NavigatorSearchEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.NavigatorSearchMessageEvent;

        private readonly INavigatorController _navigatorController;
        private readonly IRoomController _roomController;

        public NavigatorSearchEvent(
			INavigatorController navigatorController,
            IRoomController roomController)
        {
            _navigatorController = navigatorController;
            _roomController = roomController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            string name = clientPacket.ReadString();
            string query = clientPacket.ReadString();
            bool showMore = !_navigatorController.IsView(name);

            IList<INavigatorSearchResult> results = new List<INavigatorSearchResult>();

            if (showMore)
            {
                if (_navigatorController.TryGetCategory(name, out INavigatorCategory category))
                {
                    IList<IRoomData> rooms = 
                        await category.CategoryType.GetResults(_roomController, session.Player, query);

                    if (rooms.Count != 0)
                        results.Add(new NavigatorSearchResult(category.SortId, category.Identifier, category.PublicName, rooms, showMore));
                }
            }
            else
            {
                foreach (INavigatorCategory category in _navigatorController.TryGetCategoryByView(name))
                {
                    IList<IRoomData> rooms =
                        await category.CategoryType.GetResults(_roomController, session.Player, query);

                    if ((category.Identifier == "popular" && !string.IsNullOrEmpty(query)) || (category.Identifier == "query" && string.IsNullOrEmpty(query)))
                        continue;

                    if (rooms.Count != 0)
                        results.Add(new NavigatorSearchResult(category.SortId, category.Identifier, category.PublicName, rooms, showMore));
                }
            }

            await session.SendPacketAsync(
                new NavigatorSearchResultSetComposer(name, query, results));
        }
    }
}
