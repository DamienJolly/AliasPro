using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Players.Packets.Events
{
    internal class SaveUserVolumesEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.SaveUserVolumesMessageEvent;
        
        public Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            session.Player.PlayerSettings.VolumeSystem = clientPacket.ReadInt();
            session.Player.PlayerSettings.VolumeFurni = clientPacket.ReadInt();
            session.Player.PlayerSettings.VolumeTrax = clientPacket.ReadInt();
            return Task.CompletedTask;
        }
    }
}
