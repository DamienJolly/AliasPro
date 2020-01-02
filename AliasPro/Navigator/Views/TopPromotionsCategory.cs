using AliasPro.API.Navigator.Models;
using AliasPro.API.Navigator.Views;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Navigator.Views
{
	public class TopPromotionsCategory : ICategoryType
	{
		public TopPromotionsCategory(INavigatorCategory category)
			: base(category)
		{

		}

		public override async Task<IList<IRoomData>> GetResults(IRoomController roomController, IPlayer player, string searchCode)
		{
			IList<IRoomData> rooms = new List<IRoomData>();

			foreach (IRoomData roomData in roomController.Rooms)
			{
				if (!roomData.Name.ToLower().Contains(searchCode.ToLower()))
					continue;

				if (!roomData.IsPromoted)
					continue;

				rooms.Add(roomData);
			}

			return rooms;
		}
	}
}
