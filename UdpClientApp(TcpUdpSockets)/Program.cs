using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpClientApp_TcpUdpSockets_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Client Run");
                while (true)
                {
                    UdpClient client = new UdpClient();

                    IPAddress address = IPAddress.Parse("127.0.0.1");
                    client.Connect(address, 5000); // set dest host to localhost on 5000 port


                    Console.WriteLine("->");
                    byte[] bytes = Encoding.UTF8.GetBytes(Console.ReadLine());
                    client.Send(bytes, bytes.Length);

                    client.Close();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}