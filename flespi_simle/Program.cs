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
        static string token = "Authorization: FlespiToken Vo6wSNjDEM19qzUdq9qbwZugZPmPl3N4hHq0lAtPalMqIwuYuKZQxiUnX7060B17";
        //token = "Authorization: FlespiToken UIy8bexWRWLVX3H3yJFCkycRTNI3xRognMeoOBbvlKf8EK20kvrsRraz4GsqnGwB";
        static string log = "flespi_simle.txt";
        static void Main(string[] args)
        {
            Print(log, "Start " + DateTime.Now.ToString());

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
                "8. List device messages snapshots",
                "9. Fetch device messages snapshot"
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
                int menuitem = Convert.ToInt32(ch.KeyChar);
                if (menuitem != 27)
                    Print(log, "Selected " + menu[(menuitem-48)]);
                switch (ch.KeyChar)
                {
                    case '1': Client("https://flespi.io/gw/devices/361201,361202"); break;
                    case '2': Client("https://flespi.io/gw/devices/361201/logs"); break;
                    case '3': Client("https://flespi.io/gw/devices/361201/messages"); break;
                    case '4': Client("https://flespi.io/gw/devices/all/settings/all"); break;
                    case '5': Client("https://flespi.io/gw/devices/361201/telemetry"); break;
                    case '6': Client("https://flespi.io/gw/channels/all"); break;
                    case '7': ClientPost("https://flespi.io/gw/devices"); break;
                    case '8': Client("https://flespi.io/gw/devices/all/snapshots"); break;
                    case '9': Client("https://flespi.io/gw/devices/361201/snapshots/1563908110"); break;
                    default: Console.WriteLine("Что это было?"); break;
                }
            } while (ch.Key != ConsoleKey.Escape);
            test1();
        }
        /**/
        static void test1()
        { 
            string json = "{\"result\":[{\"cid\":152919,\"configuration\":{\"ident\":\"865905020671073\"},\"device_type_id\":57,\"id\":361201,\"ident\":\"865905020671073\",\"messages_ttl\":31536000,\"name\":\"УАЗ а025мо\",\"phone\":\"\"},{\"cid\":152919,\"configuration\":{\"ident\":\"865905021233899\"},\"device_type_id\":57,\"id\":361202,\"ident\":\"865905021233899\",\"messages_ttl\":31536000,\"name\":\"Нива т907хв\",\"phone\":\"\"}]}";
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);

            foreach(var a in dobj["result"])
            {
                Print("json.txt", a["cid"].ToString());
                Print("json.txt", a["configuration"]["ident"]);
                Print("json.txt", a["device_type_id"].ToString());
                Print("json.txt", a["id"].ToString());
                Print("json.txt", a["ident"].ToString());
                Print("json.txt", a["messages_ttl"].ToString());
                Print("json.txt", a["name"].ToString());
                Print("json.txt", a["phone"].ToString());
            }
        }        
        /**/
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
                webClient.Headers.Add(token);
                Stream stream = webClient.OpenRead(URI);
                StreamReader reader = new StreamReader(stream);
                String request = reader.ReadToEnd();
                Print(log, new string('-', 80));
                Print(log, request);
                /*StreamWriter streamWriter = new StreamWriter("snapshot.gz");
                streamWriter.Write(request);
                streamWriter.Close();*/
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
                            Print(log, "Что-то пошло не так!");
                            break;

                        default:
                            Print(log, "Что-то однозначно пошло не так! " + ex.Message);
                            //throw ex;
                            break;
                    }
                }
            }
        }
        static void ClientPost(string url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Headers.Add(token);
            httpWebRequest.Accept = "application/json";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.KeepAlive = true;
            httpWebRequest.Method = "POST";
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";

            //Console.WriteLine(httpWebRequest.Headers.ToString());

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "["+new JavaScriptSerializer().Serialize(new
                {
                    messages_ttl = 31536000,
                    phone = "{}"
                })+"]";
                streamWriter.Write(json);
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    Print(log, streamReader.ReadToEnd());
                }
            }
            catch(WebException ex)
            {
                Print(log, ex.Message);
                //Console.WriteLine(ex.Response.Headers.ToString());
                using (var streamReader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    Print(log, streamReader.ReadToEnd());
                }
            }
        }
        static void Print(string filename, string line, bool console = true)
        {
            if (console)
                Console.WriteLine(line);
            StreamWriter writer;
            if (File.Exists(filename))
                writer = new StreamWriter(filename, true);
            else
                writer = new StreamWriter(filename);
            writer.WriteLine(line);
            writer.Close();
        }
    }
}
