using AliasPro.API.Navigator.Models;
using AliasPro.API.Navigator.Views;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using System.Collections.Generic;

namespace AliasPro.Navigator.Views
{
	public class PopularCategory : ICategoryType
    {
		private readonly INavigatorCategory _category;

		public PopularCategory(INavigatorCategory category)
		{
			_category = category;
		}

		public ICollection<IRoom> Search(IRoomController roomController, IPlayer player, string searchCode)
		{
			ICollection<IRoom> rooms = new List<IRoom>();

			foreach (IRoom room in roomController.Rooms)
			{
				if (!room.Name.ToLower().Contains(searchCode.ToLower()))
					continue;

				rooms.Add(room);
			}

			return rooms;
		}
    }
}
