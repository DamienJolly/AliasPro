using AliasPro.API.Navigator.Models;
using AliasPro.API.Navigator.Views;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Navigator.Views
{
	public class MyRoomsCategory : ICategoryType
    {
		public MyRoomsCategory(INavigatorCategory category)
			: base(category)
		{

		}

		public override async Task<IList<IRoomData>> GetResults(IRoomController roomController, IPlayer player, string searchCode)
		{
			IList<IRoomData> rooms = new List<IRoomData>();

			foreach (IRoomData roomData in await roomController.GetPlayersRooms(player.Id))
			{
				if (!roomData.Name.ToLower().Contains(searchCode.ToLower()))
					continue;

				if (roomData.OwnerId != player.Id)
					continue;

				System.Console.WriteLine(roomData.UsersNow);

				rooms.Add(roomData);
			}

			return rooms;
		}
	}
}
