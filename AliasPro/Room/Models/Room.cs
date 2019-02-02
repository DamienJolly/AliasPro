using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace AliasPro.Room.Models
{
    using Entities;
    using Gamemap;
    using Tasks;
    using Sessions;
    using System.Threading.Tasks;
    using AliasPro.Network.Protocol;
    using AliasPro.Room.Packets.Outgoing;
    using AliasPro.Network.Events;
    using AliasPro.Room.Models.Item;

    internal class Room : IRoom, IDisposable
    {
        private ActionBlock<DateTimeOffset> task;

        internal Room(IRoomData roomData, IRoomModel model)
        {
            RoomData = roomData;
            RoomModel = model;

            RoomMap = new RoomMap(RoomModel);
            EntityHandler = new EntityHandler(this);
            ItemHandler = new ItemHandler(this);

            SetupRoomCycle();
        }
        
        public RoomMap RoomMap { get; set; }
        public IRoomData RoomData { get; set; }
        public IRoomModel RoomModel { get; set; }
        public EntityHandler EntityHandler { get; set; }
        public ItemHandler ItemHandler { get; set; }
        
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
            EntityHandler.RemoveEntity(entityId);
            session.Entity = null;
            session.CurrentRoom = null;

            if (!EntityHandler.HasUserEntities)
            {
                //Todo: unload room
                //StopRoomCycle();
            }
        }
        
        public void LoadRoomItems(IDictionary<uint, IRoomItem> items)
        {
            ItemHandler.RoomItems = items;
        }
        
        private void SetupRoomCycle()
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

        public Task SendAsync(IPacketComposer serverPacket) => EntityHandler.SendAsync(serverPacket);

        public Task AddEntity(BaseEntity entity) => EntityHandler.AddEntity(entity);

        public Task AddItem(IRoomItem item) => ItemHandler.AddItem(item);

        public void Dispose()
        {
            StopRoomCycle();
            EntityHandler.Dispose();
        }
    }

    public interface IRoom
    {
        RoomMap RoomMap { get; set; }
        IRoomData RoomData { get; set; }
        IRoomModel RoomModel { get; set; }
        EntityHandler EntityHandler { get; set; }
        ItemHandler ItemHandler { get; set; }
        void OnChat(string text, int colour, BaseEntity entity);
        void LeaveRoom(ISession session);
        void LoadRoomItems(IDictionary<uint, IRoomItem> items);
        Task SendAsync(IPacketComposer serverPacket);
        Task AddEntity(BaseEntity entity);
    }
}
