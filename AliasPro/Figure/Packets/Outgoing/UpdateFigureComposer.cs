namespace AliasPro.Figure.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class UpdateFigureComposer : IPacketComposer
    {
        private readonly string _figure;
        private readonly string _gender;

        public UpdateFigureComposer(string figure, string gender)
        {
            _figure = figure;
            _gender = gender;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UpdateFigureMessageComposer);
            message.WriteString(_figure);
            message.WriteString(_gender);
            return message;
        }
    }
}
