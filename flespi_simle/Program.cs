using System;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;

namespace flespi_simle
{
    class Program
    {
        static readonly string /*token = "Authorization: FlespiToken Vo6wSNjDEM19qzUdq9qbwZugZPmPl3N4hHq0lAtPalMqIwuYuKZQxiUnX7060B17";*/
        token = "Authorization: FlespiToken UIy8bexWRWLVX3H3yJFCkycRTNI3xRognMeoOBbvlKf8EK20kvrsRraz4GsqnGwB";
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
                "9. Fetch device messages snapshot",
                "a. Change existing devices properties",
                "b. Calculate devices's intervals",
                "c. Push new command to change setting value",
                "d. Delete selected devices",
                "e. Reset setting value"
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
                //int menuitem = Convert.ToInt32(ch.KeyChar);
                //if (menuitem != 27)
                //    Print(log, "Selected " + menu[(menuitem-48)]);
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
                    case 'a': ClientPut("https://flespi.io/gw/devices", "DoesNotExist"); break;
                    case 'b': ClientPost("https://flespi.io/gw/devices/361201/calculate"); break;
                    case 'c': ClientPut("https://flespi.io/gw/devices/361201/settings/DoesNotExist"); break;
                    case 'd': ClientDelete("https://flespi.io/gw/devices", "DoesNotExist"); break;
                    case 'e': ClientDelete("https://flespi.io/gw/devices/361201/settings", "DoesNotExist"); break;
                    default: Console.WriteLine("Что это было?"); break;
                }
            } while (ch.Key != ConsoleKey.Escape);
            //test1();
            //test2();
            //test3();
            //Console.WriteLine(ReadFile("json3.txt"));
            //test5();
            //test4();
            //test6();
            //test8();

        }
        /**/
        static void test6()
        {
            //string json = "{\"result\":[{\"cid\":152919,\"configuration\":{\"ident\":\"865905020671073\"},\"device_type_id\":57,\"id\":361201,\"ident\":\"865905020671073\",\"messages_ttl\":31536000,\"name\":\"УАЗ а025мо\",\"phone\":\"\"},{\"cid\":152919,\"configuration\":{\"ident\":\"865905021233899\"},\"device_type_id\":57,\"id\":361202,\"ident\":\"865905021233899\",\"messages_ttl\":31536000,\"name\":\"Нива т907хв\",\"phone\":\"\"}]}";
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            
            string fileName = "json6.txt";
            //{"result":[{"cid":166848,"commands_ttl":86400,"configuration":null,"enabled":true,"id":11844,"messages_ttl":86400,"name":"QS","protocol_id":1,"uri":"193.193.165.37:30242"}]} 
            string json = ReadFile("json6i.txt");
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            foreach (var a in dobj["result"])
            {
                Print(fileName, a["cid"].ToString());
                Print(fileName, a["commands_ttl"].ToString());
                object o = a["configuration"];
                Print(fileName, a["configuration"].ToString());
                Print(fileName, a["enabled"].ToString());
                Print(fileName, a["id"].ToString());
                Print(fileName, a["messages_ttl"].ToString());
                Print(fileName, a["name"].ToString());
                Print(fileName, a["protocol_id"].ToString());
                Print(fileName, a["uri"].ToString());
            }
        }
        static void test8()
        {
            //string json = "{\"result\":[{\"cid\":152919,\"configuration\":{\"ident\":\"865905020671073\"},\"device_type_id\":57,\"id\":361201,\"ident\":\"865905020671073\",\"messages_ttl\":31536000,\"name\":\"УАЗ а025мо\",\"phone\":\"\"},{\"cid\":152919,\"configuration\":{\"ident\":\"865905021233899\"},\"device_type_id\":57,\"id\":361202,\"ident\":\"865905021233899\",\"messages_ttl\":31536000,\"name\":\"Нива т907хв\",\"phone\":\"\"}]}";
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            string fileName = "json8.txt";
            //{"result":[{"cid":166848,"commands_ttl":86400,"configuration":null,"enabled":true,"id":11844,"messages_ttl":86400,"name":"QS","protocol_id":1,"uri":"193.193.165.37:30242"}]} 
            string json = ReadFile("json8i.txt"); dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            foreach (var a in dobj["result"])
            {
                Print(fileName, a["id"].ToString());
                //object o = a["snapshots"];
                foreach(var v in a["snapshots"])
                Print(fileName, v.ToString());
            }
        }
        static void test1()
        { 
            //string json = "{\"result\":[{\"cid\":152919,\"configuration\":{\"ident\":\"865905020671073\"},\"device_type_id\":57,\"id\":361201,\"ident\":\"865905020671073\",\"messages_ttl\":31536000,\"name\":\"УАЗ а025мо\",\"phone\":\"\"},{\"cid\":152919,\"configuration\":{\"ident\":\"865905021233899\"},\"device_type_id\":57,\"id\":361202,\"ident\":\"865905021233899\",\"messages_ttl\":31536000,\"name\":\"Нива т907хв\",\"phone\":\"\"}]}";
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string json = ReadFile("json1i.txt");
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
            //string json = "{\"result\":[{\"event_code\":300,\"event_origin\":\"gw\\/devices\\/361201\",\"event_text\":\"85.140.0.165\\/connected\",\"id\":361201,\"ident\":\"865905020671073\",\"source\":\"85.140.0.165\",\"timestamp\":1564730883.623162,\"transport\":\"tcp\"},{\"close_code\":3,\"duration\":10892,\"event_code\":301,\"event_origin\":\"gw\\/devices\\/361201\",\"event_text\":\"85.140.0.165\\/disconnected\",\"id\":361201,\"ident\":\"865905020671073\",\"msgs\":3022,\"recv\":252588,\"send\":1825,\"source\":\"85.140.0.165\",\"timestamp\":1564741773.556435,\"transport\":\"tcp\"}]}";
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string json = ReadFile("json2i.txt");
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
            //string json = "{\"result\":[{\"absolute.acceleration\":0,\"ain.1\":0,\"ain.2\":0,\"alarm.event\":false,"+
            //"\"alarm.mode.status\":false,\"battery.voltage\":4.096,\"brake.acceleration\":0,\"bump.acceleration\":0.35000000000000003,"+
            //"\"channel.id\":11489,\"device.id\":361201,\"device.name\":\"УАЗ а025мо\",\"device.temperature\":42,\"device.type.id\":57,"+
            //"\"engine.ignition.status\":true,\"external.powersource.voltage\":12.275,\"external.powersource.voltage.range.outside.status\":false,"+
            //"\"geofence.status\":false,\"gnss.antenna.status\":true,\"gnss.type\":\"glonass\",\"gsm.signal.level\":100,\"gsm.sim.status\":false,"+
            //"\"ibutton.connected.status\":false,\"ident\":\"865905020671073\",\"incline.event\":false,\"internal.battery.voltage.limit.lower.status\":false,"+
            //"\"internal.bus.supply.voltage.range.outside.status\":false,\"movement.status\":true,\"peer\":\"85.140.0.112:12032\",\"position.altitude\":27,"+
            //"\"position.direction\":300.2,\"position.hdop\":0.5,\"position.latitude\":51.44616,\"position.longitude\":46.107544,\"position.satellites\":15,"+
            //"\"position.speed\":0,\"position.valid\":true,\"protocol.id\":16,\"record.seqnum\":28932,\"rs232.sensor.value.0\":0,\"rs485.fuel.sensor.level.0\":0,"+
            //"\"rs485.fuel.sensor.level.1\":0,\"rs485.fuel.sensor.level.2\":0,\"server.timestamp\":1563781462.895147,\"shock.event\":false,\"timestamp\":1562511864,"+
            //"\"turn.acceleration\":0,\"x.acceleration\":-0.913978494623656,\"y.acceleration\":-0.1774193548387097,\"z.acceleration\":0.3279569892473118}]}";
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string json = ReadFile("json3i.txt");
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
            string /*json = "{\"result\":[{\"cid\":152919,\"configuration\":{\"ident\":\"865905020671073\"},\"device_type_id\":57,\"id\":361201,\"ident\":\"865905020671073\",\"messages_ttl\":31536000,\"name\":\"УАЗ а025мо\",\"phone\":\"\"},{\"cid\":152919,\"configuration\":{\"ident\":\"865905021233899\"},\"device_type_id\":57,\"id\":361202,\"ident\":\"865905021233899\",\"messages_ttl\":31536000,\"name\":\"Нива т907хв\",\"phone\":\"\"}]}";*/

            json = ReadFile("json4i.txt");

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            string fileName = "json4.txt";

            foreach (var a in dobj["result"])
            {
                foreach(var v in a["address"])
                {
                    Print(fileName, v.ToString());
                }
                foreach(var v in a["current"])
                {
                    Print(fileName, v.ToString());
                }
                Print(fileName, a["device_id"].ToString());
                Print(fileName, a["mode"].ToString());
                Print(fileName, a["name"].ToString());
                //Print(fileName, a["pending"].ToString());
                Print(fileName, a["tab"].ToString());
                Print(fileName, a["updated"].ToString());
            }
        }
        static void test5()
        {
            //string json = "{\"result\":[{\"id\":361201,\"telemetry\":{\"absolute.acceleration\":{\"ts\":1564740692,\"value\":0},\"ain.1\":{\"ts\":1564740692,\"value\":0},\"ain.2\":{\"ts\":1564740692,\"value\":0},\"alarm.event\":{\"ts\":1564740692,\"value\":false},\"alarm.mode.status\":{\"ts\":1564740692,\"value\":false},\"battery.voltage\":{\"ts\":1564740692,\"value\":3.512},\"brake.acceleration\":{\"ts\":1564740692,\"value\":0},\"bump.acceleration\":{\"ts\":1564740692,\"value\":0.22},\"device.temperature\":{\"ts\":1564740692,\"value\":38},\"engine.ignition.status\":{\"ts\":1564740692,\"value\":true},\"external.powersource.voltage\":{\"ts\":1564740692,\"value\":0},\"external.powersource.voltage.range.outside.status\":{\"ts\":1564740692,\"value\":true},\"geofence.status\":{\"ts\":1564740692,\"value\":false},\"gnss.antenna.status\":{\"ts\":1564740692,\"value\":true},\"gnss.type\":{\"ts\":1564740692,\"value\":\"glonass\"},\"gsm.signal.level\":{\"ts\":1564740692,\"value\":33},\"gsm.sim.status\":{\"ts\":1564740692,\"value\":false},\"hardware.version.enum\":{\"ts\":1564730883.623066,\"value\":17},\"ibutton.connected.status\":{\"ts\":1564740692,\"value\":false},\"ident\":{\"ts\":1564740692,\"value\":\"865905020671073\"},\"incline.event\":{\"ts\":1564740692,\"value\":false},\"internal.battery.voltage.limit.lower.status\":{\"ts\":1564740692,\"value\":true},\"internal.bus.supply.voltage.range.outside.status\":{\"ts\":1564740692,\"value\":true},\"movement.status\":{\"ts\":1564740692,\"value\":true},\"peer\":{\"ts\":1564740692,\"value\":\"85.140.0.165:43418\"},\"position\":{\"ts\":1564740692,\"value\":{\"altitude\":32,\"direction\":296.6,\"hdop\":0.6000000000000001,\"latitude\":51.446176,\"longitude\":46.107536,\"satellites\":15,\"speed\":0,\"valid\":true}},\"position.altitude\":{\"ts\":1564740692,\"value\":32},\"position.direction\":{\"ts\":1564740692,\"value\":296.6},\"position.hdop\":{\"ts\":1564740692,\"value\":0.6000000000000001},\"position.latitude\":{\"ts\":1564740692,\"value\":51.446176},\"position.longitude\":{\"ts\":1564740692,\"value\":46.107536},\"position.satellites\":{\"ts\":1564740692,\"value\":15},\"position.speed\":{\"ts\":1564740692,\"value\":0},\"position.valid\":{\"ts\":1564740692,\"value\":true},\"record.seqnum\":{\"ts\":1564740692,\"value\":3754},\"rs232.sensor.value.0\":{\"ts\":1564740692,\"value\":0},\"rs485.fuel.sensor.level.0\":{\"ts\":1564740692,\"value\":0},\"rs485.fuel.sensor.level.1\":{\"ts\":1564740692,\"value\":0},\"rs485.fuel.sensor.level.2\":{\"ts\":1564740692,\"value\":0},\"server.timestamp\":{\"ts\":1564740692,\"value\":1564740694.42362},\"shock.event\":{\"ts\":1564740692,\"value\":false},\"software.version.enum\":{\"ts\":1564730883.623066,\"value\":231},\"timestamp\":{\"ts\":1564740692,\"value\":1564740692},\"turn.acceleration\":{\"ts\":1564740692,\"value\":0},\"x.acceleration\":{\"ts\":1564740692,\"value\":-0.9301075268817204},\"y.acceleration\":{\"ts\":1564740692,\"value\":-0.1989247311827957},\"z.acceleration\":{\"ts\":1564740692,\"value\":0.3763440860215054}}}]}";
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string json = ReadFile("json5i.txt");
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            string fileName = "json5.txt";

            foreach (var a in dobj["result"])
            {
                Print(fileName, a["id"].ToString());
                Print(fileName, a["telemetry"]["absolute.acceleration"]["ts"].ToString());
                Print(fileName, a["telemetry"]["absolute.acceleration"]["value"].ToString());
                Print(fileName, a["telemetry"]["ain.1"]["ts"].ToString());
                Print(fileName, a["telemetry"]["ain.1"]["value"].ToString());
                Print(fileName, a["telemetry"]["ain.2"]["ts"].ToString());
                Print(fileName, a["telemetry"]["ain.2"]["value"].ToString());
                Print(fileName, a["telemetry"]["alarm.event"]["ts"].ToString());
                Print(fileName, a["telemetry"]["alarm.event"]["value"].ToString());
                Print(fileName, a["telemetry"]["alarm.mode.status"]["ts"].ToString());
                Print(fileName, a["telemetry"]["alarm.mode.status"]["value"].ToString());
                Print(fileName, a["telemetry"]["battery.voltage"]["ts"].ToString());
                Print(fileName, a["telemetry"]["battery.voltage"]["value"].ToString());
                Print(fileName, a["telemetry"]["brake.acceleration"]["ts"].ToString());
                Print(fileName, a["telemetry"]["brake.acceleration"]["value"].ToString());

                Print(fileName, a["telemetry"]["bump.acceleration"]["ts"].ToString());
                Print(fileName, a["telemetry"]["bump.acceleration"]["value"].ToString());

                Print(fileName, a["telemetry"]["device.temperature"]["ts"].ToString());
                Print(fileName, a["telemetry"]["device.temperature"]["value"].ToString());

                Print(fileName, a["telemetry"]["engine.ignition.status"]["ts"].ToString());
                Print(fileName, a["telemetry"]["engine.ignition.status"]["value"].ToString());

                Print(fileName, a["telemetry"]["external.powersource.voltage"]["ts"].ToString());
                Print(fileName, a["telemetry"]["external.powersource.voltage"]["value"].ToString());

                Print(fileName, a["telemetry"]["external.powersource.voltage.range.outside.status"]["ts"].ToString());
                Print(fileName, a["telemetry"]["external.powersource.voltage.range.outside.status"]["value"].ToString());
                Print(fileName, a["telemetry"]["geofence.status"]["ts"].ToString());
                Print(fileName, a["telemetry"]["geofence.status"]["value"].ToString());

                Print(fileName, a["telemetry"]["gnss.antenna.status"]["ts"].ToString());
                Print(fileName, a["telemetry"]["gnss.antenna.status"]["value"].ToString());
                Print(fileName, a["telemetry"]["gnss.type"]["ts"].ToString());
                Print(fileName, a["telemetry"]["gnss.type"]["value"].ToString());
                Print(fileName, a["telemetry"]["gsm.signal.level"]["ts"].ToString());
                Print(fileName, a["telemetry"]["gsm.signal.level"]["value"].ToString());
                Print(fileName, a["telemetry"]["gsm.sim.status"]["ts"].ToString());
                Print(fileName, a["telemetry"]["gsm.sim.status"]["value"].ToString());
                Print(fileName, a["telemetry"]["hardware.version.enum"]["ts"].ToString());
                Print(fileName, a["telemetry"]["hardware.version.enum"]["value"].ToString());
                Print(fileName, a["telemetry"]["ibutton.connected.status"]["ts"].ToString());
                Print(fileName, a["telemetry"]["ibutton.connected.status"]["value"].ToString());

                Print(fileName, a["telemetry"]["ident"]["ts"].ToString());
                Print(fileName, a["telemetry"]["ident"]["value"].ToString());
                Print(fileName, a["telemetry"]["incline.event"]["ts"].ToString());
                Print(fileName, a["telemetry"]["incline.event"]["value"].ToString());
                Print(fileName, a["telemetry"]["internal.battery.voltage.limit.lower.status"]["ts"].ToString());
                Print(fileName, a["telemetry"]["internal.battery.voltage.limit.lower.status"]["value"].ToString());
                Print(fileName, a["telemetry"]["internal.bus.supply.voltage.range.outside.status"]["ts"].ToString());
                Print(fileName, a["telemetry"]["internal.bus.supply.voltage.range.outside.status"]["value"].ToString());
                Print(fileName, a["telemetry"]["movement.status"]["ts"].ToString());
                Print(fileName, a["telemetry"]["movement.status"]["value"].ToString());

                Print(fileName, a["telemetry"]["peer"]["ts"].ToString());
                Print(fileName, a["telemetry"]["peer"]["value"].ToString());
                Print(fileName, a["telemetry"]["position"]["ts"].ToString());
                Print(fileName, a["telemetry"]["position"]["value"]["altitude"].ToString());
                Print(fileName, a["telemetry"]["position"]["value"]["direction"].ToString());
                Print(fileName, a["telemetry"]["position"]["value"]["hdop"].ToString());
                Print(fileName, a["telemetry"]["position"]["value"]["latitude"].ToString());
                Print(fileName, a["telemetry"]["position"]["value"]["longitude"].ToString());
                Print(fileName, a["telemetry"]["position"]["value"]["satellites"].ToString());
                Print(fileName, a["telemetry"]["position"]["value"]["speed"].ToString());
                Print(fileName, a["telemetry"]["position"]["value"]["valid"].ToString());
                Print(fileName, a["telemetry"]["position.altitude"]["ts"].ToString());
                Print(fileName, a["telemetry"]["position.altitude"]["value"].ToString());
                Print(fileName, a["telemetry"]["position.direction"]["ts"].ToString());
                Print(fileName, a["telemetry"]["position.direction"]["value"].ToString());
                Print(fileName, a["telemetry"]["position.hdop"]["ts"].ToString());
                Print(fileName, a["telemetry"]["position.hdop"]["value"].ToString());
                Print(fileName, a["telemetry"]["position.latitude"]["ts"].ToString());
                Print(fileName, a["telemetry"]["position.latitude"]["value"].ToString());
                Print(fileName, a["telemetry"]["position.longitude"]["ts"].ToString());
                Print(fileName, a["telemetry"]["position.longitude"]["value"].ToString());
                Print(fileName, a["telemetry"]["position.satellites"]["ts"].ToString());
                Print(fileName, a["telemetry"]["position.satellites"]["value"].ToString());
                Print(fileName, a["telemetry"]["position.speed"]["ts"].ToString());
                Print(fileName, a["telemetry"]["position.speed"]["value"].ToString());
                Print(fileName, a["telemetry"]["position.valid"]["ts"].ToString());
                Print(fileName, a["telemetry"]["position.valid"]["value"].ToString());
                Print(fileName, a["telemetry"]["record.seqnum"]["ts"].ToString());
                Print(fileName, a["telemetry"]["record.seqnum"]["value"].ToString());
                Print(fileName, a["telemetry"]["rs232.sensor.value.0"]["ts"].ToString());
                Print(fileName, a["telemetry"]["rs232.sensor.value.0"]["value"].ToString());
                Print(fileName, a["telemetry"]["rs485.fuel.sensor.level.0"]["ts"].ToString());
                Print(fileName, a["telemetry"]["rs485.fuel.sensor.level.0"]["value"].ToString());
                Print(fileName, a["telemetry"]["rs485.fuel.sensor.level.1"]["ts"].ToString());
                Print(fileName, a["telemetry"]["rs485.fuel.sensor.level.1"]["value"].ToString());
                Print(fileName, a["telemetry"]["rs485.fuel.sensor.level.2"]["ts"].ToString());
                Print(fileName, a["telemetry"]["rs485.fuel.sensor.level.2"]["value"].ToString());
                Print(fileName, a["telemetry"]["server.timestamp"]["ts"].ToString());
                Print(fileName, a["telemetry"]["server.timestamp"]["value"].ToString());
                Print(fileName, a["telemetry"]["shock.event"]["ts"].ToString());
                Print(fileName, a["telemetry"]["shock.event"]["value"].ToString());
                Print(fileName, a["telemetry"]["software.version.enum"]["ts"].ToString());
                Print(fileName, a["telemetry"]["software.version.enum"]["value"].ToString());
                Print(fileName, a["telemetry"]["timestamp"]["ts"].ToString());
                Print(fileName, a["telemetry"]["timestamp"]["value"].ToString());
                Print(fileName, a["telemetry"]["turn.acceleration"]["ts"].ToString());
                Print(fileName, a["telemetry"]["turn.acceleration"]["value"].ToString());
                Print(fileName, a["telemetry"]["x.acceleration"]["ts"].ToString());
                Print(fileName, a["telemetry"]["x.acceleration"]["value"].ToString());
                Print(fileName, a["telemetry"]["y.acceleration"]["ts"].ToString());
                Print(fileName, a["telemetry"]["y.acceleration"]["value"].ToString());
                Print(fileName, a["telemetry"]["z.acceleration"]["ts"].ToString());
                Print(fileName, a["telemetry"]["z.acceleration"]["value"].ToString());
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
        /*
         string headername = "TokenName";
string headervalue = "0000000000";
var request = (HttpWebRequest)WebRequest.Create("https://URL");
request.Method = "DELETE";
request.Headers.Add(headername, headervalue);
try
{ var response = (HttpWebResponse)request.GetResponse(); var responseString = new StreamReader(response.GetResponseStream()).*********();
var jss = new JavaScriptSerializer(); var dict = jss.Deserialize<dynamic>(responseString); string message += "deleted Item with id" + dict["id"];
}
catch
{ string message += "Didn't delete Item";
} */
        static void ClientDelete(string url, string mask = "all")
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Headers.Add(token);
            httpWebRequest.Headers.Add(mask);
            httpWebRequest.Accept = "application/json";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.KeepAlive = true;
            httpWebRequest.Method = "DELETE";
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    Print(log, streamReader.ReadToEnd());
                }
            }
            catch (WebException ex)
            {
                Print(log, ex.Message);
                //Console.WriteLine(ex.Response.Headers.ToString());
                using (var streamReader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    Print(log, streamReader.ReadToEnd());
                }
            }
        }
        static void ClientPut(string url, string mask = "all")
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Headers.Add(token);
            httpWebRequest.Headers.Add(mask);
            httpWebRequest.Accept = "application/json";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.KeepAlive = true;
            httpWebRequest.Method = "PUT";
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    Print(log, streamReader.ReadToEnd());
                }
            }
            catch (WebException ex)
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
