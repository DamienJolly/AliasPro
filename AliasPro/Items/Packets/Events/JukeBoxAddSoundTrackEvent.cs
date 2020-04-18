using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Rooms.Components;
using System.Threading.Tasks;

namespace AliasPro.Items.Packets.Events
{
    public class JukeBoxAddSoundTrackEvent : IMessageEvent
    {
        public short Header => Incoming.JukeBoxAddSoundTrackMessageEvent;

        private readonly IRoomController _roomController;
        private readonly IItemController _itemController;

        public JukeBoxAddSoundTrackEvent(
            IRoomController roomController,
            IItemController itemController)
        {
            _roomController = roomController;
            _itemController = itemController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;
            if (room == null)
                return;

            if (room.OwnerId != session.Player.Id)
                return;

            int itemId = message.ReadInt();
            int unknown = message.ReadInt(); //dunno?

            if (!session.Player.Inventory.TryGetItem((uint)itemId, out IItem item))
                return; 

            if (item.ItemData.InteractionType != ItemInteractionType.MUSICDISC)
                return;

            if (room.Trax.Songs.Count >= room.Trax.maxSongs)
            {
                await session.SendPacketAsync(new JukeBoxPlaylistFullComposer());
                return;
            }

            int.TryParse(item.ItemData.ExtraData, out int songId);
            if (!Program.GetService<IItemController>().TryGetSongDataById(songId, out ISongData song))
                return;

            if (!room.Trax.TryAddSong(itemId, song))
                return;

            await _roomController.AddRoomTraxAsync(room.Id, (int)item.Id);

            item.PlayerId = 0;
            item.PlayerUsername = "";
            session.Player.Inventory.RemoveItem(item.Id);
            await _itemController.UpdatePlayerItemAsync(item);
            await session.SendPacketAsync(new RemovePlayerItemComposer(item.Id));
            await session.SendPacketAsync(new InventoryRefreshComposer());

            await session.SendPacketAsync(new JukeBoxPlayListComposer(room.Trax.Songs, room.Trax.maxSongs));
            await session.SendPacketAsync(new JukeBoxMySongsComposer(room.Trax.AvailablePlayerSongs(session.Player.Inventory.Items)));
        }
    }
}
