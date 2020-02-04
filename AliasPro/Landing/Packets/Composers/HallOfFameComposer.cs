using AliasPro.API.Landing.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Landing.Packets.Composers
{
    public class HallOfFameComposer : IMessageComposer
    {
        private readonly IList<IHallOfFamer> _hallOfFamers;
        private readonly string _key;

        public HallOfFameComposer(IList<IHallOfFamer> hallOfFamers, string key)
        {
            _hallOfFamers = hallOfFamers;
            _key = key;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.HallOfFameMessageComposer);
            message.WriteString(_key);
            message.WriteInt(_hallOfFamers.Count);

            int count = 1;
            foreach (IHallOfFamer hallOfFamer in _hallOfFamers)
            {
                message.WriteInt((int)hallOfFamer.Id);
                message.WriteString(hallOfFamer.Username);
                message.WriteString(hallOfFamer.Figure);
                message.WriteInt(count);
                message.WriteInt(hallOfFamer.Amount);
                count++;
            }
            return message;
        }
    }
}
