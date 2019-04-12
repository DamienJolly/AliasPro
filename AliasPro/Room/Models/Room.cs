using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AliasPro.Room.Models
{
    using Entities;
    using Gamemap;
    using Tasks;
    using Item;
    using Right;
    using Game;
    using AliasPro.Items.Models;
    using AliasPro.Room.Gamemap.Pathfinding;
    using AliasPro.API.Network.Events;
    using AliasPro.Room.Packets.Composers;
    using AliasPro.API.Items.Models;
    using AliasPro.Items.Types;

    internal class Room : IRoom, ITask
    {
        private readonly CancellationTokenSource _cancellationToken;
        private readonly object _loadLock = new object();

        public bool isLoaded { get; set; } = false;
        public EntityHandler EntityHandler { get; set; }
        public ItemHandler ItemHandler { get; set; }
        public GameHandler GameHandler { get; set; }
        public RightHandler RightHandler { get; set; }
        public RoomMap RoomMap { get; set; }
        public IRoomData RoomData { get; set; }
        public IRoomModel RoomModel { get; set; }

        internal Room(IRoomData roomData, IRoomModel model)
        {
            RoomData = roomData;
            RoomModel = model;

            _cancellationToken = new CancellationTokenSource();

            RoomMap = new RoomMap(this, RoomModel);
            EntityHandler = new EntityHandler(this);
            ItemHandler = new ItemHandler(this);
            GameHandler = new GameHandler(this);
            RightHandler = new RightHandler(this);
        }

        public async void SetupRoomCycle()
        {
            await TaskHandler.PeriodicAsyncTaskWithDelay(this, 500, _cancellationToken.Token);
        }

        private void StopRoomCycle()
        {
            using (_cancellationToken)
            {
                _cancellationToken.Cancel();
            }
        }

        public void Run()
        {
            lock(_loadLock)
            {
                if (isLoaded)
                {
                    try
                    {
                        EntityHandler.Cycle();
                        ItemHandler.Cycle();
                    }
                    catch
                    {
                        // room crashed
                    }
                }
            }
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

            ItemHandler.TriggerWired(WiredInteractionType.SAY_SOMETHING, entity, text);

            foreach (BaseEntity targetEntity in EntityHandler.Entities)
            {
                if (targetEntity == entity) continue;

                int newDir = targetEntity.Position.CalculateDirection(entity.Position);

                if (Math.Abs(newDir - targetEntity.BodyRotation) <= 2)
                    targetEntity.HeadRotation = newDir;

                targetEntity.DirOffsetTimer = 0;
            }

            await SendAsync(new AvatarChatComposer(entity.Id, text, 0, colour));
        }

        public async Task AddEntity(BaseEntity entity)
        {
            EntityHandler.AddEntity(entity);
            RoomMap.AddEntity(entity);

            ItemHandler.TriggerWired(WiredInteractionType.ENTER_ROOM, entity);

            await SendAsync(new EntitiesComposer(entity));
            await SendAsync(new EntityUpdateComposer(entity));
        }

        public async Task RemoveEntity(BaseEntity entity)
        {
            EntityHandler.RemoveEntity(entity);
            RoomMap.RemoveEntity(entity);
            GameHandler.LeaveTeam(entity);

            await SendAsync(new EntityRemoveComposer(entity.Id));
        }
        
        public void LoadRoomItems(IDictionary<uint, IItem> items)
        {
            foreach (IItem item in items.Values)
            {
                RoomMap.AddItem(item);
                item.CurrentRoom = this;
                ItemHandler.AddItem(item);
            }
        }

        public void LoadRoomRights(IDictionary<uint, string> rights)
        {
            foreach (var right in rights)
            {
                RightHandler.GiveRights(right.Key, right.Value);
            }
        }

        public Position GetPathToClosestEntity(Position position)
        {
            IList<Position> closestPath = new List<Position>();
            
            foreach (BaseEntity entity in EntityHandler.Entities)
            {
                IList<Position> pathToItem = PathFinder.FindPath(
                    entity,
                    RoomMap,
                    entity.Position, position);

                if (pathToItem.Count <= closestPath.Count)
                    closestPath = pathToItem;
            }

            return closestPath[closestPath.Count - 1];
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

        public void Dispose()
        {
            lock (_loadLock)
            {
                isLoaded = false;
                StopRoomCycle();
                //todo: do stuff
            }
        }
    }

    public interface IRoom
    {
        bool isLoaded { get; set; }
        EntityHandler EntityHandler { get; set; }
        ItemHandler ItemHandler { get; set; }
        GameHandler GameHandler { get; set; }
        RightHandler RightHandler { get; set; }
        RoomMap RoomMap { get; set; }
        IRoomData RoomData { get; set; }
        IRoomModel RoomModel { get; set; }

        Task AddEntity(BaseEntity entity);
        Task RemoveEntity(BaseEntity entity);
        void OnChat(string text, int colour, BaseEntity entity);
        void LoadRoomItems(IDictionary<uint, IItem> items);
        void LoadRoomRights(IDictionary<uint, string> rights);
        void SetupRoomCycle();
        Task SendAsync(IPacketComposer packet);
        void Dispose();
    }
}
