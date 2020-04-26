using AliasPro.Moderation.Types;

namespace AliasPro.API.Moderation.Models
{
    public interface IModerationTopic
    {
		int Id { get; set; }
		string Name { get; set; }
		string Reply { get; set; }
	}
}
