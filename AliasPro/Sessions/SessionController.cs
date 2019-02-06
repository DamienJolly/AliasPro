using DotNetty.Transport.Channels;

namespace AliasPro.Sessions
{
    internal class SessionController : ISessionController
    {
        private readonly SessionRepository _sessionRepository;

        public SessionController(SessionRepository repository)
        {
            _sessionRepository = repository;
        }

        public ISession GetSession(IChannelId channelId) =>
            _sessionRepository.GetSession(channelId);

        public bool TryGetSession(IChannelId channelId, out ISession session) =>
            _sessionRepository.TryGetSession(channelId, out session);

        public void CacheSession(IChannelHandlerContext channel) =>
            _sessionRepository.CacheSession(channel);

        public void RemoveFromCache(IChannelId channelId) =>
            _sessionRepository.RemoveFromCache(channelId);
    }

    public interface ISessionController
    {
        ISession GetSession(IChannelId channelId);
        bool TryGetSession(IChannelId channelId, out ISession session);
        void CacheSession(IChannelHandlerContext channel);
        void RemoveFromCache(IChannelId channelId);
    }
}
