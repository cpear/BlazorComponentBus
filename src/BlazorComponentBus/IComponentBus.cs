using BlazorComponentBus.Subscribing;

namespace BlazorComponentBus;

public interface IComponentBus
{
    Task Publish<T>(T message);
    Task Publish<T>(T message, CancellationToken ct);
    void Subscribe<T>(AsyncComponentCallBack<MessageArgs> componentCallback);
    void Subscribe<T>(ComponentCallBack<MessageArgs> componentCallback);
    void SubscribeTo<T>(AsyncComponentCallBack<T> componentCallback);
    void SubscribeTo<T>(ComponentCallBack<T> componentCallback);
    void UnSubscribe<T>(AsyncComponentCallBack<MessageArgs> componentCallBack);
    void UnSubscribe<T>(ComponentCallBack<MessageArgs> componentCallBack);
    void UnSubscribeFrom<T>(AsyncComponentCallBack<T> componentCallback);
    void UnSubscribeFrom<T>(ComponentCallBack<T> componentCallback);
}
