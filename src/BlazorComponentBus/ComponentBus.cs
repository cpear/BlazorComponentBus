using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorComponentBus.Subscribing;

namespace BlazorComponentBus
{
    public class ComponentBus
    {
        private List<KeyValuePair<Type, ComponentCallBack<MessageArgs>>> _registeredComponents = 
            new List<KeyValuePair<Type, ComponentCallBack<MessageArgs>>>();


        public void Subscribe<T>(ComponentCallBack<MessageArgs> componentCallBack)
        {
            /* Unsubscribe first just in case the same component subscribes to the same callback twice.
            This Prevents multiple callbacks to the same component. */
            UnSubscribe<T>(componentCallBack);
            
            _registeredComponents.Add(new KeyValuePair<Type, ComponentCallBack<MessageArgs>>(typeof(T), componentCallBack));
        }
        
        public void UnSubscribe<T>(ComponentCallBack<MessageArgs> componentCallBack)
        {
            _registeredComponents.Remove(new KeyValuePair<Type, ComponentCallBack<MessageArgs>>(typeof(T), componentCallBack));
        }
        
        public async Task Publish<T>(T message)
        {
            var messageType = typeof(T);

            var args = new MessageArgs(message);

            var subscribers = _registeredComponents.ToLookup(item => item.Key);

            //Look for subscribers of this message type
            //Call the subscriber and pass the message along
            foreach (var subscriber in subscribers[messageType])
            {
                await Task.Run(() => subscriber.Value.Invoke(args));
            }
            
        } 
    }
}