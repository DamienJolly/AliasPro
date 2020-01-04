using AliasPro.API.Database;
using AliasPro.API.Groups.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Protocol;
using AliasPro.Utilities;
using System.Collections.Generic;
using System.Data.Common;

namespace AliasPro.Rooms.Models
{
    internal class RoomData : IRoomData
    {
        internal RoomData(DbDataReader reader)
        {
            Id = reader.ReadData<uint>("id");
            OwnerId = reader.ReadData<int>("owner");
            OwnerName = reader.ReadData<string>("username");
            Name = reader.ReadData<string>("name");
            Description = reader.ReadData<string>("caption");
            Password = reader.ReadData<string>("password");
            ModelName = reader.ReadData<string>("model_name");
            MaxUsers = reader.ReadData<int>("max_users");
            TradeType = reader.ReadData<int>("trade_type");
            CategoryId = reader.ReadData<int>("category_id");
            Score = reader.ReadData<int>("score");
            WallPaint = reader.ReadData<string>("wall_paint");
            FloorPaint = reader.ReadData<string>("floor_paint");
            BackgroundPaint = reader.ReadData<string>("landscape_paint");
            WallHeight = reader.ReadData<int>("wall_height");
            Tags = new List<string>();

            string tags = reader.ReadData<string>("tags");
            foreach (string tag in tags.Split(";"))
            {
                if (string.IsNullOrWhiteSpace(tag))
                    continue;

                Tags.Add(tag);
            }
        }

        internal RoomData(IRoomData data)
        {
            Id = data.Id;
            OwnerId = data.OwnerId;
            OwnerName = data.OwnerName;
            Name = data.Name;
            Description = data.Description;
            Password = data.Password;
            ModelName = data.ModelName;
            MaxUsers = data.MaxUsers;
            TradeType = data.TradeType;
            CategoryId = data.CategoryId;
            Score = data.Score;
            Tags = data.Tags;
			Group = data.Group;
            Promotion = data.Promotion;
            Settings = data.Settings;
            WallPaint = data.WallPaint;
            FloorPaint = data.FloorPaint;
            BackgroundPaint = data.BackgroundPaint;
            WallHeight = data.WallHeight;
        }

        public void Compose(ServerPacket message)
        {
			message.WriteInt(Id);
			message.WriteString(Name);
			message.WriteInt(OwnerId);
			message.WriteString(OwnerName);
			message.WriteInt(DoorState);
			message.WriteInt(UsersNow);
			message.WriteInt(MaxUsers);
			message.WriteString(Description);
			message.WriteInt(TradeType);
			message.WriteInt(Score);
            message.WriteInt(0); // dunno?
            message.WriteInt(CategoryId);
			message.WriteInt(Tags.Count);
            foreach (string tag in Tags)
				message.WriteString(tag);
			message.WriteInt(EnumType);

			if (Group != null)
			{
				message.WriteInt(Group.Id);
				message.WriteString(Group.Name);
				message.WriteString(Group.Badge);
			}

            if (IsPromoted)
            {
                message.WriteString(Promotion.Title);
                message.WriteString(Promotion.Description);
                message.WriteInt((Promotion.EndTimestamp - (int)UnixTimestamp.Now) / 60);
            }
        }

        public uint Id { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string ModelName { get; set; }
        public int UsersNow { get; set; } = 0;
        public int MaxUsers { get; set; }
        public int CategoryId { get; set; }
        public int TradeType { get; set; }
        public int DoorState { get; set; } = 0;
        public int Score { get; set; }
        public string WallPaint { get; set; }
        public string FloorPaint { get; set; }
        public string BackgroundPaint { get; set; }
        public int WallHeight { get; set; }
        public IList<string> Tags { get; set; }
		public IGroup Group { get; set; }

        public int EnumType
        {
            get
            {
				int type = 0;
				//if (Data.Image != null)
				//	RoomType += 1;
				if (Group != null)
					type += 2;
				if (IsPromoted)
                    type += 4;
				//if (Data.Type == "private")
					type += 8;
				//if (Data.Allowpets)
				//	RoomType += 16;
				return type;
			}
        }

        public bool IsPromoted =>
            Promotion != null && Promotion.EndTimestamp > (int)UnixTimestamp.Now;

        public IRoomModel RoomModel { get; set; }
        public IRoomSettings Settings { get; set; }
        public IRoomPromotion Promotion { get; set; }
    }
}
