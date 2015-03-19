using Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Messaging;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static EventAggregator e = new EventAggregator();
        static void Main(string[] args)
        {
            
            e.Subscribe<Event2>(new Listener2());
            e.Subscribe<Event1>(new Listener1());
            e.Subscribe<Event1>(new Listener3());
            e.Subscribe<Event2>(new Listener3());
            Task.Run(() => StartQueue<Event1>());
            Task.Run(() => StartQueue<Event2>());
            Console.ReadLine();
        }

        private static void StartQueue<T>() where T:Event
        {
            MessageQueue msgQ = new MessageQueue(string.Format(".\\Private$\\{0}", typeof(T).FullName));
            
                System.Type[] arrTypes = new System.Type[2];
                arrTypes[0] = typeof(T);
                arrTypes[1] = typeof(object);
                
                msgQ.Formatter = new XmlMessageFormatter(arrTypes);

            while(true)
            {
                T obj = msgQ.Receive().Body as T;
                e.Publish(obj);
            }
        }
    }
}
