using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TcpListener listner = null;
            try
            {
                listner = new TcpListener(IPAddress.Any, 8080);
                listner.Start();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            byte[] bytes = new byte[1024];
            int recBytes = 0;

            TcpClient client = null;
            NetworkStream stream = null;

            try
            {
                client = listner.AcceptTcpClient();
                stream = client.GetStream();
                Console.WriteLine("Connect to client");

                stream.Read(bytes, 0, bytes.Length);
                string message = Encoding.UTF8.GetString(bytes);

                FileInfo file = JsonConvert.DeserializeObject<FileInfo>(message);

                string path = @"D:\Learning\NetworkProgramming\TCPServer\data\"+file.FileName;
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fs.Write(file.File, 0, file.File.Length);
                }
            }
            catch (Exception ex) { Console.WriteLine(); }
            finally
            {
                stream.Close();
                client.Close();
            }
        }
    }
}