using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Interaction;
using AliasPro.Items.Types;
using AliasPro.Utilities;
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
            foreach (IItem trigger in GetItemsByType(ItemInteractionType.WIRED_TRIGGER))
            {
                if (trigger.ItemData.WiredInteractionType != interaction) continue;

                trigger.WiredInteraction.Execute(args);
            }
        }

        public void TriggerEffects(IRoomTile roomTile, params object[] args)
        {
            foreach (IItem condition in roomTile.WiredConditions)
            {
                if (!condition.WiredInteraction.TryHandle(args))
                    return;
            }

            bool hasRandom = false;
            IList<IItem> effects = roomTile.WiredEffects.ToList();

            foreach (IItem extra in roomTile.WiredExtras)
            {
                if (extra.Interaction is InteractionWiredExtraUnseen extraUnseen)
                {
                    effects.Shuffle();

                    IItem effect = extraUnseen.GetUnseenEffect(effects);
                    if (effect == null)
                        continue;

                    effect.WiredInteraction.Execute(args);
                    return;
                }
                else if (extra.Interaction is InteractionWiredExtraRandom extraRandom)
                {
                    effects.Shuffle();
                    hasRandom = true;
                    break;
                }
            }

            foreach (IItem effect in effects)
            {
                effect.WiredInteraction.Execute(args);
                if (hasRandom)
                    break;
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

        internal ICollection<IItem> GetItemsByType(ItemInteractionType type) =>
            _items.Values.Where(item => item.ItemData.InteractionType == type).ToList();

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
