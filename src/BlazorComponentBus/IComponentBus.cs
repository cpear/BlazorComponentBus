using BlazorComponentBus.Subscribing;

namespace BlazorComponentBus;

public interface IComponentBus
{
    Task Publish<T>(T message, CancellationToken ct = default);
    void Subscribe<T>(AsyncComponentCallBack<MessageArgs> componentCallback);
    void Subscribe<T>(ComponentCallBack<MessageArgs> componentCallback);
    void UnSubscribe<T>(AsyncComponentCallBack<MessageArgs> componentCallBack);
    void UnSubscribe<T>(ComponentCallBack<MessageArgs> componentCallBack);
}
