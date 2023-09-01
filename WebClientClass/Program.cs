using System.Net;
using System.Text;

namespace WebClientClass
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Example04();
        }

        static void Example01()
        {
            WebClient client = new WebClient();
            Stream data = client.OpenRead("http://google.com");

            StreamReader reader = new StreamReader(data);
            string str = reader.ReadToEnd();

            Console.WriteLine(str);
            data.Close();
            reader.Close();
        }

        static void Example02()
        {
            string url = "";
            string postData = "Hello";

            byte[] postArray = Encoding.UTF8.GetBytes(postData);

            WebClient client = new WebClient();
            using (Stream stream = client.OpenWrite(url))
            {
                stream.Write(postArray, 0, postArray.Length);
            }
        }

        static void Example03()
        {
            string url = "";
            string postData = "Hello";

            byte[] postArray = Encoding.UTF8.GetBytes(postData);

            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.UploadData(url, postArray);
        }

        static void Example04()
        {
            string url = "https://github.com/Asirush/itstep/archive/refs/heads/ntbook.zip";
            string FileName = "ntbook.zip";

            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.DownloadFile(url, FileName);
        }

        static void Example05()
        {
            string url = "";
            WebClient client = new WebClient();

            byte[] myData = client.DownloadData("https://www.google.com/imgres?imgurl=https%3A%2F%2Fupload.wikimedia.org%2Fwikipedia%2Fcommons%2F1%2F15%2FCat_August_2010-4.jpg&tbnid=Kl3KKhCsQyey_M&vet=12ahUKEwjAncCRhfiAAxU9HhAIHd4BCwsQMygBegQIARB2..i&imgrefurl=https%3A%2F%2Fen.wikipedia.org%2Fwiki%2FCat&docid=HkevFQZ5DYu7oM&w=3640&h=2226&q=cats&ved=2ahUKEwjAncCRhfiAAxU9HhAIHd4BCwsQMygBegQIARB2");

            string download = Encoding.UTF8.GetString(myData);
            Console.WriteLine(download);
        }

        static void Exmpl06()
        {
            WebRequest request = WebRequest.Create("URL");
            request.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                request.Timeout = 1000;
                request.Method = "POST";
                request.Headers.Add("");
                WebResponse response = request.GetResponse();
            }
            catch (Exception)
            {

                throw;
            }
        }

        static void Example07()
        {
            WebRequest request = WebRequest.Create("URL");
            request.Credentials = CredentialCache.DefaultCredentials;

            string data = "Test";
            byte[] byArray = Encoding.UTF8.GetBytes(data);
            request.ContentType = "application/x-www-form";
            request.ContentLength = byArray.Length;

            try
            {
                WebResponse response = request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        Console.WriteLine(reader.ReadToEnd());
                    }
                }
                response.Close();
            }
            catch (WebException)
            {

                throw;
            }
            catch (Exception)
            {

                throw;
            }
        }
}