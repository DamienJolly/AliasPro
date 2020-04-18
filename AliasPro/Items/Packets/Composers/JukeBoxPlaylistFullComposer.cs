using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Packets.Composers
{
    public class JukeBoxPlaylistFullComposer : IMessageComposer
    {
        public ServerMessage Compose() =>
            new ServerMessage(Outgoing.JukeBoxPlaylistFullMessageComposer);
    }
}
