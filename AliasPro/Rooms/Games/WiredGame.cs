﻿using AliasPro.API.Rooms.Games;
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
			base.JoinTeam(entity, teamType);
			entity.SetEffect(39 + (int)teamType);
		}

		public override void LeaveTeam(PlayerEntity entity)
		{
			base.LeaveTeam(entity);
			entity.SetEffect(0);
		}
	}
}
