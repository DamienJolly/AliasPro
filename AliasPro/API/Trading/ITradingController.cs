using AliasPro.API.Rooms.Entities;
using AliasPro.API.Trading.Models;

namespace AliasPro.API.Trading
{
    public interface ITradingController
	{
		ITrade StartTrade(BaseEntity playerOne, BaseEntity playerTwo);
	}
}
