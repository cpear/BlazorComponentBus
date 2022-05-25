namespace BlazorComponentBus.Subscribing;

public delegate void ComponentCallBack<in TMessage>(TMessage message);
public delegate Task AsyncComponentCallBack<in TMessage>(TMessage message, CancellationToken ct);