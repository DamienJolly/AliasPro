using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Items.Packets.Events
{
    public class SongCodeEvent : IMessageEvent
    {
        public short Header => Incoming.SongCodeMessageEvent;

		private readonly IItemController _itemController;

		public SongCodeEvent(IItemController itemController)
		{
			_itemController = itemController;
		}

		public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            string songName = message.ReadString();
            if (!_itemController.TryGetSongDataByName(songName, out ISongData song))
                return;

            await session.SendPacketAsync(new SongCodeComposer(song));
        }
    }
}
