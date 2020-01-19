using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Navigator.Models;
using AliasPro.API.Navigator.Views;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Navigator.Views
{
	public class PromotionsCategory : ICategoryType
	{
		public PromotionsCategory(INavigatorCategory category)
			: base(category)
		{

		}

		public override async Task<IList<IRoomData>> GetResults(IRoomController roomController, IPlayer player, string searchCode)
		{
			IList<IRoomData> rooms = new List<IRoomData>();

			foreach (IRoomData roomData in roomController.Rooms)
			{
				if (searchCode.StartsWith("owner:"))
				{
					string param = searchCode.Split("owner:")[1];
					if (roomData.OwnerName.ToLower() != param.ToLower())
						continue;
				}
				else if (searchCode.StartsWith("tag:"))
				{
					string param = searchCode.Split("tag:")[1];
					if (!roomData.Tags.Contains(param))
						continue;
				}
				else if (searchCode.StartsWith("group:"))
				{
					string param = searchCode.Split("group:")[1];
					if (roomData.Group == null)
						continue;

					IGroup group =
						await Program.GetService<IGroupController>().ReadGroupData(roomData.Group.Id);

					if (!group.Name.ToLower().Contains(param.ToLower()))
						continue;
				}
				else
				{
					if (!roomData.Name.ToLower().Contains(searchCode.ToLower()))
						continue;
				}

				if (!roomData.IsPromoted)
					continue;

				if (roomData.Promotion.Category != Category.SortId)
					continue;

				rooms.Add(roomData);
			}

			return rooms;
		}
	}
}
