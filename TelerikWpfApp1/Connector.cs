using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;
using System.IO;

namespace TelerikWpfApp1
{
    public class Connector
    {
        public class Parameters
        {
            public string EntityName { get; set; }
            public string Action { get; set; }
            public string Data { get; set; }
        }

        private static Connector _instance;

        public static Connector GetInstance()
        {
            if (_instance == null)
                _instance = new Connector();
            return _instance;
        }

        public string SendRequest(Parameters parameters)
        {
            string url = "http://localhost/develop_database/hs/integration/endpoint";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";

            string json;
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                json = JsonSerializer.Serialize(parameters);
                writer.Write(json);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                json = reader.ReadToEnd();
            }
            return json;
        }
    }
}
