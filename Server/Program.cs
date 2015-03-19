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
            DataAccessLayer.QueueManager q = new DataAccessLayer.QueueManager();
            while(true)
            {
                T obj = q.ReceiveMessage() as T;
                e.Publish(obj);
            }
        }
    }
}
