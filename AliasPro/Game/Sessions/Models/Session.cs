using DotNetty.Transport.Channels;

namespace AliasPro.Game.Sessions.Models
{
    public class Session
    {
        public IChannel Channel { get; }
        public int SessionId { get; }

        public Session(IChannel channel, int sessionId)
        {
            Channel = channel;
            SessionId = sessionId;
        }
    }
}
