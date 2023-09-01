using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPHostEntry hostEntry = Dns.GetHostEntry("localhost");
            IPAddress address = hostEntry.AddressList[1];

            IPEndPoint endPoint = new IPEndPoint(address, 23456);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endPoint);
            Console.WriteLine("Connecting...");
            Console.WriteLine("Input message: ");
            string message = Console.ReadLine();

            int sentBytes = socket.Send(Encoding.UTF8.GetBytes(message+"<END>"));

            // Getting answer
            byte[] data = new byte[1024];
            int reciveBytes = socket.Receive(data);

            Console.WriteLine("Answer: {0}", Encoding.UTF8.GetString(data, 0, reciveBytes));

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}