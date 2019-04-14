namespace AliasPro.API.Rooms.Models
{
    public interface IEntityAction
    {
        string StatusToString { get; }

        void AddStatus(string key, string value);
        void RemoveStatus(string key);
        bool HasStatus(string key);
    }
}
