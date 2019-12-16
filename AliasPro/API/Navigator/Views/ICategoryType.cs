using AliasPro.API.Navigator.Models;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Navigator.Views
{
	public abstract class ICategoryType
	{
		public INavigatorCategory Category;

		public ICategoryType(INavigatorCategory category)
		{
			Category = category;
		}
		public abstract Task<IList<IRoomData>> GetResults(IRoomController roomController, IPlayer player, string searchCode);
	}
}
