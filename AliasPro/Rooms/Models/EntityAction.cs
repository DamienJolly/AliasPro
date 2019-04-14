using AliasPro.API.Rooms.Models;
using System.Collections.Generic;
using System.Text;

namespace AliasPro.Rooms.Models
{
    internal class EntityAction : IEntityAction
    {
        private readonly IDictionary<string, string> _activeStatuses;

        internal EntityAction()
        {
            _activeStatuses = new Dictionary<string, string>();
        }

        public void AddStatus(string key, string value)
        {
            RemoveStatus(key);
            _activeStatuses.Add(key, value);
        }

        public void RemoveStatus(string key) =>
            _activeStatuses.Remove(key);

        public bool HasStatus(string key) =>
            _activeStatuses.ContainsKey(key);

        public string StatusToString
        {
            get
            {
                StringBuilder builder = new StringBuilder("/");
                foreach (var status in _activeStatuses)
                {
                    builder.Append(status.Key);

                    if (!string.IsNullOrEmpty(status.Value))
                        builder.Append(" " + status.Value);

                    builder.Append("/");
                }
                builder.Append("/");
                return builder.ToString();
            }
        }
    }
}
