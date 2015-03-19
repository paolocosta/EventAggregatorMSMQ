using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IEventAggregator
    {
        void Publish<TEvent>(TEvent message) where TEvent : Event;
        void Publish<TEvent>() where TEvent : Event, new();
        void Subscribe(IListener listener);
        void Unsubscribe(IListener listener);
        void Subscribe<TEvent>(IListener<TEvent> listener) where TEvent : Event;
        void Unsubscribe<TEvent>(IListener<TEvent> listener) where TEvent : Event;
    } 

    
}
