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
        static readonly string token = "Authorization: FlespiToken Vo6wSNjDEM19qzUdq9qbwZugZPmPl3N4hHq0lAtPalMqIwuYuKZQxiUnX7060B17";
        //token = "Authorization: FlespiToken UIy8bexWRWLVX3H3yJFCkycRTNI3xRognMeoOBbvlKf8EK20kvrsRraz4GsqnGwB";
        static readonly string log = "flespi_simle.txt";
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
            //test1();
            test2();
        }
        /**/
        static void test1()
        { 
            string json = "{\"result\":[{\"cid\":152919,\"configuration\":{\"ident\":\"865905020671073\"},\"device_type_id\":57,\"id\":361201,\"ident\":\"865905020671073\",\"messages_ttl\":31536000,\"name\":\"УАЗ а025мо\",\"phone\":\"\"},{\"cid\":152919,\"configuration\":{\"ident\":\"865905021233899\"},\"device_type_id\":57,\"id\":361202,\"ident\":\"865905021233899\",\"messages_ttl\":31536000,\"name\":\"Нива т907хв\",\"phone\":\"\"}]}";
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            string fileName = "json1.txt";

            foreach(var a in dobj["result"])
            {
                Print(fileName, a["cid"].ToString());
                Print(fileName, a["configuration"]["ident"]);
                Print(fileName, a["device_type_id"].ToString());
                Print(fileName, a["id"].ToString());
                Print(fileName, a["ident"].ToString());
                Print(fileName, a["messages_ttl"].ToString());
                Print(fileName, a["name"].ToString());
                Print(fileName, a["phone"].ToString());
            }
        }
        /**/
        static void test2()
        {
            string json = "{\"result\":[{\"event_code\":300,\"event_origin\":\"gw\\/devices\\/361201\",\"event_text\":\"85.140.0.165\\/connected\",\"id\":361201,\"ident\":\"865905020671073\",\"source\":\"85.140.0.165\",\"timestamp\":1564730883.623162,\"transport\":\"tcp\"},{\"close_code\":3,\"duration\":10892,\"event_code\":301,\"event_origin\":\"gw\\/devices\\/361201\",\"event_text\":\"85.140.0.165\\/disconnected\",\"id\":361201,\"ident\":\"865905020671073\",\"msgs\":3022,\"recv\":252588,\"send\":1825,\"source\":\"85.140.0.165\",\"timestamp\":1564741773.556435,\"transport\":\"tcp\"}]}";
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            string fileName = "json2.txt";

            foreach (var a in dobj["result"])
            {
                Print(fileName, a["event_code"].ToString());
                Print(fileName, a["event_origin"].ToString());
                Print(fileName, a["event_text"].ToString());
                Print(fileName, a["id"].ToString());
                Print(fileName, a["ident"].ToString());
                Print(fileName, a["source"].ToString());
                Print(fileName, a["timestamp"].ToString());
                Print(fileName, a["transport"].ToString()); 
            }
        }
        /**/
        //"{"result":[{"absolute.acceleration":0,"ain.1":0,"ain.2":0,"alarm.event":false,"alarm.mode.status":false,"battery.voltage":4.096,"brake.acceleration":0,"bump.acceleration":0.35000000000000003,"channel.id":11489,"device.id":361201,"device.name":"УАЗ а025мо","device.temperature":42,"device.type.id":57,"engine.ignition.status":true,"external.powersource.voltage":12.275,"external.powersource.voltage.range.outside.status":false,"geofence.status":false,"gnss.antenna.status":true,"gnss.type":"glonass","gsm.signal.level":100,"gsm.sim.status":false,"ibutton.connected.status":false,"ident":"865905020671073","incline.event":false,"internal.battery.voltage.limit.lower.status":false,"internal.bus.supply.voltage.range.outside.status":false,"movement.status":true,"peer":"85.140.0.112:12032","position.altitude":27,"position.direction":300.2,"position.hdop":0.5,"position.latitude":51.44616,"position.longitude":46.107544,"position.satellites":15,"position.speed":0,"position.valid":true,"protocol.id":16,"record.seqnum":28932,"rs232.sensor.value.0":0,"rs485.fuel.sensor.level.0":0,"rs485.fuel.sensor.level.1":0,"rs485.fuel.sensor.level.2":0,"server.timestamp":1563781462.895147,"shock.event":false,"timestamp":1562511864,"turn.acceleration":0,"x.acceleration":-0.913978494623656,"y.acceleration":-0.1774193548387097,"z.acceleration":0.3279569892473118}]}";
        static void test3()
        {
            string json = "{\"result\":[{\"cid\":152919,\"configuration\":{\"ident\":\"865905020671073\"},\"device_type_id\":57,\"id\":361201,\"ident\":\"865905020671073\",\"messages_ttl\":31536000,\"name\":\"УАЗ а025мо\",\"phone\":\"\"},{\"cid\":152919,\"configuration\":{\"ident\":\"865905021233899\"},\"device_type_id\":57,\"id\":361202,\"ident\":\"865905021233899\",\"messages_ttl\":31536000,\"name\":\"Нива т907хв\",\"phone\":\"\"}]}";
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            string fileName = "json3.txt";

            string[] fields =
            {
                "absolute.acceleration",
                "ain.1",
                "ain.2",
                "alarm.event",
                "alarm.mode.status",
                "battery.voltage",
                "brake.acceleration",
                "bump.acceleration",
                "channel.id",
                "device.id",
                "device.name",
                "device.temperature",
                "device.type.id",
                "engine.ignition.status",
                "external.powersource.voltage",
                "external.powersource.voltage.range.outside.status",
                "geofence.status",
                "gnss.antenna.status",
                "gnss.type",
                "gsm.signal.level",
                "gsm.sim.status",
                "ibutton.connected.status",
                "ident",
                "incline.event",
                "internal.battery.voltage.limit.lower.status",
                "internal.bus.supply.voltage.range.outside.status",
                "movement.status",
                "peer",
                "position.altitude",
                "position.direction",
                "position.latitude",
                "position.longitude",
                "position.satellites",
                "position.speed",
                "position.valid",
                "protocol.id",
                "record.seqnum",
                "rs232.sensor.value.0",
                "rs485.fuel.sensor.level.0",
                "rs485.fuel.sensor.level.1",
                "rs485.fuel.sensor.level.2",
                "server.timestamp",
                "shock.event",
                "timestamp",
                "turn.acceleration",
                "x.acceleration",
                "y.acceleration",
                "z.acceleration"
            };

            foreach (var a in dobj["result"])
            {

            }
        }
        static void test4()
        {
            string json = "{\"result\":[{\"cid\":152919,\"configuration\":{\"ident\":\"865905020671073\"},\"device_type_id\":57,\"id\":361201,\"ident\":\"865905020671073\",\"messages_ttl\":31536000,\"name\":\"УАЗ а025мо\",\"phone\":\"\"},{\"cid\":152919,\"configuration\":{\"ident\":\"865905021233899\"},\"device_type_id\":57,\"id\":361202,\"ident\":\"865905021233899\",\"messages_ttl\":31536000,\"name\":\"Нива т907хв\",\"phone\":\"\"}]}";
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            string fileName = "json3.txt";

            foreach (var a in dobj["result"])
            {

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
