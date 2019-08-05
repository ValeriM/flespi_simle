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
            //test2();
            //test3();
            //Console.WriteLine(ReadFile("json3.txt"));
            test5();
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
        //
        static void test3()
        {
            string json = "{\"result\":[{\"absolute.acceleration\":0,\"ain.1\":0,\"ain.2\":0,\"alarm.event\":false,"+
            "\"alarm.mode.status\":false,\"battery.voltage\":4.096,\"brake.acceleration\":0,\"bump.acceleration\":0.35000000000000003,"+
            "\"channel.id\":11489,\"device.id\":361201,\"device.name\":\"УАЗ а025мо\",\"device.temperature\":42,\"device.type.id\":57,"+
            "\"engine.ignition.status\":true,\"external.powersource.voltage\":12.275,\"external.powersource.voltage.range.outside.status\":false,"+
            "\"geofence.status\":false,\"gnss.antenna.status\":true,\"gnss.type\":\"glonass\",\"gsm.signal.level\":100,\"gsm.sim.status\":false,"+
            "\"ibutton.connected.status\":false,\"ident\":\"865905020671073\",\"incline.event\":false,\"internal.battery.voltage.limit.lower.status\":false,"+
            "\"internal.bus.supply.voltage.range.outside.status\":false,\"movement.status\":true,\"peer\":\"85.140.0.112:12032\",\"position.altitude\":27,"+
            "\"position.direction\":300.2,\"position.hdop\":0.5,\"position.latitude\":51.44616,\"position.longitude\":46.107544,\"position.satellites\":15,"+
            "\"position.speed\":0,\"position.valid\":true,\"protocol.id\":16,\"record.seqnum\":28932,\"rs232.sensor.value.0\":0,\"rs485.fuel.sensor.level.0\":0,"+
            "\"rs485.fuel.sensor.level.1\":0,\"rs485.fuel.sensor.level.2\":0,\"server.timestamp\":1563781462.895147,\"shock.event\":false,\"timestamp\":1562511864,"+
            "\"turn.acceleration\":0,\"x.acceleration\":-0.913978494623656,\"y.acceleration\":-0.1774193548387097,\"z.acceleration\":0.3279569892473118}]}";
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
                foreach(var field in fields)
                    Print(fileName, a[field].ToString());
            }
        }
        static void test4()
        {
            string json = "{\"result\":[{\"cid\":152919,\"configuration\":{\"ident\":\"865905020671073\"},\"device_type_id\":57,\"id\":361201,\"ident\":\"865905020671073\",\"messages_ttl\":31536000,\"name\":\"УАЗ а025мо\",\"phone\":\"\"},{\"cid\":152919,\"configuration\":{\"ident\":\"865905021233899\"},\"device_type_id\":57,\"id\":361202,\"ident\":\"865905021233899\",\"messages_ttl\":31536000,\"name\":\"Нива т907хв\",\"phone\":\"\"}]}";
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            string fileName = "json4.txt";

            foreach (var a in dobj["result"])
            {

            }
        }
        static void test5()
        {
            string json = "{\"result\":[{\"id\":361201,\"telemetry\":{\"absolute.acceleration\":{\"ts\":1564740692,\"value\":0},\"ain.1\":{\"ts\":1564740692,\"value\":0},\"ain.2\":{\"ts\":1564740692,\"value\":0},\"alarm.event\":{\"ts\":1564740692,\"value\":false},\"alarm.mode.status\":{\"ts\":1564740692,\"value\":false},\"battery.voltage\":{\"ts\":1564740692,\"value\":3.512},\"brake.acceleration\":{\"ts\":1564740692,\"value\":0},\"bump.acceleration\":{\"ts\":1564740692,\"value\":0.22},\"device.temperature\":{\"ts\":1564740692,\"value\":38},\"engine.ignition.status\":{\"ts\":1564740692,\"value\":true},\"external.powersource.voltage\":{\"ts\":1564740692,\"value\":0},\"external.powersource.voltage.range.outside.status\":{\"ts\":1564740692,\"value\":true},\"geofence.status\":{\"ts\":1564740692,\"value\":false},\"gnss.antenna.status\":{\"ts\":1564740692,\"value\":true},\"gnss.type\":{\"ts\":1564740692,\"value\":\"glonass\"},\"gsm.signal.level\":{\"ts\":1564740692,\"value\":33},\"gsm.sim.status\":{\"ts\":1564740692,\"value\":false},\"hardware.version.enum\":{\"ts\":1564730883.623066,\"value\":17},\"ibutton.connected.status\":{\"ts\":1564740692,\"value\":false},\"ident\":{\"ts\":1564740692,\"value\":\"865905020671073\"},\"incline.event\":{\"ts\":1564740692,\"value\":false},\"internal.battery.voltage.limit.lower.status\":{\"ts\":1564740692,\"value\":true},\"internal.bus.supply.voltage.range.outside.status\":{\"ts\":1564740692,\"value\":true},\"movement.status\":{\"ts\":1564740692,\"value\":true},\"peer\":{\"ts\":1564740692,\"value\":\"85.140.0.165:43418\"},\"position\":{\"ts\":1564740692,\"value\":{\"altitude\":32,\"direction\":296.6,\"hdop\":0.6000000000000001,\"latitude\":51.446176,\"longitude\":46.107536,\"satellites\":15,\"speed\":0,\"valid\":true}},\"position.altitude\":{\"ts\":1564740692,\"value\":32},\"position.direction\":{\"ts\":1564740692,\"value\":296.6},\"position.hdop\":{\"ts\":1564740692,\"value\":0.6000000000000001},\"position.latitude\":{\"ts\":1564740692,\"value\":51.446176},\"position.longitude\":{\"ts\":1564740692,\"value\":46.107536},\"position.satellites\":{\"ts\":1564740692,\"value\":15},\"position.speed\":{\"ts\":1564740692,\"value\":0},\"position.valid\":{\"ts\":1564740692,\"value\":true},\"record.seqnum\":{\"ts\":1564740692,\"value\":3754},\"rs232.sensor.value.0\":{\"ts\":1564740692,\"value\":0},\"rs485.fuel.sensor.level.0\":{\"ts\":1564740692,\"value\":0},\"rs485.fuel.sensor.level.1\":{\"ts\":1564740692,\"value\":0},\"rs485.fuel.sensor.level.2\":{\"ts\":1564740692,\"value\":0},\"server.timestamp\":{\"ts\":1564740692,\"value\":1564740694.42362},\"shock.event\":{\"ts\":1564740692,\"value\":false},\"software.version.enum\":{\"ts\":1564730883.623066,\"value\":231},\"timestamp\":{\"ts\":1564740692,\"value\":1564740692},\"turn.acceleration\":{\"ts\":1564740692,\"value\":0},\"x.acceleration\":{\"ts\":1564740692,\"value\":-0.9301075268817204},\"y.acceleration\":{\"ts\":1564740692,\"value\":-0.1989247311827957},\"z.acceleration\":{\"ts\":1564740692,\"value\":0.3763440860215054}}}]}";
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            string fileName = "json5.txt";

            foreach (var a in dobj["result"])
            {
                Print(fileName, a["id"]);
                Print(fileName, a["telemetry"]["absolute.acceleration"]["ts"]);
                Print(fileName, a["telemetry"]["absolute.acceleration"]["value"]);
                Print(fileName, a["telemetry"]["ain.1"]["ts"]);
                Print(fileName, a["telemetry"]["ain.1"]["value"]);
                Print(fileName, a["telemetry"]["ain.2"]["ts"]);
                Print(fileName, a["telemetry"]["ain.2"]["value"]);
                Print(fileName, a["telemetry"]["alarm.event"]["ts"]);
                Print(fileName, a["telemetry"]["alarm.event"]["value"]);
                Print(fileName, a["telemetry"]["alarm.mode.status"]["ts"]);
                Print(fileName, a["telemetry"]["alarm.mode.status"]["value"]);
                Print(fileName, a["telemetry"]["battery.voltage"]["ts"]);
                Print(fileName, a["telemetry"]["battery.voltage"]["value"]);
                Print(fileName, a["telemetry"]["brake.acceleration"]["ts"]);
                Print(fileName, a["telemetry"]["brake.acceleration"]["value"]);

                Print(fileName, a["telemetry"]["bump.acceleration"]["ts"]);
                Print(fileName, a["telemetry"]["bump.acceleration"]["value"]);

                Print(fileName, a["telemetry"]["device.temperature"]["ts"]);
                Print(fileName, a["telemetry"]["device.temperature"]["value"]);

                Print(fileName, a["telemetry"]["engine.ignition.status"]["ts"]);
                Print(fileName, a["telemetry"]["engine.ignition.status"]["value"]);

                Print(fileName, a["telemetry"]["external.powersource.voltage"]["ts"]);
                Print(fileName, a["telemetry"]["external.powersource.voltage"]["value"]);

                Print(fileName, a["telemetry"]["external.powersource.voltage.range.outside.status"]["ts"]);
                Print(fileName, a["telemetry"]["external.powersource.voltage.range.outside.status"]["value"]);
                Print(fileName, a["telemetry"]["geofence.status"]["ts"]);
                Print(fileName, a["telemetry"]["geofence.status"]["value"]);

                Print(fileName, a["telemetry"]["gnss.antenna.status"]["ts"]);
                Print(fileName, a["telemetry"]["gnss.antenna.status"]["value"]);
                Print(fileName, a["telemetry"]["gnss.type"]["ts"]);
                Print(fileName, a["telemetry"]["gnss.type"]["value"]);
                Print(fileName, a["telemetry"]["gsm.signal.level"]["ts"]);
                Print(fileName, a["telemetry"]["gsm.signal.level"]["value"]);
                Print(fileName, a["telemetry"]["gsm.sim.status"]["ts"]);
                Print(fileName, a["telemetry"]["gsm.sim.status"]["value"]);
                Print(fileName, a["telemetry"]["hardware.version.enum"]["ts"]);
                Print(fileName, a["telemetry"]["hardware.version.enum"]["value"]);
                Print(fileName, a["telemetry"]["ibutton.connected.status"]["ts"]);
                Print(fileName, a["telemetry"]["ibutton.connected.status"]["value"]);

                Print(fileName, a["telemetry"]["ident"]["ts"]);
                Print(fileName, a["telemetry"]["ident"]["value"]);
                Print(fileName, a["telemetry"]["incline.event"]["ts"]);
                Print(fileName, a["telemetry"]["incline.event"]["value"]);
                Print(fileName, a["telemetry"]["internal.battery.voltage.limit.lower.status"]["ts"]);
                Print(fileName, a["telemetry"]["internal.battery.voltage.limit.lower.status"]["value"]);
                Print(fileName, a["telemetry"]["internal.bus.supply.voltage.range.outside.status"]["ts"]);
                Print(fileName, a["telemetry"]["internal.bus.supply.voltage.range.outside.status"]["value"]);
                Print(fileName, a["telemetry"]["movement.status"]["ts"]);
                Print(fileName, a["telemetry"]["movement.status"]["value"]);

                Print(fileName, a["telemetry"]["peer"]["ts"]);
                Print(fileName, a["telemetry"]["peer"]["value"]);
                Print(fileName, a["telemetry"]["position"]["ts"]);
                Print(fileName, a["telemetry"]["position"]["value"]["altitude"]);
                Print(fileName, a["telemetry"]["position"]["value"]["direction"]);
                Print(fileName, a["telemetry"]["position"]["value"]["hdop"]);
                Print(fileName, a["position"]["value"]["latitude"]);
                Print(fileName, a["position"]["value"]["longitude"]);
                Print(fileName, a["position"]["value"]["satellites"]);
                Print(fileName, a["position"]["value"]["speed"]);
                Print(fileName, a["position"]["value"]["valid"]);
                Print(fileName, a["position.altitude"]["ts"]);
                Print(fileName, a["position.altitude"]["value"]);
                Print(fileName, a["position.direction"]["ts"]);
                Print(fileName, a["position.direction"]["value"]);
                Print(fileName, a["position.hdop"]["ts"]);
                Print(fileName, a["position.hdop"]["value"]);
                Print(fileName, a["position.latitude"]["ts"]);
                Print(fileName, a["position.latitude"]["value"]);
                Print(fileName, a["position.longitude"]["ts"]);
                Print(fileName, a["position.longitude"]["value"]);
                Print(fileName, a["position.satellites"]["ts"]);
                Print(fileName, a["position.satellites"]["value"]);
                Print(fileName, a["position.speed"]["ts"]);
                Print(fileName, a["position.speed"]["value"]);
                Print(fileName, a["position.valid"]["ts"]);
                Print(fileName, a["position.valid"]["value"]);
                Print(fileName, a["record.seqnum"]["ts"]);
                Print(fileName, a["record.seqnum"]["value"]);
                Print(fileName, a["rs232.sensor.value.0"]["ts"]);
                Print(fileName, a["rs232.sensor.value.0"]["value"]);
                Print(fileName, a["rs485.fuel.sensor.level.0"]["ts"]);
                Print(fileName, a["rs485.fuel.sensor.level.0"]["value"]);
                Print(fileName, a["rs485.fuel.sensor.level.1"]["ts"]);
                Print(fileName, a["rs485.fuel.sensor.level.1"]["value"]);
                Print(fileName, a["rs485.fuel.sensor.level.2"]["ts"]);
                Print(fileName, a["rs485.fuel.sensor.level.2"]["value"]);
                Print(fileName, a["server.timestamp"]["ts"]);
                Print(fileName, a["server.timestamp"]["value"]);
                Print(fileName, a["shock.event"]["ts"]);
                Print(fileName, a["shock.event"]["value"]);
                Print(fileName, a["software.version.enum"]["ts"]);
                Print(fileName, a["software.version.enum"]["value"]);
                Print(fileName, a["timestamp"]["ts"]);
                Print(fileName, a["timestamp"]["value"]);
                Print(fileName, a["turn.acceleration"]["ts"]);
                Print(fileName, a["turn.acceleration"]["value"]);
                Print(fileName, a["x.acceleration"]["ts"]);
                Print(fileName, a["x.acceleration"]["value"]);
                Print(fileName, a["y.acceleration"]["ts"]);
                Print(fileName, a["y.acceleration"]["value"]);
                Print(fileName, a["z.acceleration"]["ts"]);
                Print(fileName, a["z.acceleration"]["value"]);
            }
        }
        /**/
        /*
         * "id\",
         * "telemetry":{\"absolute.acceleration\":{\"ts\":1564740692,\"value\":0},\"ain.1\":{\"ts\":1564740692,\"value\":0},\"ain.2\":{\"ts\":1564740692,\"value\":0},\"alarm.event\":{\"ts\":1564740692,\"value\":false},\"alarm.mode.status\":{\"ts\":1564740692,\"value\":false},\"battery.voltage\":{\"ts\":1564740692,\"value\":3.512},\"brake.acceleration\":{\"ts\":1564740692,\"value\":0},\"bump.acceleration\":{\"ts\":1564740692,\"value\":0.22},\"device.temperature\":{\"ts\":1564740692,\"value\":38},\"engine.ignition.status\":{\"ts\":1564740692,\"value\":true},\"external.powersource.voltage\":{\"ts\":1564740692,\"value\":0},\"external.powersource.voltage.range.outside.status\":{\"ts\":1564740692,\"value\":true},\"geofence.status\":{\"ts\":1564740692,\"value\":false},\"gnss.antenna.status\":{\"ts\":1564740692,\"value\":true},\"gnss.type\":{\"ts\":1564740692,\"value\":\"glonass\"},\"gsm.signal.level\":{\"ts\":1564740692,\"value\":33},\"gsm.sim.status\":{\"ts\":1564740692,\"value\":false},\"hardware.version.enum\":{\"ts\":1564730883.623066,\"value\":17},\"ibutton.connected.status\":{\"ts\":1564740692,\"value\":false},\"ident\":{\"ts\":1564740692,\"value\":\"865905020671073\"},\"incline.event\":{\"ts\":1564740692,\"value\":false},\"internal.battery.voltage.limit.lower.status\":{\"ts\":1564740692,\"value\":true},\"internal.bus.supply.voltage.range.outside.status\":{\"ts\":1564740692,\"value\":true},\"movement.status\":{\"ts\":1564740692,\"value\":true},\"peer\":{\"ts\":1564740692,\"value\":\"85.140.0.165:43418\"},\"position\":{\"ts\":1564740692,\"value\":{\"altitude\":32,\"direction\":296.6,\"hdop\":0.6000000000000001,\"latitude\":51.446176,\"longitude\":46.107536,\"satellites\":15,\"speed\":0,\"valid\":true}},\"position.altitude\":{\"ts\":1564740692,\"value\":32},\"position.direction\":{\"ts\":1564740692,\"value\":296.6},\"position.hdop\":{\"ts\":1564740692,\"value\":0.6000000000000001},\"position.latitude\":{\"ts\":1564740692,\"value\":51.446176},\"position.longitude\":{\"ts\":1564740692,\"value\":46.107536},\"position.satellites\":{\"ts\":1564740692,\"value\":15},\"position.speed\":{\"ts\":1564740692,\"value\":0},\"position.valid\":{\"ts\":1564740692,\"value\":true},\"record.seqnum\":{\"ts\":1564740692,\"value\":3754},\"rs232.sensor.value.0\":{\"ts\":1564740692,\"value\":0},\"rs485.fuel.sensor.level.0\":{\"ts\":1564740692,\"value\":0},\"rs485.fuel.sensor.level.1\":{\"ts\":1564740692,\"value\":0},\"rs485.fuel.sensor.level.2\":{\"ts\":1564740692,\"value\":0},\"server.timestamp\":{\"ts\":1564740692,\"value\":1564740694.42362},\"shock.event\":{\"ts\":1564740692,\"value\":false},\"software.version.enum\":{\"ts\":1564730883.623066,\"value\":231},\"timestamp\":{\"ts\":1564740692,\"value\":1564740692},\"turn.acceleration\":{\"ts\":1564740692,\"value\":0},\"x.acceleration\":{\"ts\":1564740692,\"value\":-0.9301075268817204},\"y.acceleration\":{\"ts\":1564740692,\"value\":-0.1989247311827957},\"z.acceleration\":{\"ts\":1564740692,\"value\":0.3763440860215054}}}]}*/
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
        static string ReadFile(string fileName)
        {
            string ret;
            StreamReader reader = new StreamReader(fileName);
            ret = reader.ReadToEnd();
            reader.Close();
            return ret;
        }
    }
}
