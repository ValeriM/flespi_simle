using System;
using System.Text;
using System.Web.Script.Serialization;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace flespi_simle
{
    public static class Tests
    {
        // Иммитация 2. Get device logs records
        static void test2()
        {
            //string json = "{\"result\":[{\"event_code\":300,\"event_origin\":\"gw\\/devices\\/361201\",\"event_text\":\"85.140.0.165\\/connected\",\"id\":361201,\"ident\":\"865905020671073\",\"source\":\"85.140.0.165\",\"timestamp\":1564730883.623162,\"transport\":\"tcp\"},{\"close_code\":3,\"duration\":10892,\"event_code\":301,\"event_origin\":\"gw\\/devices\\/361201\",\"event_text\":\"85.140.0.165\\/disconnected\",\"id\":361201,\"ident\":\"865905020671073\",\"msgs\":3022,\"recv\":252588,\"send\":1825,\"source\":\"85.140.0.165\",\"timestamp\":1564741773.556435,\"transport\":\"tcp\"}]}";
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string json = Utils.ReadFile("json2i.txt");
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            string fileName = "json2.txt";

            foreach (var a in dobj["result"])
            {
                Utils.Print(fileName, a["event_code"].ToString());
                Utils.Print(fileName, a["event_origin"].ToString());
                Utils.Print(fileName, a["event_text"].ToString());
                Utils.Print(fileName, a["id"].ToString());
                Utils.Print(fileName, a["ident"].ToString());
                Utils.Print(fileName, a["source"].ToString());
                Utils.Print(fileName, a["timestamp"].ToString());
                Utils.Print(fileName, a["transport"].ToString());
            }
        }
        //Иммитация 6. Get collection of channels matching filter parameters
        public static void test6()
        {
            //string json = "{\"result\":[{\"cid\":152919,\"configuration\":{\"ident\":\"865905020671073\"},\"device_type_id\":57,\"id\":361201,\"ident\":\"865905020671073\",\"messages_ttl\":31536000,\"name\":\"УАЗ а025мо\",\"phone\":\"\"},{\"cid\":152919,\"configuration\":{\"ident\":\"865905021233899\"},\"device_type_id\":57,\"id\":361202,\"ident\":\"865905021233899\",\"messages_ttl\":31536000,\"name\":\"Нива т907хв\",\"phone\":\"\"}]}";
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            string fileName = "json6.txt";
            //{"result":[{"cid":166848,"commands_ttl":86400,"configuration":null,"enabled":true,"id":11844,"messages_ttl":86400,"name":"QS","protocol_id":1,"uri":"193.193.165.37:30242"}]} 
            string json = Utils.ReadFile("json6i.txt");
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            foreach (var a in dobj["result"])
            {
                Utils.Print(fileName, a["cid"].ToString());
                Utils.Print(fileName, a["commands_ttl"].ToString());
                object o = a["configuration"];
                Utils.Print(fileName, a["configuration"].ToString());
                Utils.Print(fileName, a["enabled"].ToString());
                Utils.Print(fileName, a["id"].ToString());
                Utils.Print(fileName, a["messages_ttl"].ToString());
                Utils.Print(fileName, a["name"].ToString());
                Utils.Print(fileName, a["protocol_id"].ToString());
                Utils.Print(fileName, a["uri"].ToString());
            }
        }
        // 8. List device messages snapshots
        static void test8()
        {
            //string json = "{\"result\":[{\"cid\":152919,\"configuration\":{\"ident\":\"865905020671073\"},\"device_type_id\":57,\"id\":361201,\"ident\":\"865905020671073\",\"messages_ttl\":31536000,\"name\":\"УАЗ а025мо\",\"phone\":\"\"},{\"cid\":152919,\"configuration\":{\"ident\":\"865905021233899\"},\"device_type_id\":57,\"id\":361202,\"ident\":\"865905021233899\",\"messages_ttl\":31536000,\"name\":\"Нива т907хв\",\"phone\":\"\"}]}";
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            string fileName = "json8.txt";
            //{"result":[{"cid":166848,"commands_ttl":86400,"configuration":null,"enabled":true,"id":11844,"messages_ttl":86400,"name":"QS","protocol_id":1,"uri":"193.193.165.37:30242"}]} 
            string json = Utils.ReadFile("json8i.txt"); dynamic dobj = jsonSerializer.Deserialize<dynamic>(json);
            foreach (var a in dobj["result"])
            {
                Utils.Print(fileName, a["id"].ToString());
                //object o = a["snapshots"];
                foreach (var v in a["snapshots"])
                    Utils.Print(fileName, v.ToString());
            }
        }
        // Тестирование MQTT-клиента
        public static void ClientReceiveTest11(MqttClient.MqttMsgPublishedEventHandler peh, MqttClient.MqttMsgSubscribedEventHandler seh, MqttClient.MqttMsgPublishEventHandler peh2, MqttClient.MqttMsgUnsubscribedEventHandler ueh)
        {
            MqttClient client = new MqttClient("test.mosquitto.org");
            byte code = client.Connect(Guid.NewGuid().ToString()/*, token, ""*/);

            Console.WriteLine("Protocol " + client.ProtocolVersion);

            Console.WriteLine("Connected " + client.IsConnected);
            Console.WriteLine(client.Settings.Port + " " + client.Settings.SslPort);

            client.MqttMsgPublished += /*client_MqttMsgPublished*/peh;
            client.MqttMsgSubscribed += /*client_MqttMsgSubscribed*/seh;
            client.MqttMsgPublishReceived += /*client_MqttMsgPublishReceived*/peh2;
            client.MqttMsgUnsubscribed += /*client_MqttMsgUnsubscribed*/ueh;

            ushort msgId = client.Publish("/my_topic", // topic
                           Encoding.UTF8.GetBytes("MyMessageBody"), // message body
                           MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                           false); // retained
            ushort msgId2 = client.Subscribe(new string[] { "/topic_1", "/topic_2" },
                            new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                            MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            //client.Disconnect();
        }
    }
}