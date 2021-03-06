﻿using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Players.Packets.Events
{
    internal class SaveBlockCameraFollowEvent : IMessageEvent
    {
        public short Header => Incoming.SaveBlockCameraFollowMessageEvent;
        
        public Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            session.Player.PlayerSettings.CameraFollow = message.ReadBoolean();
            return Task.CompletedTask;
        }
    }
}
