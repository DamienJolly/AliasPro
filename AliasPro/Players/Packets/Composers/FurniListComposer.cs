using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Interaction;
using AliasPro.Items.Types;
using System.Collections.Generic;

namespace AliasPro.Players.Packets.Composers
{
    public class FurniListComposer : IMessageComposer
    {
        private readonly ICollection<IItem> _items;

        public FurniListComposer(ICollection<IItem> items)
        {
            _items = items;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.FurniListMessageComposer);
            message.WriteInt(1);
            message.WriteInt(0);

            message.WriteInt(_items.Count);
            foreach (IItem item in _items)
            {
                message.WriteInt((int)item.Id);
                message.WriteString(item.ItemData.Type.ToUpper());
                message.WriteInt((int)item.Id);
                message.WriteInt((int)item.ItemData.SpriteId);

                switch (item.ItemData.InteractionType)
                {
                    case ItemInteractionType.LANDSCAPE:
                        message.WriteInt(4);
                        break;
                    case ItemInteractionType.FLOOR:
                        message.WriteInt(3);
                        break;
                    case ItemInteractionType.WALLPAPER:
                        message.WriteInt(2);
                        break;
                    default:
                        switch (item.Interaction)
                        {
                            case InteractionGift interactionGift:
                                message.WriteInt((interactionGift.ColorId * 1000) + interactionGift.RibbonId);
                                break;
                            default:
                                message.WriteInt(1);
                                break;
                        }
                        break;
                }

                item.Interaction.ComposeExtraData(message);
				message.WriteBoolean(item.ItemData.AllowRecycle);
                message.WriteBoolean(item.ItemData.AllowTrade);
                message.WriteBoolean(item.ItemData.AllowInventoryStack);
                message.WriteBoolean(item.ItemData.AllowMarketplace);

                message.WriteInt(-1); //time left
                message.WriteBoolean(false); //renatable period started
                message.WriteInt(-1); //room id

                if (item.ItemData.Type == "s")
                {
                    message.WriteString("");
                    switch (item.Interaction)
                    {
                        case InteractionGift interactionGift:
                            message.WriteInt((interactionGift.ColorId * 1000) + interactionGift.RibbonId);
                            break;
                        case InteractionMusicDisc interactionMusicDisc:
                            message.WriteInt(interactionMusicDisc.SongId);
                            break;
                        default:
                            message.WriteInt(0);
                            break;
                    }
                }
            }
            return message;
        }
    }
}
