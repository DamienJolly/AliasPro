using System.Collections.Generic;

namespace AliasPro.Players.Components
{
    public class IgnoreComponent
	{
		private readonly IDictionary<int, string> _ignoredUsers;

		public IgnoreComponent(
			IDictionary<int, string> ignoredUsers)
        {
			_ignoredUsers = ignoredUsers;
		}

		public bool TryGetIgnoredUser(int userId) => 
			_ignoredUsers.ContainsKey(userId);

		public bool TryAdd(int userId, string username)
		{
			if (_ignoredUsers.ContainsKey(userId))
				return false;

			_ignoredUsers.Add(userId, username);
			return true;
		}

		public bool TryRemove(int userId) => 
			_ignoredUsers.Remove(userId);

		public ICollection<string> IgnoredUsers =>
			_ignoredUsers.Values;
	}
}
