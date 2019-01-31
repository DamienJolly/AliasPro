using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Network.Events
{
    using Protocol;
    using Sessions;

    internal class EventProvider : IEventProvider
    {
        private readonly ILogger<EventProvider> _logger;
        private readonly IDictionary<short, IAsyncPacket> _events;

        public EventProvider(ILogger<EventProvider> logger, IEnumerable<IAsyncPacket> events)
        {
            _logger = logger;
            _events = events.ToDictionary(x => x.Header, x => x);
        }

        public async Task Handle(ISession session, IClientPacket clientPacket)
        {
            if (_events.TryGetValue(clientPacket.Header, out IAsyncPacket eventHandler))
            {
                _logger.LogInformation($"Executing {eventHandler.GetType().Name} for header: {clientPacket.Header}.");
                await eventHandler.HandleAsync(session, clientPacket);
            }
            else
            {
                _logger.LogError($"Unable to handle packet: {clientPacket.Header}");
            }
        }
    }
}
