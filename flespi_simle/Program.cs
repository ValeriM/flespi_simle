using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Web.Script;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
//using Newtonsoft.Json;

namespace flespi_simle
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] menu =
            {
                "Esc. Exit",
                "1. Get collection of devices matching filter parameters",
                "2. Get device logs records",
                "3. Get device messages (Can take a long time!)",
                "4. Get settings collection",
                "5. Get device telemetry",
                "6. Get collection of channels matching filter parameters",
                "7. Create new device",
                "8. Эээ... восемь, кажись"
            };
            ConsoleKeyInfo ch;
            do
            {
                foreach (var m in menu)
                {
                    Console.WriteLine(m);

                }
                ch = Console.ReadKey();
                Console.WriteLine();
                switch (ch.KeyChar)
                {
                    case '1': Client("https://flespi.io/gw/devices/361201"); break;
                    case '2': Client("https://flespi.io/gw/devices/361201/logs"); break;
                    case '3': Client("https://flespi.io/gw/devices/361201/messages"); break;
                    case '4': Client("https://flespi.io/gw/devices/all/settings/all"); break;
                    case '5': Client("https://flespi.io/gw/devices/361201/telemetry"); break;
                    case '6': Client("https://flespi.io/gw/channels/all"); break;
                    case '7': ClientPost("https://flespi.io/gw/devices"); break;
                    default: Console.WriteLine("Что это было?"); break;
                }
            } while (ch.Key != ConsoleKey.Escape);
            //test();
        }
        static void test()
        {
            string json = @"{
                'Email': 'james@example.com',
                'Active': true,
                'CreatedDate': '2013-01-20T00:00:00Z',
                'Roles': [
                {'User':'111','Admin':'123'}
                ]}";

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            string result = dobj["Email"].ToString();
            object result1 = dobj["Roles"][0]["User"];
        }
        /*
            Если нужна авторизация
            WebClient webClient = new WebClient();
            webClient.Credentials = new NetworkCredential(username, password);
            Stream stream = webClient.OpenRead(URI);
        */
        static void Client(string URI)
        {
            try
            {
                WebClient webClient = new WebClient();
                //webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                webClient.Headers.Add("Accept: application/json");
                webClient.Headers.Add("Authorization: FlespiToken Vo6wSNjDEM19qzUdq9qbwZugZPmPl3N4hHq0lAtPalMqIwuYuKZQxiUnX7060B17");
                //("Authorization: FlespiToken UIy8bexWRWLVX3H3yJFCkycRTNI3xRognMeoOBbvlKf8EK20kvrsRraz4GsqnGwB");
                Stream stream = webClient.OpenRead(URI);
                StreamReader reader = new StreamReader(stream);
                String request = reader.ReadToEnd();
                Console.WriteLine(request);
                stream.Close();
                reader.Close();
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse)
                {
                    switch (((HttpWebResponse)ex.Response).StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            //response = null;
                            Console.WriteLine("Что-то пошло не так!");
                            break;

                        default:
                            Console.WriteLine("Что-то однозначно пошло не так!");
                            throw ex;
                    }
                }
            }
        }
        static void ClientPost(string url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Accept = "application/json";
            httpWebRequest.KeepAlive = true;
            httpWebRequest.ServicePoint.Expect100Continue = false;
            httpWebRequest.AllowAutoRedirect = true;
            httpWebRequest.ContentType = " application/json; charset=utf-8";
            httpWebRequest.Timeout = 10000000;
            httpWebRequest.Method = "POST";
            httpWebRequest.UserAgent = "Microsoft ADO.NET Data Services";
            httpWebRequest.Headers.Add("Authorization: FlespiToken Vo6wSNjDEM19qzUdq9qbwZugZPmPl3N4hHq0lAtPalMqIwuYuKZQxiUnX7060B17");

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    messages_ttl = "31536000",
                    phone = "{}"
                });
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                Console.WriteLine(streamReader.ReadToEnd());
            }
        }

    }
}
