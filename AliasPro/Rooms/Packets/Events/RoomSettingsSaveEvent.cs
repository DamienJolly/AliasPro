using AliasPro.API.Navigator;
using AliasPro.API.Navigator.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomSettingsSaveEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RoomSettingsSaveMessageEvent;

        private readonly IRoomController _roomController;
        private readonly INavigatorController _navigatorController;

        public RoomSettingsSaveEvent(IRoomController roomController, INavigatorController navigatorController)
        {
            _roomController = roomController;
            _navigatorController = navigatorController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            uint roomId = (uint)clientPacket.ReadInt();
            if (!_roomController.TryGetRoom(roomId, out IRoom room))
                return;

            if (!room.Rights.IsOwner(session.Player.Id)) return;

            string name = clientPacket.ReadString();
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

            room.Description = clientPacket.ReadString();
            
            int doorState = clientPacket.ReadInt();
            if (doorState < 0 || doorState > 3) doorState = 0;

            string password = clientPacket.ReadString();

            if (doorState == 2 && password.Length <= 0)
            {
                await session.SendPacketAsync(new RoomEditSettingsErrorComposer(room.Id, RoomEditSettingsErrorComposer.PASSWORD_REQUIRED));
            }
            else
            {
                room.DoorState = doorState;
                room.Password = password;
            }

            int maxUsers = clientPacket.ReadInt();
            if ((maxUsers % 5) != 0 || 
                maxUsers < 10 || 
                maxUsers > 50)
                maxUsers = 10;

            room.MaxUsers = maxUsers;

            int categoryId = clientPacket.ReadInt();
            //todo: fix
            //if (_navigatorController.TryGetRoomCategory((uint)categoryId, out INavigatorCategory category))
           //     room.CategoryId = categoryId;

            room.Tags.Clear();
            int amount = clientPacket.ReadInt();
            for (int i = 0; i < amount && i < 2; i++)
            {
                string tag = clientPacket.ReadString();
                if (string.IsNullOrWhiteSpace(tag))
                    continue;

                if (tag.Length > 15)
                {
                    await session.SendPacketAsync(new RoomEditSettingsErrorComposer(room.Id, RoomEditSettingsErrorComposer.TAGS_TOO_LONG));
                    continue;
                }
                
                room.Tags.Add(tag);
            }
            
            int tradeType = clientPacket.ReadInt();
            if (tradeType < 0 || tradeType > 2) tradeType = 0;

            room.TradeType = tradeType;

            room.Settings.AllowPets = clientPacket.ReadBool();
            room.Settings.AllowPetsEat = clientPacket.ReadBool();
            room.Settings.RoomBlocking = clientPacket.ReadBool();
            room.Settings.HideWalls = clientPacket.ReadBool();

            int wallThickness = clientPacket.ReadInt();
            if (wallThickness < -2 || wallThickness > 1) wallThickness = 0;

            int floorThickness = clientPacket.ReadInt();
            if (floorThickness < -2 || floorThickness > 1) floorThickness = 0;

            int whoMutes = clientPacket.ReadInt();
            if (whoMutes < 0 || whoMutes > 1) whoMutes = 0;

            int whoKicks = clientPacket.ReadInt();
            if (whoKicks < 0 || whoKicks > 1) whoKicks = 0;

            int whoBans = clientPacket.ReadInt();
            if (whoBans < 0 || whoBans > 1) whoBans = 0;

            room.Settings.WallThickness = wallThickness;
            room.Settings.FloorThickness = floorThickness;
            room.Settings.WhoMutes = whoMutes;
            room.Settings.WhoKicks = whoKicks;
            room.Settings.WhoBans = whoBans;

            int chatMode = clientPacket.ReadInt();
            if (chatMode < 0 || chatMode > 1) chatMode = 0;

            int chatSize = clientPacket.ReadInt();
            if (chatSize < 0 || chatSize > 2) chatSize = 1;

            int chatSpeed = clientPacket.ReadInt();
            if (chatSpeed < 0 || chatSpeed > 2) chatSpeed = 1;

            int chatDistance = clientPacket.ReadInt();
            if (chatDistance < 0) chatDistance = 1;
            else if (chatDistance > 100) chatDistance = 14;

            int chatFlood = clientPacket.ReadInt();
            if (chatFlood < 0 || chatFlood > 2) chatFlood = 1;

            room.Settings.ChatMode = chatMode;
            room.Settings.ChatSize = chatSize;
            room.Settings.ChatSpeed = chatSpeed;
            room.Settings.ChatDistance = chatDistance;
            room.Settings.ChatFlood = chatFlood;

            await room.SendAsync(new RoomVisualizationSettingsComposer(room.Settings));
            await room.SendAsync(new RoomChatSettingsComposer(room.Settings));
            await room.SendAsync(new RoomSettingsUpdatedComposer(room.Id));

            await session.SendPacketAsync(new RoomSettingsSavedComposer(room.Id));
        }
    }
}
