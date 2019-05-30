using AliasPro.API.Navigator.Models;
using AliasPro.API.Navigator.Views;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using System.Collections.Generic;

namespace AliasPro.Navigator.Views
{
	public class MyRoomsCategory : ICategoryType
    {
		private readonly INavigatorCategory _category;

		public MyRoomsCategory(INavigatorCategory category)
		{
			_category = category;
		}

		public ICollection<IRoom> Search(IRoomController roomController, IPlayer player, string searchCode)
        {
			roomController.LoadPlayersRooms(player.Id);
			ICollection<IRoom> rooms = new List<IRoom>();

			foreach (IRoom room in roomController.Rooms)
			{
				if (!room.Name.ToLower().Contains(searchCode.ToLower()))
					continue;

				if (room.OwnerId != player.Id)
					continue;

				rooms.Add(room);
			}

			return rooms;
        }
    }
}
