using AliasPro.API.Navigator.Models;
using AliasPro.API.Navigator.Views;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using System.Collections.Generic;

namespace AliasPro.Navigator.Views
{
    public class TopPromotionsCategory : ICategoryType
    {
		private readonly INavigatorCategory _category;

		public TopPromotionsCategory(INavigatorCategory category)
		{
			_category = category;
		}

		public ICollection<IRoom> Search(IRoomController roomController, IPlayer player, string searchCode)
		{
            ICollection<IRoom> rooms = new List<IRoom>();
            return rooms;
        }
    }
}
