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
        static readonly string log = "flespi_simle.txt";
        static MqttClient client;
        static string clientId;
        static string conStr = Properties.Settings.Default.Connection;

        static void Main(string[] args)
        {
            //string conStr = Properties.Settings.Default.Connection;

            Utils.Print(log, conStr);

            Utils.Print(log, "Start " + DateTime.Now.ToString());

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
                //    zu.Print(log, "Selected " + menu[(menuitem-48)]);
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
            //Tests.ClientReceiveTest11(client_MqttMsgPublished, null, null, null);
            //SendTicksRequest();
            //test_zero();
            //Parse("flespi/state/gw/devices/361202/telemetry/z.acceleration", "-0.10101010101");
        }
        /**/

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
                Utils.Print(fileName, a["cid"].ToString());
                Utils.Print(fileName, a["configuration"]["ident"]);
                Utils.Print(fileName, a["device_type_id"].ToString());
                Utils.Print(fileName, a["id"].ToString());
                //zu.Print(fileName, a["ident"].ToString());
                Utils.Print(fileName, a["messages_ttl"].ToString());
                Utils.Print(fileName, a["name"].ToString());
                //zu.Print(fileName, a["phone"].ToString());

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
            Utils.Print(fileName, "Row(s) affected " + rows.ToString());
        }
        /**/
        
        /**/
        //
        static void test_zero()
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string json = Utils.ReadFile("json3i.txt");
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            Dictionary<string, SqlDbType> fields = new Dictionary<string, SqlDbType>
            {
               {"absolute_acceleration", SqlDbType.Int},//0
               {"alarm_event", SqlDbType.Bit },//1
               {"alarm_mode_status", SqlDbType.Bit},//2
               {"brake_acceleration", SqlDbType.Int},//3
               { "bump_acceleration",SqlDbType.Real },//4
               { "channel_id", SqlDbType.Int},//5
               { "device_id", SqlDbType.Int},//6
               { "device_name",SqlDbType.VarChar },//7
               { "device_type_id",SqlDbType.Int },//8
               { "engine_ignition_status",SqlDbType.Bit },//9
                { "external_powersource_voltage",SqlDbType.Real },//10
                { "external_powersource_voltage_range_outside_status",SqlDbType.Bit },//11
                { "geofence_status",SqlDbType.Bit },//12
                { "gnss_antenna_status",SqlDbType.Bit },//13
                { "gnss_type",SqlDbType.VarChar },//14
                { "gsm_signal_level",SqlDbType.Int },//15
                { "gsm_sim_status",SqlDbType.Bit },//16
                { "ibutton_connected_status",SqlDbType.Bit },//17
                { "ident",SqlDbType.VarChar },//18
                { "incline_event",SqlDbType.Bit },//19
                { "internal_battery_voltage_limit_lower_status",SqlDbType.Bit },//20
                { "internal_bus_supply_voltage_range_outside_status",SqlDbType.Bit },//21
                { "movement_status",SqlDbType.Bit },//22
                { "peer",SqlDbType.VarChar },//23
                { "position_altitude",SqlDbType.Real },//24
                { "position_direction",SqlDbType.Real },//25
                { "position_hdop",SqlDbType.Real },//26
                { "position_latitude",SqlDbType.Real },//27
                { "position_longitude",SqlDbType.Real },//28
                { "position_satellites",SqlDbType.Real },//29
                { "position_speed",SqlDbType.Real },//30
                { "position_valid",SqlDbType.Bit },//31
                { "protocol_id",SqlDbType.Int },//32
                { "record_seqnum",SqlDbType.Int },//33
                { "server_timestamp",SqlDbType.Real },//34
                { "shock_event",SqlDbType.Bit },//35
                { "timestamp",SqlDbType.BigInt },//36
                { "turn_acceleration",SqlDbType.Int },//37
                { "x_acceleration",SqlDbType.Real },//38
                { "y_acceleration",SqlDbType.Real },//39
                { "z_acceleration",SqlDbType.Real }//40
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
                    Utils.Print("log.txt", field.Key + " = " +a[field.Key].ToString());
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
                "absolute_acceleration",//0
                //"ain_1",
                //"ain_2",
                "alarm_event",//1
                "alarm_mode_status",//2
                //"battery_voltage",
                "brake_acceleration",//3
                "bump_acceleration",//4
                "channel_id",//5
                "device_id",//6
                "device_name",//7
                //"device_temperature",
                "device_type_id",//
                "engine_ignition_status",//
                "external_powersource_voltage",//
                "external_powersource_voltage_range_outside_status",//
                "geofence_status",//
                "gnss_antenna_status",//
                "gnss_type",//
                "gsm_signal_level",//
                "gsm_sim_status",//
                "ibutton_connected_status",//
                "ident",//
                "incline_event",//
                "internal_battery_voltage_limit_lower_status",//
                "internal_bus_supply_voltage_range_outside_status",//
                "movement_status",//
                "peer",//
                "position_altitude",//
                "position_direction",//
                "position_hdop",//
                "position_latitude",//
                "position_longitude",//
                "position_satellites",//
                "position_speed",//
                "position_valid",//
                "protocol_id",//
                "record_seqnum",//
                //"rs232_sensor_value_0",
                //"rs485_fuel_sensor_level_0",
                //"rs485_fuel_sensor_level_1",
                //"rs485_fuel_sensor_level_2",
                "server_timestamp",//
                "shock_event",//
                "timestamp",//
                "turn_acceleration",//
                "x_acceleration",//
                "y_acceleration",//
                "z_acceleration"//
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
                    //zu.Print(fileName, "-------------- " + field + " ---------------");
                    Utils.Print(fileName, a[field].ToString());

                    sqlCommand.Parameters.Add("@["+field+"]", SqlDbType.Int);
                    sqlCommand.Parameters["@["+field+"]"].Value = a[field].ToString();

                    
                }
                sqlCommand.CommandText = "";
                rows += sqlCommand.ExecuteNonQuery();
                //sqlCommand.Parameters.Clear();
            }
            sqlConnection.Close();
            Utils.Print(fileName, "Row(s) affected " + rows.ToString());
        }
        static void test4()
        {
            string /*json = "{\"result\":[{\"cid\":152919,\"configuration\":{\"ident\":\"865905020671073\"},\"device_type_id\":57,\"id\":361201,\"ident\":\"865905020671073\",\"messages_ttl\":31536000,\"name\":\"УАЗ а025мо\",\"phone\":\"\"},{\"cid\":152919,\"configuration\":{\"ident\":\"865905021233899\"},\"device_type_id\":57,\"id\":361202,\"ident\":\"865905021233899\",\"messages_ttl\":31536000,\"name\":\"Нива т907хв\",\"phone\":\"\"}]}";*/

            json = Utils.ReadFile("json4i.txt");

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            string fileName = "json4.txt";

            foreach (var a in dobj["result"])
            {
                foreach(var v in a["address"])
                {
                    Utils.Print(fileName, v.ToString());
                }
                foreach(var v in a["current"])
                {
                    Utils.Print(fileName, v.ToString());
                }
                Utils.Print(fileName, a["device_id"].ToString());
                Utils.Print(fileName, a["mode"].ToString());
                Utils.Print(fileName, a["name"].ToString());
                //zu.Print(fileName, a["pending"].ToString());
                Utils.Print(fileName, a["tab"].ToString());
                Utils.Print(fileName, a["updated"].ToString());
            }
        }
        static void test5()
        {
            //string json = "{\"result\":[{\"id\":361201,\"telemetry\":{\"absolute.acceleration\":{\"ts\":1564740692,\"value\":0},\"ain.1\":{\"ts\":1564740692,\"value\":0},\"ain.2\":{\"ts\":1564740692,\"value\":0},\"alarm.event\":{\"ts\":1564740692,\"value\":false},\"alarm.mode.status\":{\"ts\":1564740692,\"value\":false},\"battery.voltage\":{\"ts\":1564740692,\"value\":3.512},\"brake.acceleration\":{\"ts\":1564740692,\"value\":0},\"bump.acceleration\":{\"ts\":1564740692,\"value\":0.22},\"device.temperature\":{\"ts\":1564740692,\"value\":38},\"engine.ignition.status\":{\"ts\":1564740692,\"value\":true},\"external.powersource.voltage\":{\"ts\":1564740692,\"value\":0},\"external.powersource.voltage.range.outside.status\":{\"ts\":1564740692,\"value\":true},\"geofence.status\":{\"ts\":1564740692,\"value\":false},\"gnss.antenna.status\":{\"ts\":1564740692,\"value\":true},\"gnss.type\":{\"ts\":1564740692,\"value\":\"glonass\"},\"gsm.signal.level\":{\"ts\":1564740692,\"value\":33},\"gsm.sim.status\":{\"ts\":1564740692,\"value\":false},\"hardware.version.enum\":{\"ts\":1564730883.623066,\"value\":17},\"ibutton.connected.status\":{\"ts\":1564740692,\"value\":false},\"ident\":{\"ts\":1564740692,\"value\":\"865905020671073\"},\"incline.event\":{\"ts\":1564740692,\"value\":false},\"internal.battery.voltage.limit.lower.status\":{\"ts\":1564740692,\"value\":true},\"internal.bus.supply.voltage.range.outside.status\":{\"ts\":1564740692,\"value\":true},\"movement.status\":{\"ts\":1564740692,\"value\":true},\"peer\":{\"ts\":1564740692,\"value\":\"85.140.0.165:43418\"},\"position\":{\"ts\":1564740692,\"value\":{\"altitude\":32,\"direction\":296.6,\"hdop\":0.6000000000000001,\"latitude\":51.446176,\"longitude\":46.107536,\"satellites\":15,\"speed\":0,\"valid\":true}},\"position.altitude\":{\"ts\":1564740692,\"value\":32},\"position.direction\":{\"ts\":1564740692,\"value\":296.6},\"position.hdop\":{\"ts\":1564740692,\"value\":0.6000000000000001},\"position.latitude\":{\"ts\":1564740692,\"value\":51.446176},\"position.longitude\":{\"ts\":1564740692,\"value\":46.107536},\"position.satellites\":{\"ts\":1564740692,\"value\":15},\"position.speed\":{\"ts\":1564740692,\"value\":0},\"position.valid\":{\"ts\":1564740692,\"value\":true},\"record.seqnum\":{\"ts\":1564740692,\"value\":3754},\"rs232.sensor.value.0\":{\"ts\":1564740692,\"value\":0},\"rs485.fuel.sensor.level.0\":{\"ts\":1564740692,\"value\":0},\"rs485.fuel.sensor.level.1\":{\"ts\":1564740692,\"value\":0},\"rs485.fuel.sensor.level.2\":{\"ts\":1564740692,\"value\":0},\"server.timestamp\":{\"ts\":1564740692,\"value\":1564740694.42362},\"shock.event\":{\"ts\":1564740692,\"value\":false},\"software.version.enum\":{\"ts\":1564730883.623066,\"value\":231},\"timestamp\":{\"ts\":1564740692,\"value\":1564740692},\"turn.acceleration\":{\"ts\":1564740692,\"value\":0},\"x.acceleration\":{\"ts\":1564740692,\"value\":-0.9301075268817204},\"y.acceleration\":{\"ts\":1564740692,\"value\":-0.1989247311827957},\"z.acceleration\":{\"ts\":1564740692,\"value\":0.3763440860215054}}}]}";
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string json = Utils.ReadFile("json5i.txt");
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            string fileName = "json5.txt";

            foreach (var a in dobj["result"])
            {
                Utils.Print(fileName, a["id"].ToString());
                Utils.Print(fileName, a["telemetry"]["absolute.acceleration"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["absolute.acceleration"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["ain.1"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["ain.1"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["ain.2"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["ain.2"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["alarm.event"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["alarm.event"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["alarm.mode.status"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["alarm.mode.status"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["battery.voltage"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["battery.voltage"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["brake.acceleration"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["brake.acceleration"]["value"].ToString());

                Utils.Print(fileName, a["telemetry"]["bump.acceleration"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["bump.acceleration"]["value"].ToString());

                Utils.Print(fileName, a["telemetry"]["device.temperature"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["device.temperature"]["value"].ToString());

                Utils.Print(fileName, a["telemetry"]["engine.ignition.status"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["engine.ignition.status"]["value"].ToString());

                Utils.Print(fileName, a["telemetry"]["external.powersource.voltage"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["external.powersource.voltage"]["value"].ToString());

                Utils.Print(fileName, a["telemetry"]["external.powersource.voltage.range.outside.status"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["external.powersource.voltage.range.outside.status"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["geofence.status"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["geofence.status"]["value"].ToString());

                Utils.Print(fileName, a["telemetry"]["gnss.antenna.status"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["gnss.antenna.status"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["gnss.type"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["gnss.type"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["gsm.signal.level"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["gsm.signal.level"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["gsm.sim.status"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["gsm.sim.status"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["hardware.version.enum"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["hardware.version.enum"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["ibutton.connected.status"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["ibutton.connected.status"]["value"].ToString());

                Utils.Print(fileName, a["telemetry"]["ident"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["ident"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["incline.event"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["incline.event"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["internal.battery.voltage.limit.lower.status"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["internal.battery.voltage.limit.lower.status"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["internal.bus.supply.voltage.range.outside.status"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["internal.bus.supply.voltage.range.outside.status"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["movement.status"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["movement.status"]["value"].ToString());

                Utils.Print(fileName, a["telemetry"]["peer"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["peer"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["position"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["position"]["value"]["altitude"].ToString());
                Utils.Print(fileName, a["telemetry"]["position"]["value"]["direction"].ToString());
                Utils.Print(fileName, a["telemetry"]["position"]["value"]["hdop"].ToString());
                Utils.Print(fileName, a["telemetry"]["position"]["value"]["latitude"].ToString());
                Utils.Print(fileName, a["telemetry"]["position"]["value"]["longitude"].ToString());
                Utils.Print(fileName, a["telemetry"]["position"]["value"]["satellites"].ToString());
                Utils.Print(fileName, a["telemetry"]["position"]["value"]["speed"].ToString());
                Utils.Print(fileName, a["telemetry"]["position"]["value"]["valid"].ToString());
                Utils.Print(fileName, a["telemetry"]["position.altitude"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["position.altitude"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["position.direction"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["position.direction"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["position.hdop"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["position.hdop"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["position.latitude"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["position.latitude"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["position.longitude"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["position.longitude"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["position.satellites"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["position.satellites"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["position.speed"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["position.speed"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["position.valid"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["position.valid"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["record.seqnum"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["record.seqnum"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["rs232.sensor.value.0"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["rs232.sensor.value.0"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["rs485.fuel.sensor.level.0"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["rs485.fuel.sensor.level.0"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["rs485.fuel.sensor.level.1"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["rs485.fuel.sensor.level.1"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["rs485.fuel.sensor.level.2"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["rs485.fuel.sensor.level.2"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["server.timestamp"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["server.timestamp"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["shock.event"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["shock.event"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["software.version.enum"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["software.version.enum"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["timestamp"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["timestamp"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["turn.acceleration"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["turn.acceleration"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["x.acceleration"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["x.acceleration"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["y.acceleration"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["y.acceleration"]["value"].ToString());
                Utils.Print(fileName, a["telemetry"]["z.acceleration"]["ts"].ToString());
                Utils.Print(fileName, a["telemetry"]["z.acceleration"]["value"].ToString());
            }
        }
        /**/
        /*
         * "id\",
         * "telemetry":{\"absolute.acceleration\":{\"ts\":1564740692,\"value\":0},\"ain.1\":{\"ts\":1564740692,\"value\":0},\"ain.2\":{\"ts\":1564740692,\"value\":0},\"alarm.event\":{\"ts\":1564740692,\"value\":false},\"alarm.mode.status\":{\"ts\":1564740692,\"value\":false},\"battery.voltage\":{\"ts\":1564740692,\"value\":3.512},\"brake.acceleration\":{\"ts\":1564740692,\"value\":0},\"bump.acceleration\":{\"ts\":1564740692,\"value\":0.22},\"device.temperature\":{\"ts\":1564740692,\"value\":38},\"engine.ignition.status\":{\"ts\":1564740692,\"value\":true},\"external.powersource.voltage\":{\"ts\":1564740692,\"value\":0},\"external.powersource.voltage.range.outside.status\":{\"ts\":1564740692,\"value\":true},\"geofence.status\":{\"ts\":1564740692,\"value\":false},\"gnss.antenna.status\":{\"ts\":1564740692,\"value\":true},\"gnss.type\":{\"ts\":1564740692,\"value\":\"glonass\"},\"gsm.signal.level\":{\"ts\":1564740692,\"value\":33},\"gsm.sim.status\":{\"ts\":1564740692,\"value\":false},\"hardware.version.enum\":{\"ts\":1564730883.623066,\"value\":17},\"ibutton.connected.status\":{\"ts\":1564740692,\"value\":false},\"ident\":{\"ts\":1564740692,\"value\":\"865905020671073\"},\"incline.event\":{\"ts\":1564740692,\"value\":false},\"internal.battery.voltage.limit.lower.status\":{\"ts\":1564740692,\"value\":true},\"internal.bus.supply.voltage.range.outside.status\":{\"ts\":1564740692,\"value\":true},\"movement.status\":{\"ts\":1564740692,\"value\":true},\"peer\":{\"ts\":1564740692,\"value\":\"85.140.0.165:43418\"},\"position\":{\"ts\":1564740692,\"value\":{\"altitude\":32,\"direction\":296.6,\"hdop\":0.6000000000000001,\"latitude\":51.446176,\"longitude\":46.107536,\"satellites\":15,\"speed\":0,\"valid\":true}},\"position.altitude\":{\"ts\":1564740692,\"value\":32},\"position.direction\":{\"ts\":1564740692,\"value\":296.6},\"position.hdop\":{\"ts\":1564740692,\"value\":0.6000000000000001},\"position.latitude\":{\"ts\":1564740692,\"value\":51.446176},\"position.longitude\":{\"ts\":1564740692,\"value\":46.107536},\"position.satellites\":{\"ts\":1564740692,\"value\":15},\"position.speed\":{\"ts\":1564740692,\"value\":0},\"position.valid\":{\"ts\":1564740692,\"value\":true},\"record.seqnum\":{\"ts\":1564740692,\"value\":3754},\"rs232.sensor.value.0\":{\"ts\":1564740692,\"value\":0},\"rs485.fuel.sensor.level.0\":{\"ts\":1564740692,\"value\":0},\"rs485.fuel.sensor.level.1\":{\"ts\":1564740692,\"value\":0},\"rs485.fuel.sensor.level.2\":{\"ts\":1564740692,\"value\":0},\"server.timestamp\":{\"ts\":1564740692,\"value\":1564740694.42362},\"shock.event\":{\"ts\":1564740692,\"value\":false},\"software.version.enum\":{\"ts\":1564730883.623066,\"value\":231},\"timestamp\":{\"ts\":1564740692,\"value\":1564740692},\"turn.acceleration\":{\"ts\":1564740692,\"value\":0},\"x.acceleration\":{\"ts\":1564740692,\"value\":-0.9301075268817204},\"y.acceleration\":{\"ts\":1564740692,\"value\":-0.1989247311827957},\"z.acceleration\":{\"ts\":1564740692,\"value\":0.3763440860215054}}}]}*/
        /**/

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
                Utils.Print(log, new string('-', 80));
                Utils.Print(log, request);
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
                            Utils.Print(log, "Что-то пошло не так!");
                            break;

                        default:
                            Utils.Print(log, "Что-то однозначно пошло не так! " + ex.Message);
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
                    Utils.Print(log, streamReader.ReadToEnd());
                }
            }
            catch(WebException ex)
            {
                Utils.Print(log, ex.Message);
                //Console.WriteLine(ex.Response.Headers.ToString());
                using (var streamReader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    Utils.Print(log, streamReader.ReadToEnd());
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
                    Utils.Print(log, streamReader.ReadToEnd());
                }
            }
            catch (WebException ex)
            {
                Utils.Print(log, ex.Message);
                //Console.WriteLine(ex.Response.Headers.ToString());
                using (var streamReader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    Utils.Print(log, streamReader.ReadToEnd());
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
                    Utils.Print(log, streamReader.ReadToEnd());
                }
            }
            catch (WebException ex)
            {
                Utils.Print(log, ex.Message);
                //Console.WriteLine(ex.Response.Headers.ToString());
                using (var streamReader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    Utils.Print(log, streamReader.ReadToEnd());
                }
            }
        }

        static void client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            Console.WriteLine("MessageId = " + e.MessageId + " Published = " + e.IsPublished);
        }

        static void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Utils.Print("received.txt", "Subscribed for id = " + e.MessageId);
        }
        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            settings(e.Topic, "{\"result\":[" + Encoding.UTF8.GetString(e.Message) + "]}");
            //zu.Print("received.txt", "Received = " + Encoding.UTF8.GetString(e.Message) + " on topic " + e.Topic);
            //zu.Print("Received.txt", Encoding.UTF8.GetString(e.Message) + " " + e.Topic);
            ///// работает test3("{\"result\":["+Encoding.UTF8.GetString(e.Message)+"]}");
        }
        static void client_MqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
        {
            Console.WriteLine("Unsubscribed for id = " + e.MessageId);
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
            if (value == "True")
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

            string device_id = headers[4];
            string field = headers[6];
            string value = message;
            string sql_insert = "";
            string sql_update = "";

            string tableName = "Settings";
            int recid = 0;

            SqlConnection sqlConnection = new SqlConnection(conStr);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("Select recid From " + tableName + " Where [device_uid] = " + device_id, sqlConnection);
            int rows = 0;

            Object o = sqlCommand.ExecuteScalar();

            if (o != null)
                recid = Convert.ToInt32(o.ToString());

            foreach (var a in dobj["result"])
            {
                switch (field)
                {
                    case "sin0":
                    case "sin1":
                    case "sin2":
                    case "sin3":
                    case "sin4":
                    case "sin5":
                    case "sin6":
                    case "sin7":
                        sql_insert = string.Format("Insert Into {7} ({0}delay, {0}msg, {0}photo, {0}ring, {0}sms, {0}type, Device_UID) Values ({1}, {2}, {3}, {4}, {5}, {6}, {8})"
                            , field+"_", a["delay"].ToString(), "'"+a["msg"].ToString()+"'", a["photo"].ToString() == "True"? "1" : "0", a["ring"].ToString() == "True"?"1":"0", a["sms"].ToString() == "True"?"1":"0", a["type"].ToString(), tableName, device_id);
                        sql_update = string.Format("Update Settings Set {0}delay={1}, {0}msg={2}, {0}photo={3}, {0}ring={4}, {0}sms={5}, {0}type={6} Where recid={7}"
                            , field + "_", a["delay"].ToString(), "'" + a["msg"].ToString() + "'", a["photo"].ToString() == "True" ? "1" : "0", a["ring"].ToString() == "True" ? "1" : "0", a["sms"].ToString() == "True" ? "1" : "0", a["type"].ToString(), recid);
                        break;
                    case "sout0":
                    case "sout1":
                    case "sout2":
                    case "sout3":
                        sql_insert = string.Format(@"Insert Into Settings ({0}amode_count, {0}amode_delay, {0}amode_dur, {0}amode_type,
                         {0}dmode_count, {0}dmode_dur, {0}dmode_type,
                         {0}emode_count, {0}emode_dur, {0}emode_type, device_uid) Values ("+
                         a["amode"]["count"].ToString()+"," +a["amode"]["delay"].ToString() + "," + a["amode"]["dur"].ToString() + "," + a["amode"]["type"].ToString() + "," +
                         a["dmode"]["count"].ToString() + "," + a["dmode"]["dur"].ToString() + "," + a["dmode"]["type"].ToString() + "," +
                         a["emode"]["count"].ToString() + "," + a["emode"]["dur"].ToString() + "," + a["emode"]["type"].ToString()+", {1})", field + "_", device_id);
                        sql_update = string.Format(@"Update Settings Set {0}amode_count={1}, {0}amode_delay={2}, {0}amode_dur={3}, {0}amode_type={4},
                         {0}dmode_count={5}, {0}dmode_dur={6}, {0}dmode_type={7}, {0}emode_count={8}, {0}emode_dur={9}, {0}emode_type={10} Where recid={11}", field + "_",
                         a["amode"]["count"].ToString(), a["amode"]["delay"].ToString(), a["amode"]["dur"].ToString(), a["amode"]["type"].ToString(),
                         a["dmode"]["count"].ToString(), a["dmode"]["dur"].ToString(), a["dmode"]["type"].ToString(),
                         a["emode"]["count"].ToString(), a["emode"]["dur"].ToString(), a["emode"]["type"].ToString(), recid);
                        break;
                    case "canbus":// какая-то
                        /*string b = a["mode"]["type"].ToString();
                        string c = a["mode"]["timeout"].ToString();
                        string d = a["mode"]["do_not_clean_after_timeout"].ToString();
                        string e = a["mode"]["baudrate"].ToString();*/
                        sql_insert = string.Format("Insert Into Settings ({0}mode_baudrate, {0}mode_do_not_clean_after_timeout, {0}mode_timeout, {0}mode_type, Device_uid) Values (" +
                            a["mode"]["baudrate"].ToString() + "," + a["mode"]["do_not_clean_after_timeout"].ToString() == "True" ? "1":"0" + "," + a["mode"]["timeout"].ToString() + "," + a["mode"]["type"].ToString() + ", {1}",
                            field+"_", device_id);
                        sql_update = string.Format("Update Settings Set {0}mode_baudrate={1}, {0}mode_do_not_clean_after_timeout={2}, {0}mode_timeout={3}, {0}mode_type={4} Where recid={5}", field+"_", a["mode"]["baudrate"].ToString(), a["mode"]["do_not_clean_after_timeout"].ToString() == "True" ? "1" : "0", a["mode"]["timeout"].ToString(), a["mode"]["type"].ToString(), recid);
                        break;
                    case "phones":
                        sql_insert = string.Format("Insert Into Settings ({0}number1, {0}number2, {0}number3, {0}number4, device_uid) Values ('"+a["number1"].ToString()+"','" + a["number2"].ToString() + "','" + a["number3"].ToString() + "','" + a["number4"].ToString() + "', {1})", field+"_", device_id);
                        sql_update = string.Format("Update Settings Set {0}number1='{1}', {0}number2='{2}', {0}number3='{3}', {0}number4='{4}' Where recid={5}", field+"_", a["number1"].ToString(), a["number2"].ToString(), a["number3"].ToString(), a["number4"].ToString(), recid);
                        break;
                    case "wr_period":
                        sql_insert = string.Format("Insert Into Settings ({0}move_int, {0}stop_int, device_uid) Values ("+a["move_int"].ToString()+","+a["stop_int"].ToString() +", {1})", field+"_", device_id);
                        sql_update = string.Format("Update Settings Set {0}move_int={1}, {0}stop_int={2} Where recid={3}", field+"_", a["move_int"].ToString(), a["stop_int"].ToString(), recid);
                        break;
                    case "network_sim0":
                    case "network_sim1":
                        sql_insert = string.Format("Insert Into Settings ({0}apn, {0}password, {0}username, device_uid) Values ('" + a["apn"].ToString() + "','" + a["password"].ToString() + "','" + a["username"].ToString() + "', {1})", field + "_", device_id);
                        sql_update = string.Format("Update Settings Set {0}apn='{1}', {0}password='{2}', {0}username='{3}' Where recid={4}", field + "_", a["apn"].ToString(), a["password"].ToString(), a["username"].ToString(), recid);
                        break;
                    case "sign":
                        sql_insert = string.Format("Insert Into Settings ({0}datimeout, {0}gwtime, {0}useib, device_uid) Values (" + a["datimeout"].ToString() + "," + a["gwtime"].ToString() + "," + a["useib"].ToString() + ", {1})", field + "_", device_id);
                        sql_update = string.Format("Update Settings Set {0}datimeout={1}, {0}gwtime={2}, {0}useib={3} Where recid={4}", field + "_", a["datimeout"].ToString(), a["gwtime"].ToString(), a["useib"].ToString(), recid);
                        break;
                    case "output0":
                    case "output1":
                    case "output2":
                    case "output3":
                        sql_insert = string.Format("Insert Into Settings ({0}out_val, device_uid) Values (" + a["out_val"].ToString() == "True" ? "1":"0" + ", {1})", field + "_", device_id);
                        sql_update = string.Format("Update Settings Set {0}out_val={1} Where recid={2}", field + "_", a["out_val"].ToString() == "True" ? "1": "0", recid);
                        break;
                    case "backend_server2":
                        sql_insert = string.Format("Insert Into Settings ({0}host, {0}port, device_uid) Values ('" + a["host"].ToString()+"', "+a["port"].ToString() + ", {1})", field + "_", device_id);
                        sql_update = string.Format("Update Settings Set {0}host='{1}', {0}port={2} Where recid={3}", field + "_", a["host"].ToString(), a["port"].ToString(), recid);
                        break;
                    case "tracking":
                        sql_insert = string.Format("Insert Into Settings ({0}angle, {0}angle_speed, {0}delta_speed, {0}dist, {0}speed, device_uid) Values (" + a["angle"].ToString() + ", " + a["angle_speed"].ToString() + ", " + a["delta_speed"].ToString() + ", " + a["dist"].ToString() + ", " + a["speed"].ToString() + ", {1})", field + "_", device_id);
                        sql_update = string.Format("Update Settings Set {0}angle={1}, {0}angle_speed={2}, {0}delta_speed={3}, {0}dist={4}, {0}speed={5} Where recid={6}", field + "_", a["angle"].ToString(), a["angle_speed"].ToString(), a["delta_speed"].ToString(), a["dist"].ToString(), a["speed"].ToString(), recid);
                        break;
                    case "sgps":
                        sql_insert = string.Format("Insert Into Settings ({0}radius, {0}ring, {0}sms, {0}speed, {0}timeout, {0}type, device_uid) Values (" + a["radius"].ToString() + ", " + a["ring"].ToString()=="True"?"1":"0" + ", " + a["sms"].ToString()=="True"?"1":"0" + ", " + a["speed"].ToString() + ", " + a["timeout"].ToString() + ", " + a["type"].ToString() + ", {1})", field + "_", device_id);
                        sql_update = string.Format("Update Settings Set {0}radius={1}, {0}ring={2}, {0}sms={3}, {0}speed={5}, {0}timeout={6}, {0}type={7} Where recid={7}", field + "_", a["radius"].ToString(), a["ring"].ToString()=="True"?"1":"0", a["sms"].ToString()=="True"?"1":"0", a["speed"].ToString(), a["timeout"].ToString(), a["type"].ToString(), recid);
                        break;
                    case "guard":
                        sql_insert = string.Format("Insert Into Settings ({0}enable, device_uid) Values (" + a["enable"].ToString() == "True" ? "1" : "0" + ", {1})", field + "_", device_id);
                        sql_update = string.Format("Update Settings Set {0}enable={1} Where recid={2}", field + "_", a["enable"].ToString() == "True" ? "1" : "0", recid);
                        break;
                    case "shock":
                        sql_insert = string.Format("Insert Into Settings ({0}angle, {0}sens, {0}timeout, {0}type, device_uid) Values (" + 
                            a["angle"].ToString() + ", " + a["sens"].ToString()+ ", " + a["timeout"].ToString() + ", " + a["type"].ToString() + ", {1})", field + "_", device_id);
                        sql_update = string.Format("Update Settings Set {0}angle={1}, {0}sens={2}, {0}timeout={3}, {0}type={4} Where recid={5}", 
                            field + "_", a["angle"].ToString(), a["sens"].ToString(), a["timeout"].ToString(), a["type"].ToString(), recid);
                        break;
                    case "easylogic_get":
                        sql_insert = string.Format("Insert Into Settings ({0}scripts, device_uid) Values ('" + a["scripts"].ToString() + "', {1})", field + "_", device_id);
                        sql_update = string.Format("Update Settings Set {0}scripts='{1}' Where recid={2}", field + "_", a["scripts"].ToString(), recid);
                        break;
                    case "sacc":
                        sql_insert = string.Format("Insert Into Settings ({0}msg, {0}photo, {0}ring, {0}sms, {0}type, device_uid) Values ('" +
                            a["msg"].ToString() + "', " + a["photo"].ToString()=="True"?"1":"0" + ", " + a["ring"].ToString()=="True"?"1":"0" + ", " + a["sms"].ToString()=="True"?"1":"0" + ", " + a["type"].ToString() + ", {1})", field + "_", device_id);
                        sql_update = string.Format("Update Settings Set {0}msg='{1}', {0}photo={2}, {0}ring={3}, {0}sms={4}, {0}type={5} Where recid={6}",
                            field + "_", a["msg"].ToString(), a["photo"].ToString()=="True"?"1":"0", a["ring"].ToString()=="True"?"1":"0", a["sms"].ToString()=="True"?"1":"0", a["type"].ToString(), recid);
                        break;
                    case "headpack1":
                        sql_insert = string.Format("Insert Into Settings ({0}tag_01, {0}tag_02, {0}tag_03, device_uid) Values (" +
                            a["tag_01"].ToString() == "True" ? "1" : "0" + ", " + a["tag_02"].ToString() == "True" ? "1" : "0" + ", " + a["tag_03"].ToString() == "True" ? "1" : "0" + ", {1})", field + "_", device_id);
                        sql_update = string.Format("Update Settings Set {0}tag_01={1}, {0}tag_02={2}, {0}tag_03={3} Where recid={4}",
                            field + "_", a["tag_01"].ToString() == "True" ? "1" : "0", a["tag_02"].ToString() == "True" ? "1" : "0", a["tag_03"].ToString() == "True" ? "1" : "0", recid);
                        break;
                    case "mainpack1":
                        //dbFields = ",{0}tag_01,{0}tag_02,{0}tag_03,{0}tag_04,{0}tag_05,{0}tag_06,{0}tag_07,{0}tag_08,{0}tag_09,{0}tag_10,{0}tag_11,{0}tag_12,{0}tag_13,{0}tag_14,{0}tag_15";
                        sql_insert = string.Format("Insert Into Settings ({0}tag_01, {0}tag_02, {0}tag_03,{0}tag_04,{0}tag_05,{0}tag_06,{0}tag_07,{0}tag_08,{0}tag_09,{0}tag_10,{0}tag_11,{0}tag_12,{0}tag_13,{0}tag_14,{0}tag_15, device_uid) Values (" + 
                            a["tag_01"].ToString()=="True"?"1":"0"+","+a["tag_02"].ToString() == "True" ? "1" : "0"+ "," + a["tag_03"].ToString() == "True" ? "1" : "0" +
                            a["tag_04"].ToString() == "True" ? "1" : "0" + "," + a["tag_05"].ToString() == "True" ? "1" : "0" + "," + a["tag_06"].ToString() == "True" ? "1" : "0" +
                            a["tag_07"].ToString() == "True" ? "1" : "0" + "," + a["tag_08"].ToString() == "True" ? "1" : "0" + "," + a["tag_09"].ToString() == "True" ? "1" : "0" +
                            a["tag_10"].ToString() == "True" ? "1" : "0" + "," + a["tag_11"].ToString() == "True" ? "1" : "0" + "," + a["tag_12"].ToString() == "True" ? "1" : "0" +
                            a["tag_13"].ToString() == "True" ? "1" : "0" + "," + a["tag_14"].ToString() == "True" ? "1" : "0" + "," + a["tag_15"].ToString() == "True" ? "1" : "0" +
                            ", {1})", field+"_", device_id);
                        sql_update = string.Format("Update Settings Set {0}tag_01={1}, {0}tag_02={2}, {0}tag_03={3},{0}tag_04={4}, {0}tag_05={5}, {0}tag_06={6},{0}tag_07={7}, {0}tag_08={8}, {0}tag_09={9},{0}tag_10={10}, {0}tag_11={11}, {0}tag_12={12},{0}tag_13={13}, {0}tag_14={14}, {0}tag_15={15} Where recid={16}", 
                            field+"_", 
                            a["tag_01"].ToString()== "True"?"1":"0", a["tag_02"].ToString() == "True" ? "1" : "0", a["tag_03"].ToString() == "True" ? "1" : "0",
                            a["tag_04"].ToString() == "True" ? "1" : "0", a["tag_05"].ToString() == "True" ? "1" : "0", a["tag_06"].ToString() == "True" ? "1" : "0",
                            a["tag_07"].ToString() == "True" ? "1" : "0", a["tag_08"].ToString() == "True" ? "1" : "0", a["tag_09"].ToString() == "True" ? "1" : "0",
                            a["tag_10"].ToString() == "True" ? "1" : "0", a["tag_11"].ToString() == "True" ? "1" : "0", a["tag_12"].ToString() == "True" ? "1" : "0",
                            a["tag_13"].ToString() == "True" ? "1" : "0", a["tag_14"].ToString() == "True" ? "1" : "0", a["tag_15"].ToString() == "True" ? "1" : "0",
                            recid);
                        break;
                    default: /*Device_UID*/break;
                }
                //zu.Print(fileName, a["telemetry"]["absolute.acceleration"]["ts"].ToString());

                Utils.Print("settings.txt", "topic: " + topic + " message " + message);
                Utils.Print("settings.txt", "------------------------------------------------------");
            }
            
            /*sqlCommand.CommandText = "SELECT t.name FROM sys.columns AS c JOIN sys.types AS t ON c.user_type_id = t.user_type_id WHERE c.object_id = OBJECT_ID('Settings') and c.name = '" + field + "'";
            Object t = sqlCommand.ExecuteScalar();*/
            //string s = "";
            //if (/*t.ToString()*/getType(tableName, /*field*/"sin0_msg") == "nvarchar")
            //{
            //    s = "'";
            //}
            //if (value == "True")
            //    value = "1";
            //if (value == "false")
            //    value = "0";

            // Получить тип поля его названию и названию таблицы
            string getType(string tableNameI, string fieldName)
            {
                sqlCommand.CommandText = "SELECT t.name FROM sys.columns AS c JOIN sys.types AS t ON c.user_type_id = t.user_type_id WHERE c.object_id = OBJECT_ID('"+tableNameI+"') and c.name = '" + fieldName + "'";
                Object t = sqlCommand.ExecuteScalar();
                return t.ToString();
            }

            if (recid == 0)
            {
                sqlCommand.CommandText = sql_insert;
            }
            else
            {
                sqlCommand.CommandText = sql_update;
            }

            rows = sqlCommand.ExecuteNonQuery();

            sqlConnection.Close();

            //Console.CancelKeyPress

            //client.Unsubscribe(new string[] { "flespi/state/gw/devices/361202/settings/guard" });
        }
        static void ClientReceiveTest1()
        {
            string BrokerAddress = "mqtt.flespi.io";
            client = new MqttClient(BrokerAddress);

            // register a callback-function (we have to implement, see below) which is called by the library when a message was received
            client.MqttMsgSubscribed += client_MqttMsgSubscribed;
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            client.MqttMsgUnsubscribed += client_MqttMsgUnsubscribed;

            // use a unique id as client id, each time we start the application
            clientId = Guid.NewGuid().ToString();

            client.Connect(clientId, "FlespiToken Vo6wSNjDEM19qzUdq9qbwZugZPmPl3N4hHq0lAtPalMqIwuYuKZQxiUnX7060B17", "");

            Console.WriteLine(client.IsConnected ? "Connected" : "... там");

            //*** работает ***
            //ushort code = client.Subscribe(new string[] { "flespi/message/gw/devices/361202" }, new byte[] { 2 });

            //ushort code2 = client.Subscribe(new string[] { "flespi/state/gw/devices/361201" }, new byte[] { 2 });
            //ushort code = client.Subscribe(new string[] { "flespi/state/gw/devices/361202/settings/canbus" }, new byte[] { 2 });
            ushort code = client.Subscribe(new string[] { "flespi/state/gw/devices/361201/settings/canbus" }, new byte[] { 2 });

            //ushort code3 = client.Subscribe(new string[] { "flespi/state/gw/devices/361201/settings/network_sim1" }, new byte[] { 2 });

            //ushort code = client.Subscribe(new string[] { "flespi/state/gw/devices/361202/telemetry/+" }, new byte[] { 2 });
        }
    }
}
