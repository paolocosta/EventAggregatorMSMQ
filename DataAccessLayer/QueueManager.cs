using Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataAccessLayer
{
    public class EventContainer
    {
        public string TypeName {get;set;}
        public string Event {get;set;}
    }

    public static class Extension
    {
        public static string SerializeObject<T>(this T toSerialize)
        {
            var result = "";
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                result =  textWriter.ToString();
            }
            return result;
        }

        public static T DeserializeObject<T>(this string toDeSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            var reader = new StringReader(toDeSerialize);
            var serializer = new XmlSerializer(typeof(T));
            var instance = (T)serializer.Deserialize(reader);
            return instance;
        }
    }

    public class QueueManager
    {
        
        public void SendMessage(Event @event)
        {
            EventContainer c = new EventContainer()
            {
                TypeName = @event.GetType().FullName,
                Event = Newtonsoft.Json.JsonConvert.SerializeObject(@event)
            };
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(c.GetType());
            using(SqlConnection conn = new SqlConnection(@"Data Source=.\sqlexpress;Initial Catalog=ServiceBroker;Integrated Security=True;Pooling=False"))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand("SendMessage"))
                {
                    cmd.Connection = conn;
                    cmd.Parameters.Add(new SqlParameter("@RequestMsg", c.SerializeObject()));
                    cmd.CommandType =System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        

        public Event ReceiveMessage(){
            Event result = null;
            using (SqlConnection conn = new SqlConnection(@"Data Source=.\sqlexpress;Initial Catalog=ServiceBroker;Integrated Security=True;Pooling=False"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("ReceiveMessage"))
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        var c = (reader[0] as string).DeserializeObject<EventContainer>();
                        result = (Newtonsoft.Json.JsonConvert.DeserializeObject(c.Event, Type.GetType(c.TypeName))) as Event;
                     }
                    reader.Close();
                }
            }
            return result;
        }
    }
}
