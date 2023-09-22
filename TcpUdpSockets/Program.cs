using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpUdpSockets
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UdpClient udpCLient = new UdpClient(11000);
            UdpClient udpCLientB = new UdpClient(11000);
            try
            {
                udpCLient.Connect("", 11000);
                Byte[] sendBytes = Encoding.UTF8.GetBytes("Who are you?");
                udpCLient.Send(sendBytes, sendBytes.Length);

                udpCLientB.Send(sendBytes, sendBytes.Length, "HOST", 11000);

                IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] recive = udpCLient.Receive(ref remoteEndpoint);
            }
            catch (Exception ex) { }
        }
    }
}