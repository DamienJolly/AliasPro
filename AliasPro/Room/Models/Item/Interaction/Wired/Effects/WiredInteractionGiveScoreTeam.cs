﻿using AliasPro.Item.Models;
using AliasPro.Room.Models.Entities;
using AliasPro.Room.Models.Game;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionGiveScoreTeam : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.GIVE_SCORE_TEAM;

        private bool _active = false;
        private int _tick = 0;

        public WiredData WiredData { get; set; }

        public WiredInteractionGiveScoreTeam(IItem item)
        {
            _item = item;
            WiredData =
                new WiredData((int)_type, _item.ExtraData);
        }

        public void OnTrigger(params object[] args)
        {
            if (!_active)
            {
                _active = true;
                _tick = WiredData.Delay;
            }
        }

        public void OnCycle()
        {
            if (_active)
            {
                if (_tick <= 0)
                {
                    if (TeamType != GameTeamType.NONE)
                    {
                        _item.CurrentRoom.GameHandler.GiveTeamPoints(TeamType, TeamPoints, MaxPoints);
                    }
                    _active = false;
                }
                _tick--;
            }
        }

        private int TeamPoints =>
            (WiredData.Params.Count <= 0) ? 1 : WiredData.Params[0];

        private int MaxPoints =>
            (WiredData.Params.Count <= 1) ? 1 : WiredData.Params[1];

        private GameTeamType TeamType
        {
            get
            {
                int teamId =
                    (WiredData.Params.Count <= 2) ? 0 : WiredData.Params[2];

                switch (teamId)
                {
                    case 1: return GameTeamType.RED;
                    case 2: return GameTeamType.GREEN;
                    case 3: return GameTeamType.BLUE;
                    case 4: return GameTeamType.YELLOW;
                    default: return GameTeamType.NONE;
                }
            }
        }
    }
}
