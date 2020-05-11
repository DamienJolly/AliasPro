using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Types;
using AliasPro.Utilities;

namespace AliasPro.Game.Catalog.Models
{
    public class CatalogFeaturedPage
    {
        public CatalogFeaturedPage(int slotId, string caption, string image, string type, 
            string pageName, int pageId, string productName, int expireTimestamp)
        {
            SlotId = slotId;
            Caption = caption;
            Image = image;
            Type = type.ToEnum(FeaturedPageType.PAGE_NAME);
            PageName = pageName;
            PageId = pageId;
            ProductName = productName;
            ExpireTimestamp = expireTimestamp;
        }

        public void Compose(ServerMessage message)
        {
            message.WriteInt(SlotId);
            message.WriteString(Caption);
            message.WriteString(Image);
            message.WriteInt((int)Type);

            switch (Type)
            {
                default:
                case FeaturedPageType.PAGE_NAME:
                    message.WriteString(PageName);
                    break;

                case FeaturedPageType.PAGE_ID:
                    message.WriteInt(PageId);
                    break;

                case FeaturedPageType.PRODUCT_NAME:
                    message.WriteString(ProductName);
                    break;
            }

            message.WriteInt((int)UnixTimestamp.Now - ExpireTimestamp);
        }

        public int SlotId { get; }
        public string Caption { get; }
        public string Image { get; }
        public FeaturedPageType Type { get; }
        public string PageName { get; }
        public int PageId { get; }
        public string ProductName { get; }
        public int ExpireTimestamp { get; }
    }
}
