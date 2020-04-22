using AliasPro.API.Rooms.Games;
using AliasPro.API.Rooms.Games.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Games.Types;
using AliasPro.Rooms.Types;

namespace AliasPro.Rooms.Games
{
	public class WiredGame : BaseGame
	{
		public override GameType Type => GameType.WIRED;

		public WiredGame(IRoom room)
			: base(room)
		{

		}

		public override void Initialize()
		{
			TimesGivenScore = 0;
			foreach (IGameTeam team in Teams)
			{
				team.ResetScores();
			}
		}

		public override void JoinTeam(PlayerEntity entity, GameTeamType teamType)
		{
			//this.room.giveEffect(habbo, FreezeGame.effectId + teamColor.type, -1);
			base.JoinTeam(entity, teamType);
		}

		public override void LeaveTeam(PlayerEntity entity)
		{
			//this.room.giveEffect(habbo, 0, -1);
			base.LeaveTeam(entity);
		}
	}
}
