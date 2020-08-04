using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorComponentBus
{
    public class ComponentBus
    {
        private List<KeyValuePair<Type, EventHandler<MessageArgs>>> _componentRegistrants = 
            new List<KeyValuePair<Type, EventHandler<MessageArgs>>>();

        /// <summary>
        /// The mechanism that allows components to subscribe to a specific message type
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="callBack"></param>
        public void Subscribe(Type messageType, EventHandler<MessageArgs> callBack)
        {
            _componentRegistrants.Add(new KeyValuePair<Type, EventHandler<MessageArgs>>(messageType, callBack));
        }

        public void Subscribe<T>(EventHandler<MessageArgs> callBack)
        {
            _componentRegistrants.Add(new KeyValuePair<Type, EventHandler<MessageArgs>>(typeof(T), callBack));
        }

        public async Task Publish<T>(T message)
        {
            var messageType = typeof(T);

            var args = new MessageArgs(message);

            var subscribers = _componentRegistrants.ToLookup(item => item.Key);

            //Look for subscribers of this message type
            //Call the subscriber and pass the message along
            foreach (var subscriber in subscribers[messageType])
            {
                await Task.Run(() => subscriber.Value.Invoke(this, args));
            }

        } 
    }
}