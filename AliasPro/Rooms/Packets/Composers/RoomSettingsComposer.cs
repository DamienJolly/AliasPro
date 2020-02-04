using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomSettingsComposer : IMessageComposer
    {
        private readonly IRoomData _roomData;

        public RoomSettingsComposer(IRoomData roomData)
        {
            _roomData = roomData;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomSettingsMessageComposer);
            message.WriteInt((int)_roomData.Id);
            message.WriteString(_roomData.Name);
            message.WriteString(_roomData.Description);
            message.WriteInt(_roomData.DoorState);
            message.WriteInt(_roomData.CategoryId);
            message.WriteInt(_roomData.MaxUsers);
            message.WriteInt(50); //dunno?
            message.WriteInt(_roomData.Tags.Count);
            foreach (string tag in _roomData.Tags)
                message.WriteString(tag);
            message.WriteInt(_roomData.TradeType);
            message.WriteInt(_roomData.Settings.AllowPets ? 1 : 0);
            message.WriteInt(_roomData.Settings.AllowPetsEat ? 1 : 0);
            message.WriteInt(_roomData.Settings.RoomBlocking ? 1 : 0);
            message.WriteInt(_roomData.Settings.HideWalls ? 1 : 0);
            message.WriteInt(_roomData.Settings.WallThickness);
            message.WriteInt(_roomData.Settings.FloorThickness);
            message.WriteInt(_roomData.Settings.ChatMode);
            message.WriteInt(_roomData.Settings.ChatSize);
            message.WriteInt(_roomData.Settings.ChatSpeed);
            message.WriteInt(_roomData.Settings.ChatDistance);
            message.WriteInt(_roomData.Settings.ChatFlood);
            message.WriteBoolean(false); //dunno?
            message.WriteInt(_roomData.Settings.WhoMutes);
            message.WriteInt(_roomData.Settings.WhoKicks);
            message.WriteInt(_roomData.Settings.WhoBans);
            return message;
        }
    }
}
