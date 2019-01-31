using System.Collections.Generic;

namespace AliasPro.Landing.Packets.Outgoing
{
    using Models;
    using Network.Events.Headers;
    using Network.Protocol;

    public class HallOfFameComposer : ServerPacket
    {
        public HallOfFameComposer(IList<IHallOfFamer> hallOfFamers, string key)
            : base(Outgoing.HallOfFameMessageComposer)
        {
            WriteString(key);
            WriteInt(hallOfFamers.Count);

            int count = 1;
            foreach(IHallOfFamer hallOfFamer in hallOfFamers)
            {
                WriteInt(hallOfFamer.Id);
                WriteString(hallOfFamer.Username);
                WriteString(hallOfFamer.Figure);
                WriteInt(count);
                WriteInt(hallOfFamer.Amount);
                count++;
            }
        }
    }
}
