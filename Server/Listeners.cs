using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Listener1:IListener<Event1>
    {
        public void Handle(Event1 receivedEvent)
        {
            Console.WriteLine("Listener 1" + receivedEvent.ToString());
        }
    }

    public class Listener2 : IListener<Event2>
    {
        public void Handle(Event2 receivedEvent)
        {
            Console.WriteLine("Listener2 " + receivedEvent.ToString());
        }
    }


    public class Listener3 : IListener<Event2>, IListener<Event1>
    {
        public void Handle(Event1 receivedEvent)
        {
            Console.WriteLine("Listener3 " + receivedEvent.ToString());
            
        }

        public void Handle(Event2 receivedEvent)
        {
            Console.WriteLine("Listener3 " + receivedEvent.ToString());
        }
    }
}
