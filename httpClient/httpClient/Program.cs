using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace httpClient
{
    internal class Program
    {
        static void get()
        {
            string uri = "https://jsonplaceholder.typicode.com/todos";

            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
            request.Method = "GET";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            StreamReader reader = new StreamReader(response.GetResponseStream());
            Console.WriteLine(reader.ReadToEnd());
            reader.Close();
        }

        static void example()
        {
            string uri = "https://jsonplaceholder.typicode.com/todos";

            var client = new HttpClient();
            var responce = client.GetAsync(uri);
            if (responce.IsCompleted) { }
            else { Console.WriteLine("error"); };

            var responseContent = responce.Result.Content.ReadAsStringAsync().Result;
            Console.WriteLine(responseContent);

            client.Dispose();
        }

        static void example1()
        {
            string uri = "https://httpbin.org/get";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("MyCustomHeader", "Custom");
            var responce = client.GetAsync(uri);

            responce.Wait();

            if (responce.IsCompleted) { }
            else { Console.WriteLine("error"); };

            var responseContent = responce.Result.Content.ReadAsStringAsync().Result;
            Console.WriteLine(responseContent);

            client.Dispose();
        }

        static void Auth()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Bearer jshbiu");

                var result = client.GetAsync("https://httpbin.org/bearer");
                var content = result.Result.Content.ReadAsStringAsync();
                Console.WriteLine(content.Result);
            }
        }

        static void BasicAuth(string userName, string passwd)
        {
            using (var client = new HttpClient())
            {
                var authToken = Encoding.UTF8.GetBytes($"{userName}:{passwd}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                var result = client.GetAsync("https://httpbin.org/basic-auth/" + $"{userName}/{passwd}");
                var content = result.Result.Content.ReadAsStringAsync();
                Console.WriteLine(content.Result);
            }
        }

        static void Timeout()
        {
            using (var client = new HttpClient())
            {
                // variant 1
                /*                client.Timeout = TimeSpan.FromSeconds(10);

                                var result = client.GetAsync("https://jsonplaceholder.typicode.com/todos");*/

                // variant 2
                var cts = new CancellationTokenSource(); // CancelletionToken
                cts.CancelAfter(TimeSpan.FromSeconds(10));

                var result = client.GetAsync("https://jsonplaceholder.typicode.com/todos", cts.Token);

                var content = result.Result.Content.ReadAsStringAsync();
                Console.WriteLine(content.Result);
            }
        }

        static void Post()
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent("{\"args\":\"John Doe\",\"data\":30}\", Encoding.UTF8, \"application/json\"");
                var responce = client.PostAsync("https://httpbin.org/post", content);
                var responceContent = responce.Result.Content.ReadAsStringAsync().Result;
                Console.WriteLine(responceContent);

            }
        }

        static void ftpListDirectory()
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://127.0.0.1");
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            request.Credentials = new NetworkCredential("admin", "admin");

            FtpWebResponse responce = (FtpWebResponse)request.GetResponse();

            Stream responceStream = responce.GetResponseStream();
            StreamReader reader = new StreamReader(responceStream);
            Console.WriteLine(reader.ReadToEnd());

            Console.WriteLine($"Directory List Complete, status " + $"{responce.StatusDescription}");

            reader.Close();
            responce.Close();
        }

        static void ftpDownloadFile()
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://127.0.0.1/1.txt");
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            request.Credentials = new NetworkCredential("admin", "admin");

            FtpWebResponse responce = (FtpWebResponse)request.GetResponse();

            Stream responceStream = responce.GetResponseStream();

            FileStream fs = new FileStream("myFile.txt", FileMode.Create);
            byte[] data = new byte[64];
            int size = 0;

            while((size = responceStream.Read(data, 0, data.Length))>0)
            {
                fs.Write(data, 0, size);
            }
            fs.Close();
            responce.Close();
            Console.WriteLine("Done!");
        }

        static void ftpUploadFile()
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://127.0.0.1");
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential("admin", "admin");

            byte[] fileContent;
            using(StreamReader sr = new StreamReader("MyFile01.txt"))
            {
                fileContent = Encoding.UTF8.GetBytes(sr.ReadToEnd());
            }

            using(Stream sw = request.GetRequestStream())
            {
                sw.Write(fileContent);
            }
            request.GetResponse();

            Console.WriteLine("Upload is done");
        }

        static void ftpDeleteFile()
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://127.0.0.1");
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = new NetworkCredential("admin", "admin");

            request.GetResponse();

            Console.WriteLine("File deletion is done");
        }

        static void Main(string[] args)
        {
            
        }
    }
}