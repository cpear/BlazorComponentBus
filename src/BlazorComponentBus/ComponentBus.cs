using BlazorComponentBus.Subscribing;

namespace BlazorComponentBus;

public class ComponentBus : IComponentBus
{
    private readonly List<KeyValuePair<Type, object>> _registeredComponents = new();


    public void Subscribe<T>(ComponentCallBack<MessageArgs> componentCallback) =>
        Subscribe<T>(componentCallback as object);

    public void Subscribe<T>(AsyncComponentCallBack<MessageArgs> componentCallback) =>
        Subscribe<T>(componentCallback as object);

    private void Subscribe<T>(object componentCallback)
    {
        /* Unsubscribe first just in case the same component subscribes to the same callback twice.
        This Prevents multiple callbacks to the same component. */
        UnSubscribe<T>(componentCallback);

        _registeredComponents.Add(new KeyValuePair<Type, object>(typeof(T), componentCallback));
    }

    public void UnSubscribe<T>(ComponentCallBack<MessageArgs> componentCallBack) =>
        UnSubscribe<T>(componentCallBack as object);

    public void UnSubscribe<T>(AsyncComponentCallBack<MessageArgs> componentCallBack) =>
        UnSubscribe<T>(componentCallBack as object);

    private void UnSubscribe<T>(object componentCallBack)
    {
        _registeredComponents.Remove(new KeyValuePair<Type, object>(typeof(T), componentCallBack));
    }

    public async Task Publish<T>(T message, CancellationToken ct = default)
    {
        var messageType = typeof(T);

        var args = new MessageArgs(message!);

        var subscribers = _registeredComponents.ToLookup(item => item.Key);

        //Look for subscribers of this message type
        //Call the subscriber and pass the message along
        foreach (var subscriber in subscribers[messageType].Select(_ => _.Value))
        {
            if (subscriber is ComponentCallBack<MessageArgs> syncCallback)
            {
                await Task.Run(() => syncCallback.Invoke(args), ct);
            }
            else if (subscriber is AsyncComponentCallBack<MessageArgs> asyncCallback)
            {
                await Task.Run(() => asyncCallback.Invoke(args, ct), ct);
            }
        }

    }
}
