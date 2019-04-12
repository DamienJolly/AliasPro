using AliasPro.API.Landing.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Landing.Packets.Composers
{
    public class NewsListComposer : IPacketComposer
    {
        private readonly IList<IArticle> _articles;

        public NewsListComposer(IList<IArticle> articles)
        {
            _articles = articles;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.NewsListMessageComposer);
            message.WriteInt(_articles.Count);
            foreach (IArticle article in _articles)
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
