using BlazorComponentBus.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BlazorComponentBus.UnitTests
{
    public class ComponentBusTests
    {

        [Fact]
        public async Task ShouldSubscribeToAndPublishAnEvent()
        {
            var bus = new ComponentBus();
            var publisher = new PublishingComponent(bus);
            var subscriber = new SubscribingComponent();

            subscriber.SubscribeThisComponent<TestEventMessage>(bus);

            await publisher.PublishTestMessageEvent();

            Assert.Equal(1, subscriber.Count);
        }

        [Fact]
        public async Task ShouldUnsubscribe()
        {
            var bus = new ComponentBus();
            var publisher = new PublishingComponent(bus);
            var subscriber = new SubscribingComponent();

            subscriber.SubscribeThisComponent<TestEventMessage>(bus);

            subscriber.UnsubscribeThisComponent<TestEventMessage>(bus);

            await publisher.PublishTestMessageEvent();

            Assert.Equal(0, subscriber.Count);
        }

        [Fact]
        public void ShouldNotFailIfNoSubscriberFoundWhenUnsubscribing()
        {
            var bus = new ComponentBus();
            var subscriber = new SubscribingComponent();

            //There is no subscriber but we are going to try an unsubscribe anyway
            subscriber.UnsubscribeThisComponent<TestEventMessage>(bus);

            Assert.True(true);
        }

        [Fact]
        public async Task ShouldNotFailIfPublishWithNoSubscribers()
        {
            var bus = new ComponentBus();
            var publisher = new PublishingComponent(bus);

            //There is no subscriber but we are going to try and publish an event
            await publisher.PublishTestMessageEvent();

            Assert.True(true);
        }

        [Fact]
        public async Task ShouldOnlyUnsubscribeOneComponentAndNotAll()
        {
            var bus = new ComponentBus();
            var publisher = new PublishingComponent(bus);
            var subscriber = new SubscribingComponent(); //Different Type
            var secondSubscriber = new SecondSubscribingComponent(); //Different Type

            subscriber.SubscribeThisComponent<TestEventMessage>(bus);
            secondSubscriber.SubscribeThisComponent<TestEventMessage>(bus);

            subscriber.UnsubscribeThisComponent<TestEventMessage>(bus);

            await publisher.PublishTestMessageEvent();

            Assert.Equal(0, subscriber.Count);
            Assert.Equal(1, secondSubscriber.Count);
        }

        [Fact]
        public async Task ShouldOnlyUnsubscribeOneComponentAndNotAllMultiInstance()
        {
            var bus = new ComponentBus();
            var publisher = new PublishingComponent(bus);
            var subscriber = new SubscribingComponent(); //Same Type
            var secondSubscriber = new SubscribingComponent(); //Same Type

            subscriber.SubscribeThisComponent<TestEventMessage>(bus);
            secondSubscriber.SubscribeThisComponent<TestEventMessage>(bus);

            subscriber.UnsubscribeThisComponent<TestEventMessage>(bus);

            await publisher.PublishTestMessageEvent();

            Assert.Equal(0, subscriber.Count);
            Assert.Equal(1, secondSubscriber.Count);
        }

        [Fact]
        public async Task ShouldOnlyUnsubscribeOneEventType()
        {
            var bus = new ComponentBus();
            var publisher = new PublishingComponent(bus);
            var subscriber = new SubscribingComponent();

            subscriber.SubscribeThisComponent<TestEventMessage>(bus);
            subscriber.SubscribeThisComponent<AnotherTestEventMessage>(bus);

            subscriber.UnsubscribeThisComponent<TestEventMessage>(bus);

            await publisher.PublishTestMessageEvent();
            await publisher.PublishAnotherTestEventMessage();

            Assert.Equal(1, subscriber.Count);
        }

        [Fact]
        public async Task ShouldSubscribeTo2Events()
        {
            var bus = new ComponentBus();
            var publisher = new PublishingComponent(bus);
            var subscriber = new SubscribingComponent();

            subscriber.SubscribeThisComponent<TestEventMessage>(bus);
            subscriber.SubscribeThisComponent<AnotherTestEventMessage>(bus);


            await publisher.PublishTestMessageEvent();
            await publisher.PublishAnotherTestEventMessage();

            Assert.Equal(2, subscriber.Count);
        }

        [Fact]
        public async Task MultiSubscribeToSameMessageShouldOnlyGetOneResult()
        {
            var bus = new ComponentBus();
            var publisher = new PublishingComponent(bus);
            var subscriber = new SubscribingComponent();

            subscriber.SubscribeThisComponent<TestEventMessage>(bus);
            subscriber.SubscribeThisComponent<TestEventMessage>(bus);
            subscriber.SubscribeThisComponent<TestEventMessage>(bus);
            subscriber.SubscribeThisComponent<TestEventMessage>(bus);
            subscriber.SubscribeThisComponent<TestEventMessage>(bus);
            subscriber.SubscribeThisComponent<TestEventMessage>(bus);
            subscriber.SubscribeThisComponent<TestEventMessage>(bus);
            subscriber.SubscribeThisComponent<TestEventMessage>(bus);

            await publisher.PublishTestMessageEvent();

            Assert.Equal(1, subscriber.Count);
        }

        [Fact]
        public async Task AsyncSubscribeShouldSubscribeToAndPublishAnEvent()
        {
            var bus = new ComponentBus();
            var publisher = new PublishingComponent(bus);
            var subscriber = new AsyncSubscribingComponent();

            subscriber.SubscribeThisComponent<TestEventMessage>(bus);

            await publisher.PublishTestMessageEvent();

            Assert.Equal(1, subscriber.Count);
        }

        [Fact]
        public async Task AsyncSubscribeShouldUnsubscribe()
        {
            var bus = new ComponentBus();
            var publisher = new PublishingComponent(bus);
            var subscriber = new AsyncSubscribingComponent();

            subscriber.SubscribeThisComponent<TestEventMessage>(bus);
            subscriber.UnsubscribeThisComponent<TestEventMessage>(bus);

            await publisher.PublishTestMessageEvent();

            Assert.Equal(0, subscriber.Count);
        }

        [Fact]
        public async Task AsyncSubscribeMultiSubscribeToSameMessageShouldOnlyGetOneResult()
        {
            var bus = new ComponentBus();
            var publisher = new PublishingComponent(bus);
            var subscriber = new AsyncSubscribingComponent();

            subscriber.SubscribeThisComponent<TestEventMessage>(bus);
            subscriber.SubscribeThisComponent<TestEventMessage>(bus);
            subscriber.SubscribeThisComponent<TestEventMessage>(bus);
            subscriber.SubscribeThisComponent<TestEventMessage>(bus);
            subscriber.SubscribeThisComponent<TestEventMessage>(bus);
            subscriber.SubscribeThisComponent<TestEventMessage>(bus);
            subscriber.SubscribeThisComponent<TestEventMessage>(bus);
            subscriber.SubscribeThisComponent<TestEventMessage>(bus);

            await publisher.PublishTestMessageEvent();

            Assert.Equal(1, subscriber.Count);
        }


        [Fact]
        public async Task ShouldSubscribeWithExtensionMethod()
        {
            var bus = new ComponentBus();
            var publisher = new PublishingComponent(bus);
            var subscriber = new SubscribingComponent();

            subscriber.SubscribeToThisComponent<TestEventMessage>(bus);

            await publisher.PublishTestMessageEvent();

            Assert.Equal(1, subscriber.Count);
        }

        [Fact]
        public async Task ShouldSubscribeAsyncWithExtensionMethod()
        {

            var bus = new ComponentBus();
            var publisher = new PublishingComponent(bus);
            var subscriber = new AsyncSubscribingComponent();

            subscriber.SubscribeToThisComponent<TestEventMessage>(bus);

            await publisher.PublishTestMessageEvent();

            Assert.Equal(1, subscriber.Count);
        }


        [Fact]
        public async Task ShouldUnSubscribeWithExtensionMethod()
        {
            var bus = new ComponentBus();
            var publisher = new PublishingComponent(bus);
            var subscriber = new SubscribingComponent();

            subscriber.SubscribeToThisComponent<TestEventMessage>(bus);
            subscriber.UnsubscribeFromThisComponent<TestEventMessage>(bus);

            await publisher.PublishTestMessageEvent();

            Assert.Equal(0, subscriber.Count);
        }

        [Fact]
        public async Task ShouldUnSubscribeAsyncWithExtensionMethod()
        {

            var bus = new ComponentBus();
            var publisher = new PublishingComponent(bus);
            var subscriber = new AsyncSubscribingComponent();

            subscriber.SubscribeToThisComponent<TestEventMessage>(bus);
            subscriber.UnsubscribeFromThisComponent<TestEventMessage>(bus);

            await publisher.PublishTestMessageEvent();

            Assert.Equal(0, subscriber.Count);
        }


    }





    public class PublishingComponent
    {
        private readonly IComponentBus _bus;

        public PublishingComponent(IComponentBus bus) => _bus = bus;

        public Task PublishTestMessageEvent() => _bus.Publish(new TestEventMessage());
        public Task PublishAnotherTestEventMessage() => _bus.Publish(new AnotherTestEventMessage());
    }

    public class SubscribingComponent
    {
        public int Count { get; set; }

        public void SubscribeThisComponent<T>(IComponentBus bus) => bus.Subscribe<T>(ReceiveMessage);
        public void SubscribeToThisComponent<T>(IComponentBus bus) => bus.SubscribeTo<T>(ReceiveMessage);
        public void UnsubscribeThisComponent<T>(IComponentBus bus) => bus.UnSubscribe<T>(ReceiveMessage);
        public void UnsubscribeFromThisComponent<T>(IComponentBus bus) => bus.UnSubscribeFrom<T>(ReceiveMessage);

        public void ReceiveMessage(MessageArgs args) => Count++;
        public void ReceiveMessage<T>(T message) => Count++;

    }

    public class AsyncSubscribingComponent
    {
        public int Count { get; set; }

        public void SubscribeThisComponent<T>(ComponentBus bus) => bus.Subscribe<T>(ReceiveMessageAsync);
        public void SubscribeToThisComponent<T>(IComponentBus bus) => bus.SubscribeTo<T>(ReceiveMessageAsync);
        public void UnsubscribeThisComponent<T>(ComponentBus bus) => bus.UnSubscribe<T>(ReceiveMessageAsync);
        public void UnsubscribeFromThisComponent<T>(IComponentBus bus) => bus.UnSubscribeFrom<T>(ReceiveMessageAsync);

        public Task ReceiveMessageAsync(MessageArgs args, CancellationToken ct)
        {
            Count++;
            return Task.CompletedTask;
        }

        public Task ReceiveMessageAsync<T>(T message, CancellationToken ct)
        {
            Count++;
            return Task.CompletedTask;
        }
    }

    public class SecondSubscribingComponent
    {
        public int Count { get; set; }

        public void SubscribeThisComponent<T>(ComponentBus bus) => bus.Subscribe<T>(ReceiveMessage);
        public void UnsubscribeThisComponent<T>(ComponentBus bus) => bus.UnSubscribe<T>(ReceiveMessage);
        public void ReceiveMessage(MessageArgs args) => Count++;
    }

    public sealed record TestEventMessage();
    public sealed record AnotherTestEventMessage();
}
