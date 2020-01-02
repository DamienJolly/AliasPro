using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Types;
using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Rooms.Components
{
    public class ItemsComponent
    {
        private readonly IRoom _room;
        private readonly IDictionary<uint, IItem> _items;

        internal ItemsComponent(IRoom room, IDictionary<uint, IItem> items)
        {
            _room = room;
            _items = items;

            InitializeItems();
        }

        public void InitializeItems()
        {
            foreach (IItem item in _items.Values)
            {
                item.CurrentRoom = _room;
                _room.RoomGrid.AddItem(item);
            }
        }

        public void TriggerWired(WiredInteractionType interaction, params object[] args)
        {
            foreach (IItem trigger in WiredTriggers)
            {
                if (trigger.ItemData.WiredInteractionType != interaction) continue;

                trigger.WiredInteraction.OnTrigger(args);
            }
        }

        public void TriggerEffects(IRoomTile roomTile, params object[] args)
        {
            foreach (IItem condition in roomTile.WiredConditions)
            {
                if (!condition.WiredInteraction.OnTrigger(args))
                    return;
            }

            // todo: addons

            foreach (IItem effect in roomTile.WiredEffects)
            {
                effect.WiredInteraction.OnTrigger(args);
            }
        }

        internal void AddItem(IItem item)
        {
            if (!_items.ContainsKey(item.Id))
            {
                _items.Add(item.Id, item);
            }
        }

        internal void RemoveItem(uint itemId) =>
            _items.Remove(itemId);

        internal bool TryGetItem(uint itemId, out IItem item) =>
            _items.TryGetValue(itemId, out item);

        internal ICollection<IItem> Items =>
            _items.Values;

        internal ICollection<IItem> FloorItems =>
            _items.Values.Where(item => item.ItemData.Type == "s").ToList();

        internal ICollection<IItem> WallItems =>
            _items.Values.Where(item => item.ItemData.Type == "i").ToList();

        internal ICollection<IItem> WiredTriggers =>
            _items.Values.Where(item => item.ItemData.InteractionType == ItemInteractionType.WIRED_TRIGGER).ToList();

        internal ICollection<IItem> WiredEffects =>
            _items.Values.Where(item => item.ItemData.InteractionType == ItemInteractionType.WIRED_EFFECT).ToList();


        internal IDictionary<uint, string> GetItemOwners
        {
            get
            {
                IDictionary<uint, string> itemOwners =
                new Dictionary<uint, string>();

                foreach (IItem item in _items.Values)
                {
                    if (!itemOwners.ContainsKey(item.PlayerId))
                    {
                        itemOwners.Add(item.PlayerId, item.PlayerUsername);
                    }
                }

                return itemOwners;
            }
        }
    }
}
