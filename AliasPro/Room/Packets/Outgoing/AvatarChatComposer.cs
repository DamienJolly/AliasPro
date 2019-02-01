namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events.Headers;
    using Network.Protocol;

    public class AvatarChatComposer : ServerPacket
    {
        public AvatarChatComposer(int id, string message, int expression, int colour)
            : base(Outgoing.AvatarChatMessageComposer)
        {
            WriteInt(id);
            WriteString(message);
            WriteInt(expression);
            WriteInt(colour);
            WriteInt(0);
            WriteInt(-1);
        }
    }
}
