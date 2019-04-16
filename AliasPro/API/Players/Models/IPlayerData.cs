﻿using AliasPro.Players.Types;

namespace AliasPro.API.Players.Models
{
    public interface IPlayerData
    {
        uint Id { get; set; }
        int Credits { get; set; }
        int Rank { get; set; }
        string Username { get; set; }
        string Figure { get; set; }
        PlayerGender Gender { get; set; }
        string Motto { get; set; }
        bool Online { get; set; }
        int Score { get; set; }
    }
}
