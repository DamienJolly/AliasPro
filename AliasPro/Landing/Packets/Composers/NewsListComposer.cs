using AliasPro.API.Landing.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Landing.Packets.Composers
{
    public class NewsListComposer : IMessageComposer
    {
        private readonly IList<IArticle> _articles;

        public NewsListComposer(IList<IArticle> articles)
        {
            _articles = articles;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.NewsListMessageComposer);
            message.WriteInt(_articles.Count);
            foreach (IArticle article in _articles)
            {
                message.WriteInt((int)article.Id);
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
