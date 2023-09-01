using System.Net;
using System.Net.Sockets;
using System.Text;
public class Program
{
    private static ManualResetEvent manualResetEvent;

    static Program()
    {
        manualResetEvent = new ManualResetEvent(false);
    }
    static void main()
    {
        IPHostEntry hostEntry = Dns.GetHostEntry("localhost"); IPAddress address = hostEntry.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(address, 23456);
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(endPoint);
        socket.Listen(5);
        while (true)
        {
            manualResetEvent.Reset();
            Console.WriteLine("Waiting a connection...");
            socket.BeginAccept(AcceptCallback, socket);
            manualResetEvent.WaitOne();
        }
    }
    private static void AcceptCallback(IAsyncResult ar)
    {
        Socket socket = ar.AsyncState as Socket;
        Socket endSocket = socket.EndAccept(ar);
        State state = new State()
        {
            socket = endSocket,
            buffer = new byte[10],
            data = new StringBuilder()
        };
        endSocket.BeginReceive(state.buffer, 0, state.buffer.Length, SocketFlags.None, ReciveCallback, state);
    }
    private static void ReciveCallback(IAsyncResult ar)
    {
        State state = ar.AsyncState as State;
        Socket socket = state.socket as Socket;

        int readBytes = socket.EndReceive(ar);
        if(readBytes > 0)
        {
            state.data.Append(Encoding.UTF8.GetString(state.buffer, 0, readBytes));

            if (state.data.ToString().Contains("<END>"))
            {
                string answer = string.Format("Thanks receved {0} bytes receicve", state.data.Length);
                byte[] reciveMsg = Encoding.UTF8.GetBytes(answer);
                socket.BeginSend(reciveMsg, 0, reciveMsg.Length, SocketFlags.None, SendCallback, socket);
            }
            else
            {
                socket.BeginReceive(state.buffer, 0, state.buffer.Length, SocketFlags.None, ReciveCallback, state);
            }
        }
    }
    private static void SendCallback(IAsyncResult ar)
    {
        Socket socket = ar.AsyncState as Socket;
        int sendBytes = socket.EndSend(ar);

        Console.WriteLine("Send {0} to client", sendBytes);
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();

        manualResetEvent.Set();
    }
}
public class State
{
    public Socket socket { get; set; }
    public byte[] buffer { get; set; }
    public StringBuilder data { get; set; }
}