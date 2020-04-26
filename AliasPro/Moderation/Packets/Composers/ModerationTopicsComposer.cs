using AliasPro.API.Moderation.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationTopicsComposer : IMessageComposer
    {
		private readonly ICollection<IModerationCategory> _categories;

		public ModerationTopicsComposer(ICollection<IModerationCategory> categories)
		{
			_categories = categories;
		}

		public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.ModerationTopicsMessageComposer);
			message.WriteInt(_categories.Count);
			foreach (IModerationCategory category in _categories)
			{
				message.WriteString(category.Code);
				message.WriteInt(category.Topics.Count);
				foreach (IModerationTopic topic in category.Topics)
				{
					message.WriteString(topic.Name);
					message.WriteInt(topic.Id);
					message.WriteString(""); //dunno?
				}
			}
			return message;
        }
    }
}
