using AliasPro.API.Navigator;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomSettingsSaveEvent : IMessageEvent
    {
        public short Header => Incoming.RoomSettingsSaveMessageEvent;

        private readonly IRoomController _roomController;

        public RoomSettingsSaveEvent(
            IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            uint roomId = (uint)message.ReadInt();

            IRoom room = await _roomController.LoadRoom(roomId);
            if (room == null)
                return;

            if (room.OwnerId != session.Player.Id) return;

            string name = message.ReadString();
            if (name.Length > 60)
                name.Substring(0, 60);

            if (name.Length <= 0)
            {
                await session.SendPacketAsync(new RoomEditSettingsErrorComposer(room.Id, RoomEditSettingsErrorComposer.ROOM_NAME_MISSING));
            }
            else
            {
                room.Name = name;
            }

            room.Description = message.ReadString();
            
            int doorState = message.ReadInt();
            if (doorState < 0 || doorState > 3) doorState = 0;

            string password = message.ReadString();

            if (doorState == 2 && password.Length <= 0)
            {
                await session.SendPacketAsync(new RoomEditSettingsErrorComposer(room.Id, RoomEditSettingsErrorComposer.PASSWORD_REQUIRED));
            }
            else
            {
                room.DoorState = doorState;
                room.Password = password;
            }

            int maxUsers = message.ReadInt();
            if ((maxUsers % 5) != 0 || 
                maxUsers < 10 || 
                maxUsers > 50)
                maxUsers = 10;

            room.MaxUsers = maxUsers;

            int categoryId = message.ReadInt();
            //todo: fix
            //if (_navigatorController.TryGetRoomCategory((uint)categoryId, out INavigatorCategory category))
           //     room.CategoryId = categoryId;

            room.Tags.Clear();
            int amount = message.ReadInt();
            for (int i = 0; i < amount && i < 2; i++)
            {
                string tag = message.ReadString();
                if (string.IsNullOrWhiteSpace(tag))
                    continue;

                if (tag.Length > 15)
                {
                    await session.SendPacketAsync(new RoomEditSettingsErrorComposer(room.Id, RoomEditSettingsErrorComposer.TAGS_TOO_LONG));
                    continue;
                }
                
                room.Tags.Add(tag);
            }
            
            int tradeType = message.ReadInt();
            if (tradeType < 0 || tradeType > 2) tradeType = 0;

            room.TradeType = tradeType;

            room.Settings.AllowPets = message.ReadBoolean();
            room.Settings.AllowPetsEat = message.ReadBoolean();
            room.Settings.RoomBlocking = message.ReadBoolean();
            room.Settings.HideWalls = message.ReadBoolean();

            int wallThickness = message.ReadInt();
            if (wallThickness < -2 || wallThickness > 1) wallThickness = 0;

            int floorThickness = message.ReadInt();
            if (floorThickness < -2 || floorThickness > 1) floorThickness = 0;

            int whoMutes = message.ReadInt();
            if (whoMutes < 0 || whoMutes > 1) whoMutes = 0;

            int whoKicks = message.ReadInt();
            if (whoKicks < 0 || whoKicks > 2) whoKicks = 0;

            int whoBans = message.ReadInt();
            if (whoBans < 0 || whoBans > 1) whoBans = 0;

            room.Settings.WallThickness = wallThickness;
            room.Settings.FloorThickness = floorThickness;
            room.Settings.WhoMutes = whoMutes;
            room.Settings.WhoKicks = whoKicks;
            room.Settings.WhoBans = whoBans;

            int chatMode = message.ReadInt();
            if (chatMode < 0 || chatMode > 1) chatMode = 0;

            int chatSize = message.ReadInt();
            if (chatSize < 0 || chatSize > 2) chatSize = 1;

            int chatSpeed = message.ReadInt();
            if (chatSpeed < 0 || chatSpeed > 2) chatSpeed = 1;

            int chatDistance = message.ReadInt();
            if (chatDistance < 0) chatDistance = 1;
            else if (chatDistance > 100) chatDistance = 14;

            int chatFlood = message.ReadInt();
            if (chatFlood < 0 || chatFlood > 2) chatFlood = 1;

            room.Settings.ChatMode = chatMode;
            room.Settings.ChatSize = chatSize;
            room.Settings.ChatSpeed = chatSpeed;
            room.Settings.ChatDistance = chatDistance;
            room.Settings.ChatFlood = chatFlood;

            if (room.Loaded)
            {
                await room.SendPacketAsync(new RoomVisualizationSettingsComposer(room.Settings));
                await room.SendPacketAsync(new RoomChatSettingsComposer(room.Settings));
                await room.SendPacketAsync(new RoomSettingsUpdatedComposer(room.Id));
            }

            await session.SendPacketAsync(new RoomSettingsSavedComposer(room.Id));
        }
    }
}
