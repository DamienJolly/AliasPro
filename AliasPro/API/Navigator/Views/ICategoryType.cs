using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using System.Collections.Generic;

namespace AliasPro.API.Navigator.Views
{
    public interface ICategoryType
    {
		ICollection<IRoom> Search(IRoomController roomController, IPlayer player, string searchCode);

	}
}
