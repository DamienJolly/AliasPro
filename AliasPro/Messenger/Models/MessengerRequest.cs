﻿using AliasPro.API.Database;
using AliasPro.API.Messenger.Models;
using AliasPro.API.Player.Models;
using AliasPro.Network.Protocol;
using System.Data.Common;

namespace AliasPro.Messenger.Models
{
    internal class MessengerRequest : IMessengerRequest
    {
        public MessengerRequest(DbDataReader reader)
        {
            Id = reader.ReadData<uint>("id");
            Username = reader.ReadData<string>("username");
            Figure = reader.ReadData<string>("figure");
            Motto = reader.ReadData<string>("motto");
        }

        public MessengerRequest(IPlayer player)
        {
            Id = player.Id;
            Username = player.Username;
            Figure = player.Figure;
            Motto = player.Motto;
        }

        public void Compose(ServerPacket message)
        {
            message.WriteInt(Id);
            message.WriteString(Username);
            message.WriteString(Figure);
        }

        public uint Id { get; }
        public string Username { get; }
        public string Figure { get; }
        public string Motto { get; }
    }
}
