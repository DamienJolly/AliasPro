using System.Collections.Generic;

namespace AliasPro.Landing.Packets.Outgoing
{
    using Models;
    using Network.Events.Headers;
    using Network.Protocol;

    public class NewsListComposer : ServerPacket
    {
        public NewsListComposer(IList<IArticles> articles)
            : base(Outgoing.NewsListMessageComposer)
        {
            WriteInt(articles.Count);
            foreach(IArticles article in articles)
            {
                WriteInt(article.Id);
                WriteString(article.Title);
                WriteString(article.Message);
                WriteString(article.Caption);
                WriteInt(article.Type);
                WriteString(article.Link);
                WriteString(article.Image);
            }
        }
    }
}
