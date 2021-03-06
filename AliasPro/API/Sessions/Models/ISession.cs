﻿using AliasPro.API.Players.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages;
using System.Threading.Tasks;

namespace AliasPro.API.Sessions.Models
{
    public interface ISession
    {
        string UniqueId { get; set; }
        IPlayer Player { get; set; }
        BaseEntity Entity { get; set; }
        IRoom CurrentRoom { get; set; }

        void Disconnect();
        Task SendPacketAsync(IMessageComposer ServerMessage);
    }
}
