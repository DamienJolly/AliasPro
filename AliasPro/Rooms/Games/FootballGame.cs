using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Games;
using AliasPro.API.Rooms.Models;
using AliasPro.Rooms.Games.Types;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Games
{
	public class FootballGame : BaseGame
	{
		public override GameType Type => GameType.FOOTBALL;

		public FootballGame(IRoom room)
			: base(room)
		{

		}

		public override async void GivePlayerPoints(BaseEntity entity, int amount)
		{
			if (Room == null)
				return;

			await Room.SendPacketAsync(new UserActionComposer(entity, 1));
		}
	}
}
