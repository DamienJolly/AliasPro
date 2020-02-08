using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages.Protocols;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Communication.Messages
{
    public class MessageHandler
    {
        private readonly ILogger<MessageHandler> logger;
        private readonly Dictionary<short, IMessageEvent> messageEvents;

        public MessageHandler(
            ILogger<MessageHandler> logger,
            IEnumerable<IMessageEvent> messageEvents)
        {
            this.logger = logger;

            this.messageEvents = messageEvents.ToDictionary(x => x.Header, x => x);

            this.logger.LogDebug("Loaded {0} message events", this.messageEvents.Count);
        }

        public async Task TryHandleAsync(ISession session, ClientMessage message)
        {
            if (messageEvents.TryGetValue(message.Id, out IMessageEvent messageEvent))
            {
                logger.LogInformation("Handling header {0}", message.Id);
                await messageEvent.RunAsync(session, message);
            }
            else
            {
                logger.LogWarning("Unregistered header {0}", message.Id);
            }
        }
    }
}
