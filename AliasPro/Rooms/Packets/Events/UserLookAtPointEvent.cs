using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class UserLookAtPointEvent : IMessageEvent
    {
        public short Header => Incoming.UserLookAtPointMessageEvent;

        public Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) 
                return Task.CompletedTask;

            int x = message.ReadInt();
            int y = message.ReadInt();

            if (x == session.Entity.Position.X &&
                y == session.Entity.Position.Y)
                return Task.CompletedTask;

            if (session.Entity.Actions.HasStatus("mv") ||
                session.Entity.Actions.HasStatus("lay"))
                return Task.CompletedTask;


            int newDir = session.Entity.BodyRotation == 0 ? 8 : session.Entity.Position.CalculateDirection(x, y);
            bool headonly = Math.Abs(newDir - session.Entity.BodyRotation) <= 1;

            session.Entity.SetRotation(newDir, headonly);
            session.Entity.Unidle();
            session.Entity.DirOffsetTimer = 0;
            return Task.CompletedTask;
        }
    }
}
