namespace AliasPro.Network.Protocol
{
    public interface IClientPacket
    {
        short Header { get; }
        int ReadInt();
        short ReadShort();
        byte ReadByte();
        byte[] ReadBytes(int length);
        bool ReadBool();
        string ReadString();
    }
}
