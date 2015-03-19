using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class EventAggregator : IEventAggregator
    {
        private readonly object listenerLock = new object();
        protected readonly Dictionary<Type, List<IListener>> listeners = new Dictionary<Type, List<IListener>>();
        public EventAggregator()
        {
        }
        public virtual void Subscribe(IListener listener)
        {
            ForEachListenerInterfaceImplementedBy(listener, Subscribe);
        }
        public virtual void Unsubscribe(IListener listener)
        {
            ForEachListenerInterfaceImplementedBy(listener, Unsubscribe);
        }
        private static void ForEachListenerInterfaceImplementedBy(IListener listener, Action<Type, IListener> action)
        {
            var listenerTypeName = typeof(IListener).Name;
            foreach (var interfaceType in listener.GetType().GetInterfaces().Where(i => i.Name.StartsWith(listenerTypeName)))
            {
                Type typeOfEvent = GetEventType(interfaceType);
                if (typeOfEvent != null)
                {
                    action(typeOfEvent, listener);
                }
            }
        }
        private static Type GetEventType(Type type)
        {
            if (type.GetGenericArguments().Count() > 0)
            {
                return type.GetGenericArguments()[0];
            }
            return null;
        }
        public virtual void Subscribe<TEvent>(IListener<TEvent> listener) where TEvent : Event
        {
            Subscribe(typeof(TEvent), listener);
        }
        protected virtual void Subscribe(Type typeOfEvent, IListener listener)
        {
            lock (listenerLock)
            {
                if (!listeners.ContainsKey(typeOfEvent))
                {
                    listeners.Add(typeOfEvent, new List<IListener>());
                }
                if (listeners[typeOfEvent].Contains(listener))
                {
                    throw new InvalidOperationException("You're not supposed to register to the same event twice");
                }
                listeners[typeOfEvent].Add(listener);
            }
        }
        public virtual void Unsubscribe<TEvent>(IListener<TEvent> listener) where TEvent : Event
        {
            Unsubscribe(typeof(TEvent), listener);
        }
        protected virtual void Unsubscribe(Type typeOfEvent, IListener listener)
        {
            lock (listenerLock)
            {
                if (listeners.ContainsKey(typeOfEvent))
                {
                    listeners[typeOfEvent].Remove(listener);
                }
            }
        }
        public virtual void Publish<TEvent>(TEvent message) where TEvent : Event
        {
            var typeOfEvent = typeof(TEvent);
            lock (listenerLock)
            {
                if (!listeners.ContainsKey(typeOfEvent)) return;
                foreach (var listener in listeners[typeOfEvent])
                {
                    var typedReference = (IListener<TEvent>)listener;
                    typedReference.Handle(message);
                    //dispatcher.BeginInvoke(() => typedReference.Handle(message));
                }
            }
        }
        public virtual void Publish<TEvent>() where TEvent : Event, new()
        {
            Publish(new TEvent());
        }
    } 
}
