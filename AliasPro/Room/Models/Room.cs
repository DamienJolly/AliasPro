﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AliasPro.Room.Models
{
    using Entities;
    using Gamemap;
    using Tasks;
    using Packets.Outgoing;
    using Network.Events;
    using Models.Item;
    using AliasPro.Item.Models;

    internal class Room : IRoom
    {
        private ActionBlock<DateTimeOffset> task;
        
        public bool isLoaded { get; set; } = false;
        public EntityHandler EntityHandler { get; set; }
        public ItemHandler ItemHandler { get; set; }
        public RoomMap RoomMap { get; set; }
        public IRoomData RoomData { get; set; }
        public IRoomModel RoomModel { get; set; }

        internal Room(IRoomData roomData, IRoomModel model)
        {
            RoomData = roomData;
            RoomModel = model;

            RoomMap = new RoomMap(this, RoomModel);
            EntityHandler = new EntityHandler(this);
            ItemHandler = new ItemHandler(this);
        }

        public async void OnChat(string text, int colour, BaseEntity entity)
        {
            if (colour == 1 || colour == -1 || colour == 2)
            {
                colour = 0;
            }

            if (text.Length > 100)
            {
                text = text.Substring(0, 100);
            }

            await SendAsync(new AvatarChatComposer(entity.Id, text, 0, colour));
        }

        public async Task AddEntity(BaseEntity entity)
        {
            EntityHandler.AddEntity(entity);
            await SendAsync(new EntitiesComposer(entity));
            await SendAsync(new EntityUpdateComposer(entity));
        }

        public async Task RemoveEntity(int entityId)
        {
            EntityHandler.RemoveEntity(entityId);
            await SendAsync(new EntityRemoveComposer(entityId));
        }
        
        public void LoadRoomItems(IDictionary<uint, IItem> items)
        {
            foreach (IItem item in items.Values)
            {
                if (RoomMap.TryGetRoomTile(item.Position.X, item.Position.Y, out RoomTile tile))
                {
                    tile.AddItem(item);
                }

                ItemHandler.AddItem(item);
            }
        }
        
        public async Task SendAsync(IPacketComposer packet)
        {
            foreach (BaseEntity entity in EntityHandler.Entities)
            {
                if (entity is UserEntity userEntity)
                {
                    await userEntity.Session.SendPacketAsync(packet);
                }
            }
        }

        public void SetupRoomCycle()
        {
            task = TaskHandler.PeriodicTaskWithDelay(time => Cycle(time), 500);
            task.Post(DateTimeOffset.Now);
        }

        private void StopRoomCycle()
        {
            task = null;
        }

        private void Cycle(DateTimeOffset timeOffset)
        {
            EntityHandler.Cycle(timeOffset);
        }
    }

    public interface IRoom
    {
        bool isLoaded { get; set; }
        EntityHandler EntityHandler { get; set; }
        ItemHandler ItemHandler { get; set; }
        RoomMap RoomMap { get; set; }
        IRoomData RoomData { get; set; }
        IRoomModel RoomModel { get; set; }

        Task AddEntity(BaseEntity entity);
        Task RemoveEntity(int entityId);
        void OnChat(string text, int colour, BaseEntity entity);
        void LoadRoomItems(IDictionary<uint, IItem> items);
        void SetupRoomCycle();
        Task SendAsync(IPacketComposer packet);
    }
}
