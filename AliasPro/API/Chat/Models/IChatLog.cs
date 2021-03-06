﻿using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.API.Chat.Models
{
    public interface IChatLog
    {
        void Compose(ServerMessage message);

        int PlayerId { get; set; }
        string PlayerUsername { get; set; }
        int TargetId { get; set; }
        int RoomId { get; set; }
        int Timestamp { get; set; }
        string Message { get; set; }
        bool Highlighted { get; set; }
    }
}
