﻿using AliasPro.API.Sessions;
using AliasPro.API.Sessions.Models;
using DotNetty.Transport.Channels;
using System.Collections.Generic;

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

        public ICollection<ISession> Sessions =>
            _sessionRepository.Sessions;
    }
}
