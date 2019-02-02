using System.Collections.Generic;

namespace AliasPro.Landing.Packets.Outgoing
{
    using Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class HallOfFameComposer : IPacketComposer
    {
        private readonly IList<IHallOfFamer> _hallOfFamers;
        private readonly string _key;

        public HallOfFameComposer(IList<IHallOfFamer> hallOfFamers, string key)
        {
            _hallOfFamers = hallOfFamers;
            _key = key;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.HallOfFameMessageComposer);
            message.WriteString(_key);
            message.WriteInt(_hallOfFamers.Count);

            int count = 1;
            foreach (IHallOfFamer hallOfFamer in _hallOfFamers)
            {
                message.WriteInt(hallOfFamer.Id);
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
