using System.Net.Sockets;

public class NetworkStateObject
{
    public byte[] Buffer { get; }
    public Socket Socket { get; }

    public NetworkStateObject(Socket socket, int bufferSize = 1024)
    {
        Buffer = new byte[bufferSize];
        Socket = socket;
    }
}