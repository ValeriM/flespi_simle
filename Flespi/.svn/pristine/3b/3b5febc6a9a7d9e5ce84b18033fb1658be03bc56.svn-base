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

            Print(log, "Start " + DateTime.Now.ToString());

            ClientReceiveTest1();
        }
        // Работа с топиком flespi/message/gw/devices/{device_id}
        static void test_zero(string json)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            //string json = ReadFile("json3i.txt");
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
                if (rows == rmax) break;// это только для отладки
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

        /*
            Если нужна авторизация
            WebClient webClient = new WebClient();
            webClient.Credentials = new NetworkCredential(username, password);
            Stream stream = webClient.OpenRead(URI);
        */
        // Создание клиента
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
        // Запрос клиента POST
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

        // Запрос клиента DELETE
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
        // Запрос клиента PUT
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
        // Вывод лога в файл и на консоль (опционально)
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
        // Читени из файла
        static string ReadFile(string fileName)
        {
            string ret;
            StreamReader reader = new StreamReader(fileName);
            ret = reader.ReadToEnd();
            reader.Close();
            return ret;
        }
        // Если будем сами публиковать что-то
        static void client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            Console.WriteLine("MessageId = " + e.MessageId + " Published = " + e.IsPublished);
        }
        // Подписка состоялась
        static void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Print("received.txt", "Subscribed for id = " + e.MessageId);
        }
        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // Получили сообщение. Отправим на парсинг и запись в таблицу
            test_zero("{\"result\":["+Encoding.UTF8.GetString(e.Message)+"]}");
        }
        static void ClientReceiveTest1()
        {
            // Создаем клиента
            string BrokerAddress = "mqtt.flespi.io";
            client = new MqttClient(BrokerAddress);

            // Реакции на подписание и получение данных
            client.MqttMsgSubscribed += client_MqttMsgSubscribed;
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            // Уникальный id клиента
            clientId = Guid.NewGuid().ToString();

            client.Connect(clientId, "FlespiToken Vo6wSNjDEM19qzUdq9qbwZugZPmPl3N4hHq0lAtPalMqIwuYuKZQxiUnX7060B17", "");

            Console.WriteLine(client.IsConnected ? "Connected" : "... там");

            //*** работает ***
            // Подписываемся на сообщения
            ushort code = client.Subscribe(new string[] { "flespi/message/gw/devices/361202" }, new byte[] { 2 });

            //ushort code2 = client.Subscribe(new string[] { "flespi/state/gw/devices/361201" }, new byte[] { 2 });
            //ushort code = client.Subscribe(new string[] { "flespi/state/gw/devices/361202/settings/+" }, new byte[] { 2 });

            //ushort code = client.Subscribe(new string[] { "flespi/state/gw/devices/361202/telemetry/+" }, new byte[] { 2 });
        }
    }
}
