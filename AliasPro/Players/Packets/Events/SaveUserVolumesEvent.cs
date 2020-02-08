using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Players.Packets.Events
{
    internal class SaveUserVolumesEvent : IMessageEvent
    {
        public short Header => Incoming.SaveUserVolumesMessageEvent;
        
        public Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            session.Player.PlayerSettings.VolumeSystem = message.ReadInt();
            session.Player.PlayerSettings.VolumeFurni = message.ReadInt();
            session.Player.PlayerSettings.VolumeTrax = message.ReadInt();
            return Task.CompletedTask;
        }
    }
}
