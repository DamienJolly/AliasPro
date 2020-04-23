using AliasPro.API.Rooms.Games;
using AliasPro.API.Rooms.Games.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Trading.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Types;
using AliasPro.Rooms.Cycles;
using AliasPro.Rooms.Models;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Rooms.Types;

namespace AliasPro.API.Rooms.Entities
{
    public abstract class BaseEntity
    {
        protected BaseEntity(int id, int x, int y, int rotation, IRoom room, string name, string figure, PlayerGender gender, string motto, int score)
        {
            Id = id;
            Room = room;
            BodyRotation = rotation;
            HeadRotation = rotation;
            Position = new RoomPosition(x, y, 0);
            NextPosition = Position;
            GoalPosition = Position;
            Name = name;
            Figure = figure;
            Gender = gender;
            Motto = motto;
            Score = score;

            Actions = new EntityAction();
            RoomEntityCycle = new RoomEntityCycle(this);
        }
        
        public async void Unidle()
        {
            IdleTimer = 0;
            IsIdle = false;
            await Room.SendPacketAsync(new UserSleepComposer(this));
        }

        public void SetRotation(int rotation, bool headOnly = false)
        {
            HeadRotation = rotation;

            if (!headOnly)
                BodyRotation = rotation;
        }

        public void SetHandItem(int handItemId)
        {
            HandItemTimer = 0;
            HandItemId = handItemId;

            if (handItemId != 0)
                HandItemTimer = 240;
        }

        public async void SetEffect(int effectId, int duration = -1)
        {
            EffectId = effectId;
            EffectTimer = duration;

            await Room.SendPacketAsync(new UserEffectComposer(Id, EffectId));
        }

        public int Id { get; set; }
        public IRoom Room { get; set; }
        public int BodyRotation { get; set; }
        public int HeadRotation { get; set; }
        public IRoomPosition Position { get; set; }
        public IRoomPosition NextPosition { get; set; }
        public IRoomPosition GoalPosition { get; set; }

        public int DribbleDuration = 0;

        public string Name { get; set; }
        public string Figure { get; set; }
        public PlayerGender Gender { get; set; }
        public string Motto { get; set; }
        public int Score { get; set; }
        
        public IEntityAction Actions;
        public RoomEntityCycle RoomEntityCycle;

        public int DanceId { get; set; } = 0;
        public bool IsIdle { get; set; } = false;
        public bool IsSitting { get; set; } = false;
        public bool IsLaying { get; set; } = false;
        public bool NeedsUpdate { get; set; } = false;
        public bool IsKicked { get; set; } = false;

        public int DirOffsetTimer = 0;
        public int IdleTimer = 0;

        public int HandItemId = 0;
        public int HandItemTimer = 0;

        public int EffectId = 0;
        public int EffectTimer = -1;

        public int SignTimer = 0;

        public IGamePlayer GamePlayer = null;

		public ITrade Trade = null;

		public abstract void OnEntityJoin();
		public abstract void OnEntityLeave();
		public abstract void Cycle();
		public abstract void Compose(ServerMessage ServerMessage);
    }
}
