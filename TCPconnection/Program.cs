using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPconnection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IPAddress address = IPAddress.Parse("127.0.0.1");
                IPEndPoint endPoint = new IPEndPoint(address, 0);

                // TcpClient client = new TcpClient("127.0.0.1", 9595);
                //TcpClient tcpClient = new TcpClient(endPoint);
                // TCP Using DNS
                TcpClient client = new TcpClient(AddressFamily.InterNetwork);
                IPAddress[] IPAddresses = Dns.GetHostAddresses("127.0.0.1");
                client.Connect(IPAddresses, 9595);

                // Will connection stay after socket being closed and how long
                client.LingerState = new LingerOption(false, 0);

                // Setting for a collecting full buffer packa
                client.NoDelay = true;

                // Size of an income packet
                client.ReceiveBufferSize = 1024;

                // Size of outcome packet
                client.SendBufferSize = 1024;

                // ms await for a income packets
                client.ReceiveTimeout = 10000;

                // ms await for a send packets
                client.SendTimeout = 10000;

                Console.WriteLine("-> ");
                string message = Console.ReadLine();

                // Convert to bytes
                byte[] data = Encoding.UTF8.GetBytes(message);

                // Get stream
                NetworkStream stream = client.GetStream();

                // Send message to our client
                stream.Write(data, 0, data.Length);

                // получаем ответ от сервера
                data = new byte[256];
                string responceData = "";

                // Считаем первый пакет ответа сервера
                int bytes = stream.Read(data, 0, data.Length);
                responceData = Encoding.UTF8.GetString(data, 0, bytes);
                Console.WriteLine("-> {0}", responceData);

                // Закрываем все
                stream.Close();
                client.Close();
            }
            // SocketException is less prioritized then Exception. Exception class will get all your errors.
            // catch (SocketException se) { }
            catch (Exception ex) { }
        }
    }
}