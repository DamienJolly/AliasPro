using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Players.Packets.Composers
{
    public class UserCreditsComposer : IMessageComposer
    {
        private readonly int _credits;

        public UserCreditsComposer(int credits)
        {
            _credits = credits;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UserCreditsMessageComposer);
            message.WriteString(_credits + ".0");
            return message;
        }
    }
}
