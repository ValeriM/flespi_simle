using System;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;

//using System.Threading;
//using System.Diagnostics;
//using OpenNETCF.MQTT;
//using System.Net.WebSockets;
//using System.Threading.Tasks;
using System.Text;

using System.Data;
using System.Data.SqlClient;

using System.Collections.Generic;

using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace flespi_simle
{
    class Program
    {
        static readonly string token = "Authorization: FlespiToken Vo6wSNjDEM19qzUdq9qbwZugZPmPl3N4hHq0lAtPalMqIwuYuKZQxiUnX7060B17";
        //token = "Authorization: FlespiToken UIy8bexWRWLVX3H3yJFCkycRTNI3xRognMeoOBbvlKf8EK20kvrsRraz4GsqnGwB";
        //token = "Authorization: FlespiToken lCG8yJPUWRc9awe3M2AaTuKcqd5N4Nvgd1cByPklwkiuGmogcOgW6QWmURXOujSx";
        static readonly string log = "flespi_simle.txt";
        static MqttClient client;
        static string clientId;
        static string conStr = Properties.Settings.Default.Connection;

        static void Main(string[] args)
        {
            //string conStr = Properties.Settings.Default.Connection;

            Print(log, conStr);

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
                "e. Reset setting value",
                "f. Get specified modems info."
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
                    case '1': Client("https://flespi.io/gw/devices/all"); break;
                    case '2': Client("https://flespi.io/gw/devices/361201/logs"); break;
                    case '3': Client("https://flespi.io/gw/devices/361202/messages"); break;
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
                    case 'f': Client("https://flespi.io/gw/modems/6624"); break;
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
            ClientReceiveTest1();
            //SendTicksRequest();
            //test_zero();
            //Parse("flespi/state/gw/devices/361202/telemetry/z.acceleration", "-0.10101010101");
        }
        /**/
        class Test6
        {

        };
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
            string json = Client("https://flespi.io/gw/devices/all"); //"{\"result\":[{\"cid\":152919,\"configuration\":{\"ident\":\"865905020671073\"},\"device_type_id\":57,\"id\":361201,\"ident\":\"865905020671073\",\"messages_ttl\":31536000,\"name\":\"УАЗ а025мо\",\"phone\":\"\"},{\"cid\":152919,\"configuration\":{\"ident\":\"865905021233899\"},\"device_type_id\":57,\"id\":361202,\"ident\":\"865905021233899\",\"messages_ttl\":31536000,\"name\":\"Нива т907хв\",\"phone\":\"\"}]}";

            SqlConnection sqlConnection = new SqlConnection(conStr);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("", sqlConnection);
            //sqlCommand.CommandText = "Insert Into Devices (cid, configuration_ident, device_type_id, id, ident, messages_ttl, name, phone) Values (@cid, @configuration_ident, @device_type_id, @id, @ident, @messages_ttl, @name, @phone)";
            sqlCommand.CommandText = "Insert Into Devices (cid, configuration_ident, device_type_id, id, messages_ttl, name) Values (@cid, @configuration_ident, @device_type_id, @id, @messages_ttl, @name)";

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            //string json = ReadFile("json1i.txt");
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            string fileName = "json1.txt";
            int rows = 0;

            foreach(var a in dobj["result"])
            {
                Print(fileName, a["cid"].ToString());
                Print(fileName, a["configuration"]["ident"]);
                Print(fileName, a["device_type_id"].ToString());
                Print(fileName, a["id"].ToString());
                //Print(fileName, a["ident"].ToString());
                Print(fileName, a["messages_ttl"].ToString());
                Print(fileName, a["name"].ToString());
                //Print(fileName, a["phone"].ToString());

                sqlCommand.Parameters.Add("@cid", SqlDbType.Int);
                sqlCommand.Parameters["@cid"].Value = a["cid"].ToString();

                sqlCommand.Parameters.Add("@configuration_ident", SqlDbType.VarChar);
                sqlCommand.Parameters["@configuration_ident"].Value = a["configuration"]["ident"].ToString();


                sqlCommand.Parameters.Add("@device_type_id", SqlDbType.Int);
                sqlCommand.Parameters["@device_type_id"].Value = a["device_type_id"].ToString();

                sqlCommand.Parameters.Add("@id", SqlDbType.Int);
                sqlCommand.Parameters["@id"].Value = a["id"].ToString();

                //sqlCommand.Parameters.Add("@ident", SqlDbType.VarChar);
                //sqlCommand.Parameters["@ident"].Value = a["ident"].ToString();

                sqlCommand.Parameters.Add("@messages_ttl", SqlDbType.Int);
                sqlCommand.Parameters["@messages_ttl"].Value = a["messages_ttl"].ToString();

                sqlCommand.Parameters.Add("@name", SqlDbType.VarChar);
                sqlCommand.Parameters["@name"].Value = a["name"].ToString();

                //sqlCommand.Parameters.Add("@phone", SqlDbType.VarChar);
                //sqlCommand.Parameters["@phone"].Value = a["phone"].ToString();

                rows += sqlCommand.ExecuteNonQuery();

                sqlCommand.Parameters.Clear();
            }
            sqlConnection.Close();
            Print(fileName, "Row(s) affected " + rows.ToString());
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
        class Test3
        {
            int id;
            int absolute_acceleration;
            int ain_1;
            int ain_2;
            bool alarm_event;
            bool alarm_mode_status;
            decimal battery_voltage;
            decimal brake_acceleration;
            decimal bump_acceleration;
            int channel_id;
            int device_id;
            string device_name;
            int device_temperature;
            int device_type_id;
            bool engine_ignition_status;
            decimal external_powersource_voltage;
            bool external_powersource_voltage_range_outside_status;
            bool geofence_status;
            bool gnss_antenna_status;
            string gnss_type;
            int gsm_signal_level;
            bool gsm_sim_status;
            bool ibutton_connected_status;
            long ident;
            bool incline_event;
            bool internal_battery_voltage_limit_lower_status;
            bool internal_bus_supply_voltage_range_outside_status;
            bool movement_status;
            string peer;
            decimal position_altitude;
            decimal position_direction;
            decimal position_hdop;
            decimal position_latitude;
            decimal position_longitude;
            int position_satellites;
            int position_speed;
            bool position_valid;
            int protocol_id;
            int record_seqnum;
            int rs232_sensor_value_0;
            int rs485_fuel_sensor_level_0;
            int rs485_fuel_sensor_level_1;
            int rs485_fuel_sensor_level_2;
            decimal server_timestamp;
            bool shock_event;
            decimal timestamp;
            int turn_acceleration;
            double x_acceleration;
            double y_acceleration;
            double z_acceleration;
        };
        static void test_zero()
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string json = ReadFile("json3i.txt");
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            Dictionary<string, SqlDbType> fields = new Dictionary<string, SqlDbType>
            {
               {"absolute.acceleration", SqlDbType.Int},//0
               {"alarm.event", SqlDbType.Bit },//1
               {"alarm.mode.status", SqlDbType.Bit},//2
               {"brake.acceleration", SqlDbType.Int},//3
               { "bump.acceleration",SqlDbType.Real },//4
               { "channel.id", SqlDbType.Int},//5
               { "device.id", SqlDbType.Int},//6
               { "device.name",SqlDbType.VarChar },//7
               { "device.type.id",SqlDbType.Int },//8
               { "engine.ignition.status",SqlDbType.Bit },//9
                { "external.powersource.voltage",SqlDbType.Real },//10
                { "external.powersource.voltage.range.outside.status",SqlDbType.Bit },//11
                { "geofence.status",SqlDbType.Bit },//12
                { "gnss.antenna.status",SqlDbType.Bit },//13
                { "gnss.type",SqlDbType.VarChar },//14
                { "gsm.signal.level",SqlDbType.Int },//15
                { "gsm.sim.status",SqlDbType.Bit },//16
                { "ibutton.connected.status",SqlDbType.Bit },//17
                { "ident",SqlDbType.VarChar },//18
                { "incline.event",SqlDbType.Bit },//19
                { "internal.battery.voltage.limit.lower.status",SqlDbType.Bit },//20
                { "internal.bus.supply.voltage.range.outside.status",SqlDbType.Bit },//21
                { "movement.status",SqlDbType.Bit },//22
                { "peer",SqlDbType.VarChar },//23
                { "position.altitude",SqlDbType.Real },//24
                { "position.direction",SqlDbType.Real },//25
                { "position.hdop",SqlDbType.Real },//26
                { "position.latitude",SqlDbType.Real },//27
                { "position.longitude",SqlDbType.Real },//28
                { "position.satellites",SqlDbType.Real },//29
                { "position.speed",SqlDbType.Real },//30
                { "position.valid",SqlDbType.Bit },//31
                { "protocol.id",SqlDbType.Int },//32
                { "record.seqnum",SqlDbType.Int },//33
                { "server.timestamp",SqlDbType.Real },//34
                { "shock.event",SqlDbType.Bit },//35
                { "timestamp",SqlDbType.BigInt },//36
                { "turn.acceleration",SqlDbType.Int },//37
                { "x.acceleration",SqlDbType.Real },//38
                { "y.acceleration",SqlDbType.Real },//39
                { "z.acceleration",SqlDbType.Real }//40
            };

            SqlConnection sqlConnection = new SqlConnection(conStr);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("", sqlConnection);
            //sqlCommand.CommandText = "Insert Into Messages ([absolute.acceleration]) Values (@p0)";
            int rows = 0;
            string sql = "Insert Into Messages (";
            string sql2 = "(";

            int rmax = 41;

            foreach(var field in fields)
            {
                sql += "[" + field.Key + "],";
                sql2 += "@p" + rows.ToString() + ",";
                rows++;
                if (rows == rmax) break;
            }
            sql = sql.Remove(sql.Length-1);
            sql2 = sql2.Remove(sql2.Length - 1);
            sql += ") Values " + sql2 + ")";

            sqlCommand.CommandText = sql;

            rows = 0;

            foreach (var a in dobj["result"])
            {
                foreach (var field in fields)
                {
                    //sqlCommand.Parameters.Add("@[" + field.Key + "]", field.Value);
                    //sqlCommand.Parameters["@[" + field.Key + "]"].Value = a[field.Key].ToString();
                    sqlCommand.Parameters.Add("@p"+rows.ToString(), field.Value);
                    sqlCommand.Parameters["@p"+rows.ToString()].Value = a[field.Key].ToString();
                    Print("log.txt", field.Key + " = " +a[field.Key].ToString());
                    rows++;
                    if (rows == rmax) break;
                }
                //rows += sqlCommand.ExecuteNonQuery();
                //sqlCommand.Parameters.Clear();
            }



            rows += sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        static void test3(string json = "")
        {
            
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            //string json = ReadFile("json3i.txt");
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            string fileName = "json3.txt";

            string[] fields =
            {
                "absolute.acceleration",//0
                //"ain.1",
                //"ain.2",
                "alarm.event",//1
                "alarm.mode.status",//2
                //"battery.voltage",
                "brake.acceleration",//3
                "bump.acceleration",//4
                "channel.id",//5
                "device.id",//6
                "device.name",//7
                //"device.temperature",
                "device.type.id",//
                "engine.ignition.status",//
                "external.powersource.voltage",//
                "external.powersource.voltage.range.outside.status",//
                "geofence.status",//
                "gnss.antenna.status",//
                "gnss.type",//
                "gsm.signal.level",//
                "gsm.sim.status",//
                "ibutton.connected.status",//
                "ident",//
                "incline.event",//
                "internal.battery.voltage.limit.lower.status",//
                "internal.bus.supply.voltage.range.outside.status",//
                "movement.status",//
                "peer",//
                "position.altitude",//
                "position.direction",//
                "position.hdop",//
                "position.latitude",//
                "position.longitude",//
                "position.satellites",//
                "position.speed",//
                "position.valid",//
                "protocol.id",//
                "record.seqnum",//
                //"rs232.sensor.value.0",
                //"rs485.fuel.sensor.level.0",
                //"rs485.fuel.sensor.level.1",
                //"rs485.fuel.sensor.level.2",
                "server.timestamp",//
                "shock.event",//
                "timestamp",//
                "turn.acceleration",//
                "x.acceleration",//
                "y.acceleration",//
                "z.acceleration"//
            };
            SqlConnection sqlConnection = new SqlConnection(conStr);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("", sqlConnection);
            sqlCommand.CommandText = @"INSERT INTO [dbo].[Messages]
           ([absolute.acceleration]
           ,[alarm.event]
           ,[alarm.mode.status]
           ,[brake.acceleration]
           ,[bump.acceleration]
           ,[channel.id]
           ,[device.id]
           ,[device.name]
           ,[device.type.id]
           ,[engine.ignition.status]
           ,[external.powersource.voltage]
           ,[external.powersource.voltage.range.outside.status]
           ,[geofence.status]
           ,[gnss.antenna.status]
           ,[gnss.type]
           ,[gsm.signal.level]
           ,[gsm.sim.status]
           ,[ibutton.connected.status]
           ,[ident]
           ,[incline.event]
           ,[internal.battery.voltage.limit.lower.status]
           ,[internal.bus.supply.voltage.range.outside.status]
           ,[movement.status]
           ,[peer]
           ,[position.altitude]
           ,[position.direction]
           ,[position.hdop]
           ,[position.latitude]
           ,[position.longitude]
           ,[position.satellites]
           ,[position.speed]
           ,[position.valid]
           ,[protocol.id]
           ,[record.seqnum]
           ,[server.timestamp]
           ,[shock.event]
           ,[timestamp]
           ,[turn.acceleration]
           ,[x.acceleration]
           ,[y.acceleration]
           ,[z.acceleration])
     VALUES
           (<absolute.acceleration, int,>
           ,<alarm.event, bit,>
           ,<alarm.mode.status, int,>
           ,<brake.acceleration, int,>
           ,<bump.acceleration, numeric(18,0),>
           ,<channel.id, int,>
           ,<device.id, int,>
           ,<device.name, nvarchar(50),>
           ,<device.type.id, int,>
           ,<engine.ignition.status, bit,>
           ,<external.powersource.voltage, numeric(18,0),>
           ,<external.powersource.voltage.range.outside.status, bit,>
           ,<geofence.status, bit,>
           ,<gnss.antenna.status, bit,>
           ,<gnss.type, nvarchar(50),>
           ,<gsm.signal.level, int,>
           ,<gsm.sim.status, bit,>
           ,<ibutton.connected.status, bit,>
           ,<ident, nvarchar(255),>
           ,<incline.event, bit,>
           ,<internal.battery.voltage.limit.lower.status, bit,>
           ,<internal.bus.supply.voltage.range.outside.status, int,>
           ,<movement.status, bit,>
           ,<peer, nvarchar(50),>
           ,<position.altitude, numeric(6,2),>
           ,<position.direction, numeric(6,2),>
           ,<position.hdop, numeric(6,2),>
           ,<position.latitude, numeric(8,6),>
           ,<position.longitude, numeric(8,6),>
           ,<position.satellites, numeric(6,2),>
           ,<position.speed, numeric(6,2),>
           ,<position.valid, bit,>
           ,<protocol.id, int,>
           ,<record.seqnum, int,>
           ,<server.timestamp, numeric(18,6),>
           ,<shock.event, bit,>
           ,<timestamp, bigint,>
           ,<turn.acceleration, int,>
           ,<x.acceleration, numeric(18,15),>
           ,<y.acceleration, numeric(18,15),>
           ,<z.acceleration, numeric(18,15),>)";

            int rows = 0;

            foreach (var a in dobj["result"])
            {
                foreach (var field in fields)
                {
                    //Print(fileName, "-------------- " + field + " ---------------");
                    Print(fileName, a[field].ToString());

                    sqlCommand.Parameters.Add("@["+field+"]", SqlDbType.Int);
                    sqlCommand.Parameters["@["+field+"]"].Value = a[field].ToString();

                    
                }
                sqlCommand.CommandText = "";
                rows += sqlCommand.ExecuteNonQuery();
                //sqlCommand.Parameters.Clear();
            }
            sqlConnection.Close();
            Print(fileName, "Row(s) affected " + rows.ToString());
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
        static string Client(string URI)
        {
            string request = string.Empty;
            try
            {
                WebClient webClient = new WebClient();
                //webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                webClient.Headers.Add("Accept: application/json");
                webClient.Headers.Add(token);
                Stream stream = webClient.OpenRead(URI);
                StreamReader reader = new StreamReader(stream);
                request = reader.ReadToEnd();
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
            return request;
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
        static void ClientReceiveTest11()
        {
            MqttClient client = new MqttClient("test.mosquitto.org");
            byte code = client.Connect(Guid.NewGuid().ToString()/*, token, ""*/);

            Console.WriteLine("Protocol " + client.ProtocolVersion);

            Console.WriteLine("Connected " + client.IsConnected);
            Console.WriteLine(client.Settings.Port + " " + client.Settings.SslPort);

            client.MqttMsgPublished += client_MqttMsgPublished;
            client.MqttMsgSubscribed += client_MqttMsgSubscribed;
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            client.MqttMsgUnsubscribed += client_MqttMsgUnsubscribed;

            ushort msgId = client.Publish("/my_topic", // topic
                           Encoding.UTF8.GetBytes("MyMessageBody"), // message body
                           MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                           false); // retained
            ushort msgId2 = client.Subscribe(new string[] { "/topic_1", "/topic_2" },
                            new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                            MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            //client.Disconnect();
        }

        static void client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            Console.WriteLine("MessageId = " + e.MessageId + " Published = " + e.IsPublished);
        }
        static void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Print("received.txt", "Subscribed for id = " + e.MessageId);
        }
        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            settings(e.Topic, "{\"result\":[" + Encoding.UTF8.GetString(e.Message) + "]}");
            //Print("received.txt", "Received = " + Encoding.UTF8.GetString(e.Message) + " on topic " + e.Topic);
            //Print("Received.txt", Encoding.UTF8.GetString(e.Message) + " " + e.Topic);
            ///// работает test3("{\"result\":["+Encoding.UTF8.GetString(e.Message)+"]}");
        }
        static void client_MqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
        {
            Console.WriteLine("Unsubscribed for id = " + e.MessageId);
        }
        static void Parse2(string topic, string message)
        {

        }
        static void Parse(string topic, string message)
        {
            string [] headers = topic.Split('/');
            string device_id = headers[4];
            string field = headers[6];
            string value = message;
            SqlConnection sqlConnection = new SqlConnection(conStr);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("Select recid From Telemetry Where [device.id] = " + device_id, sqlConnection);
            int rows = 0;

            Object o = sqlCommand.ExecuteScalar();
            sqlCommand.CommandText = "SELECT t.name FROM sys.columns AS c JOIN sys.types AS t ON c.user_type_id = t.user_type_id WHERE c.object_id = OBJECT_ID('Telemetry') and c.name = '" + field + "'";
            Object t = sqlCommand.ExecuteScalar();
            string s = "";
            if (t.ToString() == "nvarchar")
            {
                s = "'";
            }
            if (value == "true")
                value = "1";
            if (value == "false")
                value = "0";
            if (o == null)
            {
                sqlCommand.CommandText = "Insert Into Telemetry ([device.id], ["+field+"]) Values (" + device_id + ", " + s+value+s + ")";
            }
            else
            {
                sqlCommand.CommandText = "Update Telemetry Set [" + field + "] = " + s+value+s + " Where [device.id] = " + device_id;
            }

            rows = sqlCommand.ExecuteNonQuery();

            sqlConnection.Close();
        }
        static void settings(string topic, string message)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string json = message;
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);

            string[] headers = topic.Split('/');

            string field = headers[6];

            foreach (var a in dobj["result"])
            {
                switch (field)
                {
                    case "sin4":
                        Print("settings.txt", a["delay"].ToString());
                        Print("settings.txt", a["sms"].ToString());
                        break;
                    case "sout0":
                        Print("settings.txt", a["amode"]["count"].ToString());
                        Print("settings.txt", a["amode"]["dur"].ToString());
                        break;
                    default: break;
                }
                //Print(fileName, a["telemetry"]["absolute.acceleration"]["ts"].ToString());

                Print("settings.txt", "topic: " + topic + " message " + message);
                Print("settings.txt", "------------------------------------------------------");
            }
        }
        static void ClientReceiveTest1()
        {
            string BrokerAddress = "mqtt.flespi.io";
            client = new MqttClient(BrokerAddress);

            // register a callback-function (we have to implement, see below) which is called by the library when a message was received
            client.MqttMsgSubscribed += client_MqttMsgSubscribed;
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            // use a unique id as client id, each time we start the application
            clientId = Guid.NewGuid().ToString();

            client.Connect(clientId, "FlespiToken Vo6wSNjDEM19qzUdq9qbwZugZPmPl3N4hHq0lAtPalMqIwuYuKZQxiUnX7060B17", "");

            Console.WriteLine(client.IsConnected ? "Connected" : "Хуй там");

            //*** работает ***
            //ushort code = client.Subscribe(new string[] { "flespi/message/gw/devices/361202" }, new byte[] { 2 });

            //ushort code2 = client.Subscribe(new string[] { "flespi/state/gw/devices/361201" }, new byte[] { 2 });
            ushort code = client.Subscribe(new string[] { "flespi/state/gw/devices/361202/settings/+" }, new byte[] { 2 });

            //ushort code = client.Subscribe(new string[] { "flespi/state/gw/devices/361202/telemetry/+" }, new byte[] { 2 });
        }
    }
}
