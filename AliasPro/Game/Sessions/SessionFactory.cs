using AliasPro.Game.Sessions.Models;
using DotNetty.Transport.Channels;
using System.Threading;

namespace AliasPro.Game.Sessions
{
    public class SessionFactory
    {
        private int sessionIdCounter;

        public SessionFactory()
        {
            sessionIdCounter = 0;
        }

        public Session CreateSession(IChannel channel)
        {
            return new Session(channel, Interlocked.Increment(ref sessionIdCounter));
        }
    }
}
