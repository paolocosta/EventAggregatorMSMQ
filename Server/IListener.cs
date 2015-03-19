using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IListener { }
    public interface IListener<TEvent> : IListener
    where TEvent : Event
    {
        void Handle(TEvent receivedEvent);
    } 
}
