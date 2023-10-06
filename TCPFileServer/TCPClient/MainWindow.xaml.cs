using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TCPClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == true)
            {
                tbxFile.Text = ofd.FileName;
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            byte[] bytes = new byte[1024];
            TcpClient client = null;
            NetworkStream stream = null;

            try
            {
                client = new TcpClient("localhost", 8080);
                MessageBox.Show("Connected to the server");

                stream = client.GetStream();
                FileStream fs = new FileStream(tbxFile.Text, FileMode.Open, FileAccess.Read);

                FileInfo fileInfo = new FileInfo();
                fileInfo.FileName = fs.Name;
                fs.Read(fileInfo.File, 0, (int)fs.Length);

                string jsonFileInfo = JsonConvert.SerializedObject(FileInfo);

                int BufferSize = 1024;

                int NoOfPackets = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(fs.Length) / Convert.ToDouble(bytes))); 
                int TotalLength = (int)fs.Length, CurrentPacketLength;
                for (int i = 0; i < NoOfPackets; i++)
                {
                    if (TotalLength > BufferSize)
                    {
                        CurrentPacketLength = BufferSize; TotalLength = TotalLength - CurrentPacketLength;
                    }
                    else
                        CurrentPacketLength = TotalLength; bytes = new byte[CurrentPacketLength];

                    
                    stream.Write(bytes, 0, (int)bytes.Length);
                }
                fs.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally{
                stream.Close();
                client.Close();
            }
        }
    }
}
