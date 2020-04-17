using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages.Protocols;
using Microsoft.Extensions.Logging;
using System;
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

            this.messageEvents = messageEvents.ToDictionary(messageEvent => messageEvent.Header);

            logger.LogInformation("Loaded {0} message events", this.messageEvents.Count);
        }

        public async Task TryHandleAsync(ISession session, ClientMessage message)
        {
            try
            {
                if (messageEvents.TryGetValue(message.Id, out IMessageEvent messageEvent))
                {
                    logger.LogWarning("Registered header {0}", message.Id);
                    await messageEvent.RunAsync(session, message);
                }
                else
                {
                    logger.LogError("Unregistered header {0}", message.Id);
                }
            }
            catch (Exception e)
            {
                logger.LogError("Packet Error! \n" + e);
            }
        }
    }
}
