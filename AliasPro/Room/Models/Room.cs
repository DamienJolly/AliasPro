using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AliasPro.Room.Models
{
    using Entities;
    using Gamemap;
    using Tasks;
    using Sessions;
    using Packets.Outgoing;
    using Network.Events;
    using Models.Item;
    using AliasPro.Item.Models;

    internal class Room : IRoom, IDisposable
    {
        private readonly EntityHandler _entityHandler;
        private readonly ItemHandler _itemHandler;
        private ActionBlock<DateTimeOffset> task;

        internal Room(IRoomData roomData, IRoomModel model)
        {
            RoomData = roomData;
            RoomModel = model;

            RoomMap = new RoomMap(RoomModel);
            _entityHandler = new EntityHandler(this);
            _itemHandler = new ItemHandler(this);
        }

        public bool isLoaded { get; set; } = false;
        public RoomMap RoomMap { get; set; }
        public IRoomData RoomData { get; set; }
        public IRoomModel RoomModel { get; set; }
        public IDictionary<int, BaseEntity> Entities => _entityHandler.Entities;
        public IDictionary<uint, IItem> RoomItems => _itemHandler.RoomItems;

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

        public void LeaveRoom(ISession session)
        {
            int entityId = session.Entity.Id;
            _entityHandler.RemoveEntity(entityId);
            session.Entity = null;
            session.CurrentRoom = null;

            if (!_entityHandler.HasUserEntities)
            {
                //Todo: unload room
                //StopRoomCycle();
            }
        }

        public void LoadRoomItems(IDictionary<uint, IItem> items)
        {
            foreach (IItem item in items.Values)
            {
                if (RoomMap.TryGetRoomTile(item.Position.X, item.Position.Y, out RoomTile tile))
                {
                    tile.AddItem(item);
                }
            }
            _itemHandler.RoomItems = items;
        }

        public async Task AddItem(IItem item, bool newItem)
        {
            if (RoomMap.TryGetRoomTile(item.Position.X, item.Position.Y, out RoomTile tile))
            {
                System.Console.WriteLine("add");
                tile.AddItem(item);
                if (newItem)
                    await _itemHandler.AddItem(item);
            }
        }

        public async Task RemoveItem(IItem item, bool newItem)
        {
            if (RoomMap.TryGetRoomTile(item.Position.X, item.Position.Y, out RoomTile tile))
            {
                System.Console.WriteLine("remove");
                tile.RemoveItem(item);
                if (newItem)
                    await _itemHandler.RemoveItem(item);
            }
        }

        public async Task UpdateItem(IItem item, Position newPosition)
        {
            await RemoveItem(item, false);
            //todo: maybe add newPosition as a variable for items
            item.Position = newPosition;
            await AddItem(item, false);
            await _itemHandler.UpdateItem(item);
        }

        public async Task SendAsync(IPacketComposer packet)
        {
            foreach (BaseEntity entity in _entityHandler.Entities.Values)
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
            _entityHandler.Cycle(timeOffset);
        }
        
        public Task AddEntity(BaseEntity entity) => _entityHandler.AddEntity(entity);
        
        public void Dispose()
        {
            StopRoomCycle();
            _entityHandler.Dispose();
        }
    }

    public interface IRoom
    {
        bool isLoaded { get; set; }
        RoomMap RoomMap { get; set; }
        IRoomData RoomData { get; set; }
        IRoomModel RoomModel { get; set; }
        IDictionary<int, BaseEntity> Entities { get; }
        IDictionary<uint, IItem> RoomItems { get; }
        void OnChat(string text, int colour, BaseEntity entity);
        void LeaveRoom(ISession session);
        void LoadRoomItems(IDictionary<uint, IItem> items);
        void SetupRoomCycle();
        Task AddEntity(BaseEntity entity);
        Task AddItem(IItem item, bool newItem = true);
        Task RemoveItem(IItem item, bool newItem = true);
        Task UpdateItem(IItem item, Position newPosition);
        Task SendAsync(IPacketComposer packet);
    }
}
