using System;
using System.Web.Script.Serialization;
using System.IO;
namespace flespi_simle
{
    public static class zu
    {
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
        public static void Print(string filename, string line, bool console = true)
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