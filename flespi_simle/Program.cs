using System;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
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
        //static readonly string token = "Authorization: FlespiToken Vo6wSNjDEM19qzUdq9qbwZugZPmPl3N4hHq0lAtPalMqIwuYuKZQxiUnX7060B17";
        static readonly string nudeToken = "FlespiToken Vo6wSNjDEM19qzUdq9qbwZugZPmPl3N4hHq0lAtPalMqIwuYuKZQxiUnX7060B17";
        //token = "Authorization: FlespiToken UIy8bexWRWLVX3H3yJFCkycRTNI3xRognMeoOBbvlKf8EK20kvrsRraz4GsqnGwB";
        static readonly string log = "flespi_simle.txt";
        static MqttClient client;
        static string clientId;
        static string conStr = Properties.Settings.Default.Connection;

        static void Main(string[] args)
        {
            Utils.Print(log, "Строка подключения " + conStr + ". Находится в flespi_simle.exe.config");
            Utils.Print(log, "Журнал пишется в " + log);
            Utils.Print(log, "Запуск " + DateTime.Now.ToString());

            //if (args.Length < 1)
            //{
            //    Utils.Print(log, "Не... Так не пойдет. Укажите параметр (topic), Например:");
            //    Utils.Print(log, "flespi/state/gw/devices/361201/settings/headpack1");
            //    Utils.Print(log, "или там");
            //    Utils.Print(log, "flespi/message/gw/devices/361202");
            //    _ = Console.ReadKey();
            //    return;
            //}

            foreach(var arg in args)
                Utils.Print(log, arg);

            //ClientReceiveTest1(args[0]);
            ClientReceiveTest1("flespi/message/gw/devices/+");
            //ClientReceiveTest1("flespi/state/gw/devices/361201/settings/+");
        }

        /*
        */
        static void MessagesLong(string json)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);

            SqlConnection sqlConnection = new SqlConnection(conStr);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("", sqlConnection);
            int rows = 0;
            

            foreach (var a in dobj["result"])
            {
                foreach (var v in a)
                {
                    Utils.Print(log, a["device.id"].ToString() + " " + v.Key.ToString().Replace('.', '_') + " " + v.Value.ToString());
                    string sql = "Insert Into Fields (device_uid, fieldname, fieldvalue) Values ("+a["device.id"].ToString() +", '" + v.Key.ToString().Replace('.', '_') + "', '" + v.Value.ToString() + "')";
                    Utils.Print(log, sql);
                    sqlCommand.CommandText = sql;
                    rows += sqlCommand.ExecuteNonQuery();
                }
                
                //a["fieldName"].ToString();
                
            }
           
            sqlConnection.Close();
        }
        static void settings(string topic, string json)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);

            string[] headers = topic.Split('/');

            string device_id = headers[4];
            string field = headers[6];

            SqlConnection sqlConnection = new SqlConnection(conStr);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("", sqlConnection);
            int rows = 0;


            foreach (var a in dobj["result"])
            {
                foreach (var v in a)
                {
                    Type tp = v.Value.GetType();
                    int _case = 0;
                    if (tp == typeof(Int32) || tp == typeof(String) || tp == typeof(Boolean))
                        _case = 1;
                    switch(_case)
                    {
                        case 1:
                            {
                                Utils.Print(log, device_id + " " + v.Key.ToString().Replace('.', '_') + " " + v.Value.ToString());
                                string sql = "Insert Into SettingsFields (device_uid, fieldname, fieldvalue) Values (" + device_id + ", '" + field + "_" + v.Key.ToString().Replace('.', '_') + "', '" + v.Value.ToString() + "')";
                                Utils.Print(log, sql);
                                sqlCommand.CommandText = sql;
                                rows += sqlCommand.ExecuteNonQuery();
                            }
                            break;
                        default:
                            foreach (var p in v.Value)
                            {
                                string sql = "Insert Into SettingsFields (device_uid, fieldname, fieldvalue) Values (" + device_id + ", '" + field + "_" + v.Key.ToString() + "_" + p.Key.ToString() + "', '" + p.Value.ToString() + "')";
                                Utils.Print(log, sql);
                                sqlCommand.CommandText = sql;
                                rows += sqlCommand.ExecuteNonQuery();
                            }
                            break;
                    }
                        
                }

                //a["fieldName"].ToString();

            }

            sqlConnection.Close();
        }
        /*
        */
        static void Messages(string json)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            Dictionary<string, SqlDbType> fields = new Dictionary<string, SqlDbType>
            {
               {"absolute_acceleration", SqlDbType.Real},//0
               {"alarm_event", SqlDbType.Bit },//1
               {"alarm_mode_status", SqlDbType.Bit},//2
               {"brake_acceleration", SqlDbType.Real},//3
               { "bump_acceleration",SqlDbType.Real },//4
               { "channel_id", SqlDbType.Int},//5
               { "device_uid", SqlDbType.Int},//6
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
               { "timestamp",SqlDbType.Real },//36
               { "turn_acceleration",SqlDbType.Real },//37
               { "x_acceleration",SqlDbType.Real },//38
               { "y_acceleration",SqlDbType.Real },//39
               { "z_acceleration",SqlDbType.Real },//40
               { "ain_1", SqlDbType.Int}, //41
               { "ain_2", SqlDbType.Int},//42
               { "battery_voltage", SqlDbType.Real},//43
               { "device_temperature", SqlDbType.Int},//44
               { "hardware_version_enum", SqlDbType.Int},//45
               { "software_version_enum", SqlDbType.Int},//46
               { "rs232_sensor_value_0", SqlDbType.Int},//47
               { "rs485_fuel_sensor_level_0", SqlDbType.Int},//48
               { "rs485_fuel_sensor_level_1", SqlDbType.Int},//49
               { "rs485_fuel_sensor_level_2", SqlDbType.Int},//50
               { "rs485_fuel_sensor_temperature_0", SqlDbType.Int},//51
               { "rs485_fuel_sensor_temperature_1", SqlDbType.Int},//52
               { "rs485_fuel_sensor_temperature_2", SqlDbType.Int}//53
            };

            SqlConnection sqlConnection = new SqlConnection(conStr);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("", sqlConnection);
            int rows = 0;
            string sql = "Insert Into Messages (";
            string sql2 = "(";

            foreach(var field in fields)
            {
                sql += "[" + field.Key + "],";
                sql2 += "@p" + rows.ToString() + ",";
                rows++;
            }
            sql = sql.Remove(sql.Length-1);
            sql2 = sql2.Remove(sql2.Length - 1);
            sql += ") Values " + sql2 + ")";

            sqlCommand.CommandText = sql;

            rows = 0;

            bool error = false;

            foreach (var a in dobj["result"])
            {
                foreach (var field in fields)
                {
                    string fieldName = field.Key.Replace('_', '.');
                    if (field.Key == "device_uid")
                        fieldName = "device.id";
                    sqlCommand.Parameters.Add("@p"+rows.ToString(), field.Value);
                    try
                    {
                        string tmp = "";
                        if (a.ContainsKey(fieldName))
                        {
                            tmp = a[fieldName].ToString();
                            if (field.Value == SqlDbType.Bit)
                                tmp = Utils.BO10(tmp);
                        }
                        else
                        {
                            if (field.Value == SqlDbType.Bit || field.Value == SqlDbType.Int)
                                tmp = "0";
                            if (field.Value == SqlDbType.Real)
                                tmp = "0,0";
                            Utils.Print(log, "Warning: Нет такого ключа = " + fieldName);
                        }
                        if (field.Value == SqlDbType.Bit || field.Value == SqlDbType.Int)
                            sqlCommand.Parameters["@p" + rows.ToString()].Value = Convert.ToInt32(tmp);
                        else
                            if (field.Value == SqlDbType.Real)
                                sqlCommand.Parameters["@p" + rows.ToString()].Value = Convert.ToDouble(tmp);
                            else
                                sqlCommand.Parameters["@p" + rows.ToString()].Value = tmp;
                        Utils.Print(log, "@p"+rows.ToString() + "=" + field.Key + " = " + tmp);
                    }
                    catch(Exception ex)
                    {
                        Utils.Print(log, "*** ОШИБКА ***" + ex.Message);
                        error = true;
                    }
                    if (error) break;
                    rows++;
                }
                if (error) break;
            }
            if (!error)
            {
                //try
                {
                    rows += sqlCommand.ExecuteNonQuery();
                }
                /*catch(Exception ex)
                {
                    Utils.Print(log, ex.Message);
                }*/
            }
            sqlConnection.Close();
        }

        static void client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            Utils.Print(log, "MessageId = " + e.MessageId + " Published = " + e.IsPublished);
        }

        static void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Utils.Print(log, "Subscribed for id = " + e.MessageId);
        }
        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            int topic = 0;
            if (e.Topic.Contains("settings"))
                topic = 1;
            if (e.Topic.Contains("message"))
                topic = 2;
                
            switch (topic)
            {
                case 1: settings(e.Topic, "{\"result\":[" + Encoding.UTF8.GetString(e.Message) + "]}"); break;
                case 2: Messages("{\"result\":[" + Encoding.UTF8.GetString(e.Message) + "]}"); break;
                default: Utils.Print(log, "Неизвестный topic " + e.Topic);  break;
            }
        }
        static void client_MqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
        {
            Utils.Print(log, "Unsubscribed for id = " + e.MessageId);
        }

        static void settings1(string topic, string message)
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
                            , field+"_", a["delay"].ToString(), "'"+a["msg"].ToString()+"'", BO10(a["photo"]), BO10(a["ring"]), BO10(a["sms"]), a["type"].ToString(), tableName, device_id);
                        sql_update = string.Format("Update Settings Set {0}delay={1}, {0}msg={2}, {0}photo={3}, {0}ring={4}, {0}sms={5}, {0}type={6} Where recid={7}"
                            , field + "_", a["delay"].ToString(), "'" + a["msg"].ToString() + "'", BO10(a["photo"]), BO10(a["ring"]), BO10(a["sms"]), a["type"].ToString(), recid);
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
                    case "canbus":
                        sql_insert = string.Format("Insert Into Settings ({0}mode_baudrate, {0}mode_do_not_clean_after_timeout, {0}mode_timeout, {0}mode_type, Device_uid) Values (" +
                            a["mode"]["baudrate"].ToString() + "," + BO10(a["mode"]["do_not_clean_after_timeout"]) + "," + a["mode"]["timeout"].ToString() + "," + a["mode"]["type"].ToString() + ", {1}",
                            field+"_", device_id);
                        sql_update = string.Format("Update Settings Set {0}mode_baudrate={1}, {0}mode_do_not_clean_after_timeout={2}, {0}mode_timeout={3}, {0}mode_type={4} Where recid={5}", field+"_", a["mode"]["baudrate"].ToString(), BO10(a["mode"]["do_not_clean_after_timeout"]), a["mode"]["timeout"].ToString(), a["mode"]["type"].ToString(), recid);
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
                        sql_insert = string.Format("Insert Into Settings ({0}out_val, device_uid) Values (" + BO10(a["out_val"]) + ", {1})", field + "_", device_id);
                        sql_update = string.Format("Update Settings Set {0}out_val={1} Where recid={2}", field + "_", BO10(a["out_val"]), recid);
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
                        sql_insert = string.Format("Insert Into Settings ({0}radius, {0}ring, {0}sms, {0}speed, {0}timeout, {0}type, device_uid) Values (" + a["radius"].ToString() + ", " + BO10(a["ring"]) + ", " + BO10(a["sms"]) + ", " + a["speed"].ToString() + ", " + a["timeout"].ToString() + ", " + a["type"].ToString() + ", {1})", field + "_", device_id);
                        sql_update = string.Format("Update Settings Set {0}radius={1}, {0}ring={2}, {0}sms={3}, {0}speed={5}, {0}timeout={6}, {0}type={7} Where recid={7}", field + "_", a["radius"].ToString(), BO10(a["ring"]), BO10(a["sms"]), a["speed"].ToString(), a["timeout"].ToString(), a["type"].ToString(), recid);
                        break;
                    case "guard":
                        sql_insert = string.Format("Insert Into Settings ({0}enable, device_uid) Values (" + BO10(a["enable"]) + ", {1})", field + "_", device_id);
                        sql_update = string.Format("Update Settings Set {0}enable={1} Where recid={2}", field + "_", BO10(a["enable"]), recid);
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
                            a["msg"].ToString() + "', " + BO10(a["photo"]) + ", " + BO10(a["ring"]) + ", " + BO10(a["sms"]) + ", " + a["type"].ToString() + ", {1})", field + "_", device_id);
                        sql_update = string.Format("Update Settings Set {0}msg='{1}', {0}photo={2}, {0}ring={3}, {0}sms={4}, {0}type={5} Where recid={6}",
                            field + "_", a["msg"].ToString(), BO10(a["photo"]), BO10(a["ring"]), BO10(a["sms"]), a["type"].ToString(), recid);
                        break;
                    case "headpack1":
                        sql_insert = string.Format("Insert Into Settings ({0}tag_01, {0}tag_02, {0}tag_03, device_uid) Values (" +
                            BO10(a["tag_01"]) + ", " + BO10(a["tag_02"]) + ", " + BO10(a["tag_03"]) + ", {1})", field + "_", device_id);
                        sql_insert = string.Format("Insert Into Settings ({0}tag_01, {0}tag_02, {0}tag_03, device_uid) Values (" +
                            BO10(a["tag_01"]) + ", " + BO10(a["tag_02"]) + ", " + BO10(a["tag_03"]) + ", {1})", field + "_", device_id);
                        sql_update = string.Format("Update Settings Set {0}tag_01={1}, {0}tag_02={2}, {0}tag_03={3} Where recid={4}",
                            field + "_", BO10(a["tag_01"]), BO10(a["tag_02"]), BO10(a["tag_03"]), recid);
                        break;
                    case "mainpack1":
                        sql_insert = string.Format("Insert Into Settings ({0}tag_01, {0}tag_02, {0}tag_03,{0}tag_04,{0}tag_05,{0}tag_06,{0}tag_07,{0}tag_08,{0}tag_09,{0}tag_10,{0}tag_11,{0}tag_12,{0}tag_13,{0}tag_14,{0}tag_15, device_uid) Values (" +
                            BO10(a["tag_01"])+","+ BO10(a["tag_02"])+ "," + BO10(a["tag_03"]) +
                            BO10(a["tag_04"]) + "," + BO10(a["tag_05"]) + "," + BO10(a["tag_06"]) +
                            BO10(a["tag_07"]) + "," + BO10(a["tag_08"]) + "," + BO10(a["tag_09"]) +
                            BO10(a["tag_10"]) + "," + BO10(a["tag_11"]) + "," + BO10(a["tag_12"]) +
                            BO10(a["tag_13"]) + "," + BO10(a["tag_14"]) + "," + BO10(a["tag_15"]) +
                            ", {1})", field+"_", device_id);
                        sql_update = string.Format("Update Settings Set {0}tag_01={1}, {0}tag_02={2}, {0}tag_03={3},{0}tag_04={4}, {0}tag_05={5}, {0}tag_06={6},{0}tag_07={7}, {0}tag_08={8}, {0}tag_09={9},{0}tag_10={10}, {0}tag_11={11}, {0}tag_12={12},{0}tag_13={13}, {0}tag_14={14}, {0}tag_15={15} Where recid={16}", 
                            field+"_",
                            BO10(a["tag_01"]), BO10(a["tag_02"]), BO10(a["tag_03"]),
                            BO10(a["tag_04"]), BO10(a["tag_05"]), BO10(a["tag_06"]),
                            BO10(a["tag_07"]), BO10(a["tag_08"]), BO10(a["tag_09"]),
                            BO10(a["tag_10"]), BO10(a["tag_11"]), BO10(a["tag_12"]),
                            BO10(a["tag_13"]), BO10(a["tag_14"]), BO10(a["tag_15"]),
                            recid);
                        break;
                    default: /*Device_UID*/break;
                }
            }
            string BO10(object ob)
            {
                return ob.ToString() == "True" ? "1" : "0";
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
        }
        static void ClientReceiveTest1(string topic = "flespi/message/gw/devices/361202")
        {
            string BrokerAddress = "mqtt.flespi.io";
            client = new MqttClient(BrokerAddress);

            // register a callback-function (we have to implement, see below) which is called by the library when a message was received
            client.MqttMsgSubscribed += client_MqttMsgSubscribed;
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            client.MqttMsgUnsubscribed += client_MqttMsgUnsubscribed;

            // use a unique id as client id, each time we start the application
            clientId = Guid.NewGuid().ToString();

            client.Connect(clientId, nudeToken/*"FlespiToken Vo6wSNjDEM19qzUdq9qbwZugZPmPl3N4hHq0lAtPalMqIwuYuKZQxiUnX7060B17"*/, "");

            Utils.Print(log, client.IsConnected ? "Connected" : "... там");

            ushort code0 = client.Subscribe(new string[] { topic }, new byte[] { 2 });
        }
    }
}
