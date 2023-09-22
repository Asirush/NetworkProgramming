using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpServerApp_TcpUdpSockets_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server Run.");
            try
            {
                while (true)
                {
                    UdpClient server = new UdpClient(5000); // 5000 is a port we listen
                    IPEndPoint remoteEndPoint = null;

                    // get data
                    Byte[] bytes = server.Receive(ref remoteEndPoint); // ref is needed to get from whom we get data
                    string message = Encoding.UTF8.GetString(bytes);

                    Console.WriteLine("-> {0}: {1}", remoteEndPoint.Address, message);
                    server.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}