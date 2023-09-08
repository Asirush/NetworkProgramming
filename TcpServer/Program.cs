using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TcpServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Run a portscanner
            /*List<PortInfo> ports = GetOpenPort();
            foreach(PortInfo item in ports)
            {
                Console.WriteLine("Port: {0} - {1}", item.PortNumber, item.State);
            }
            return;*/


            try
            {
                TcpListener server = null;
                IPAddress localAdr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAdr, 9595);
                server.Start();

                byte[] bytes = new byte[1024];
                string data = "";
                NetworkStream stream = null;
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    int i = 0;
                    while ((i=stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = Encoding.UTF8.GetString(bytes, 0, i);
                        Console.WriteLine("-> {0}", data);

                        data = "Message Received";
                        byte[] ReciveData = Encoding.UTF8.GetBytes(data);
                        stream.Write(ReciveData, 0, ReciveData.Length);
                    }
                    client.Close();
                }
            }
            catch { }
        }

        public void pinging()
        {
            try
            {
                Ping myping = new Ping();
                PingReply reply = myping.Send("192.168.111.187", 9595);
                Console.WriteLine("Status: {0}\nTime: {0}\nAddress: {0}", reply.Status, reply.RoundtripTime.ToString(), reply.Address);
            }
            catch { }
        }

        public static List<PortInfo> GetOpenPort()
        {
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndPoint = properties.GetActiveTcpListeners();
            TcpConnectionInformation[] tcpConnections = properties.GetActiveTcpConnections();

            var data = tcpConnections.Select(s => new PortInfo(
                    s.LocalEndPoint.Port,
                    String.Format("{0}:{1}", s.LocalEndPoint.Address, s.LocalEndPoint.Port),
                    String.Format("{0}:{1}", s.RemoteEndPoint.Address, s.RemoteEndPoint.Port),
                    s.State.ToString()
                )).ToList();
            return data;
        }
        public class PortInfo
        {
            public PortInfo(int portNUmber, string local, string remote, string state)
            {
                PortNumber = portNUmber;
                Local = local;
                Remote = remote;
                State = state;
            }
            public int PortNumber {  get; set; }
            public string Local {  get; set; }
            public string Remote { get; set; }
            public string State { get; set; }
        }
    }
}