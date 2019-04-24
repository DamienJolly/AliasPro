using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using System.Collections.Generic;

namespace AliasPro.API.Rooms.Models
{
    public interface IRoomTile
    {
        IRoomPosition Position { get; }
        ICollection<BaseEntity> Entities { get; }
        ICollection<IItem> Items { get; }
        ICollection<IItem> WiredEffects { get; }
        ICollection<IItem> WiredConditions { get; }
        IItem TopItem { get; }
        double Height { get; }

        IRoomPosition PositionInFront(int rotation);
        bool IsValidTile(BaseEntity entity, bool final = false);
        bool CanRoll(IItem item);
        bool CanStack(IItem item);
        void AddItem(IItem item);
        void RemoveItem(uint itemId);
        void AddEntity(BaseEntity entity);
        void RemoveEntity(int entityId);
    }
}
