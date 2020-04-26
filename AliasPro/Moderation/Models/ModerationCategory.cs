using AliasPro.API.Database;
using AliasPro.API.Moderation.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace AliasPro.Moderation.Models
{
	internal class ModerationCategory : IModerationCategory
	{
		private readonly IDictionary<int, IModerationTopic> _topics;

		internal ModerationCategory(DbDataReader reader, IDictionary<int, IModerationTopic> topics)
		{
			Id = reader.ReadData<int>("id");
			Code = reader.ReadData<string>("code");
			_topics = topics;
		}

		public int Id { get; set; }
		public string Code { get; set; }

		public ICollection<IModerationTopic> Topics =>
			_topics.Values;

		public bool TryGetTopic(int topicId, out IModerationTopic topic) =>
			_topics.TryGetValue(topicId, out topic);
	}
}
