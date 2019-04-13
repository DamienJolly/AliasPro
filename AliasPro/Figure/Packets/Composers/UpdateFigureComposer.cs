using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using AliasPro.Players.Types;

namespace AliasPro.Figure.Packets.Composers
{
    public class UpdateFigureComposer : IPacketComposer
    {
        private readonly string _figure;
        private readonly PlayerGender _gender;

        public UpdateFigureComposer(string figure, PlayerGender gender)
        {
            _figure = figure;
            _gender = gender;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UpdateFigureMessageComposer);
            message.WriteString(_figure);
            message.WriteString(_gender == PlayerGender.MALE ? "m" : "f");
            return message;
        }
    }
}
