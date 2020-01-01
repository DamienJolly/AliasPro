using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AliasPro.Network.Events
{
    internal class EventProvider : IEventProvider
    {
        private readonly ILogger<EventProvider> _logger;
        private readonly IDictionary<short, IAsyncPacket> _events;

        public EventProvider(ILogger<EventProvider> logger, IEnumerable<IAsyncPacket> events)
        {
            _logger = logger;
            _events = events.ToDictionary(x => x.Header, x => x);
        }

        public void Handle(ISession session, IClientPacket clientPacket)
        {
            if (_events.TryGetValue(clientPacket.Header, out IAsyncPacket eventHandler))
            {
                if (Debugger.IsAttached)
                    _logger.LogInformation($"Executing {eventHandler.GetType().Name} for header: {clientPacket.Header}.");

                try
                {
                    eventHandler.HandleAsync(session, clientPacket);
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(e);
                }
            }
            else
            {
                if (Debugger.IsAttached)
                    _logger.LogWarning($"Unable to handle packet: {clientPacket.Header}");
            }
        }
    }
}
