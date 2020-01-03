using AliasPro.API.Sessions.Models;
using AliasPro.Sessions.Models;
using DotNetty.Transport.Channels;
using System.Collections.Generic;

namespace AliasPro.Sessions
{
    internal class SessionRepository
    {
        private readonly IDictionary<IChannelId, ISession> _cachedSessions;

        public SessionRepository()
        {
            _cachedSessions = new Dictionary<IChannelId, ISession>();
        }

        internal ISession GetSession(IChannelId channelId) =>
            _cachedSessions[channelId];

        internal bool TryGetSession(IChannelId channelId, out ISession session) =>
            _cachedSessions.TryGetValue(channelId, out session);

        internal void CacheSession(IChannelHandlerContext channel) =>
            _cachedSessions.Add(channel.Channel.Id, new Session(channel));

        internal void RemoveFromCache(IChannelId channelId) =>
            _cachedSessions.Remove(channelId);

        internal ICollection<ISession> Sessions => 
            _cachedSessions.Values;
    }
}
