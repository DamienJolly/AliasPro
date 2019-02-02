using System.Collections.Generic;

namespace AliasPro.Landing.Packets.Outgoing
{
    using Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class NewsListComposer : IPacketComposer
    {
        private readonly IList<IArticles> _articles;

        public NewsListComposer(IList<IArticles> articles)
        {
            _articles = articles;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.NewsListMessageComposer);
            message.WriteInt(_articles.Count);
            foreach (IArticles article in _articles)
            {
                message.WriteInt(article.Id);
                message.WriteString(article.Title);
                message.WriteString(article.Message);
                message.WriteString(article.Caption);
                message.WriteInt(article.Type);
                message.WriteString(article.Link);
                message.WriteString(article.Image);
            }
            return message;
        }
    }
}
