﻿using System.Collections.Generic;
using System.Linq;
using System;

namespace AliasPro.Room.Models.Item
{
    using AliasPro.Item.Models;
    using AliasPro.Room.Gamemap;

    public class ItemHandler
    {
        private readonly IRoom _room;
        private readonly IDictionary<uint, IItem> _items;
        
        internal ItemHandler(IRoom room)
        {
            _room = room;
            _items = new Dictionary<uint, IItem>();
        }
        
        public void TriggerWired(WiredInteraction interaction, params object[] args)
        {
            foreach (IItem trigger in WiredTriggers)
            {
                if (trigger.ItemData.WiredInteractionType != interaction) continue;

                trigger.WiredInteraction.OnTrigger(args);
            }
        }

        public void TriggerEffects(RoomTile roomTile, params object[] args)
        {
            foreach (IItem effect in roomTile.WiredEffects)
            {
                effect.WiredInteraction.OnTrigger(args);
            }
        }

        public void Cycle(DateTimeOffset timeOffset)
        {
            foreach (IItem item in Items)
            {
                item.Interaction.OnCycle();
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
            _items.Values.Where(item => item.ItemData.InteractionType == ItemInteraction.WIRED_TRIGGER).ToList();

        internal ICollection<IItem> WiredEffects =>
            _items.Values.Where(item => item.ItemData.InteractionType == ItemInteraction.WIRED_EFFECT).ToList();


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
