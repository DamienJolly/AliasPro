using DotNetty.Transport.Channels;

namespace AliasPro.Sessions
{
    public interface ISessionController
    {
        ISession GetSession(IChannelId channelId);
        bool TryGetSession(IChannelId channelId, out ISession session);
        void CacheSession(IChannelHandlerContext channel);
        void RemoveFromCache(IChannelId channelId);
    }
}
