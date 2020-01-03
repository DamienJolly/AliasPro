namespace AliasPro.API.Network.Protocol
{
    public interface IClientPacket
    {
        short Header { get; }

        int ReadInt();
        short ReadShort();
        byte ReadByte();
        byte[] ReadBytes(int length);

        int BytesAvailable();
        bool ReadBool();
        string ReadString();
    }
}
