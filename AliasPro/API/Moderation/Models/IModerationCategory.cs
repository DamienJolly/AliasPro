using System.Collections.Generic;

namespace AliasPro.API.Moderation.Models
{
    public interface IModerationCategory
    {
        int Id { get; set; }
        string Code { get; set; }
		ICollection<IModerationTopic> Topics { get; }

		bool TryGetTopic(int topicId, out IModerationTopic topic);
	}
}
