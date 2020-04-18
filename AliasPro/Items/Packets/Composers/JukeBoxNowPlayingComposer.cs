using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Items.Packets.Composers
{
    public class JukeBoxNowPlayingComposer : IMessageComposer
    {
        private readonly int _songId;
        private readonly int _currentIndex;
        private readonly int _timeElapsed = 0;

        public JukeBoxNowPlayingComposer(int songId = -1, int currentIndex = -1, int timeElapsed = 0)
        {
            _songId = songId;
            _currentIndex = currentIndex;
            _timeElapsed = timeElapsed;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.JukeBoxNowPlayingMessageComposer);
            message.WriteInt(_songId);
            message.WriteInt(_currentIndex);
            message.WriteInt(_songId);
            message.WriteInt(0); //dunno?
            message.WriteInt(_timeElapsed);
            return message;
        }
    }
}
