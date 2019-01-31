namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events.Headers;
    using Network.Protocol;

    public class FurnitureAliasesComposer : ServerPacket
    {
        public FurnitureAliasesComposer()
            : base(Outgoing.FurnitureAliasesMessageComposer)
        {
            WriteInt(0);
        }
    }
}
