using System;
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
        }
        
        [Fact]
        public async Task ShouldNotFailIfPublishWithNoSubscribers()
        {
            var bus = new ComponentBus();
            var publisher = new PublishingComponent(bus);
            
            //There is no subscriber but we are going to try and publish an event
            await publisher.PublishTestMessageEvent();
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

            await publisher.PublishTestMessageEvent();

            Assert.Equal(1, subscriber.Count);
        }
    }





    public class PublishingComponent
    {
        private readonly ComponentBus _bus;

        public PublishingComponent(ComponentBus bus)
        {
            _bus = bus;
        }

        public async Task PublishTestMessageEvent()
        {
            await _bus.Publish(new TestEventMessage());
        }
        
        public async Task PublishAnotherTestEventMessage()
        {
            await _bus.Publish(new AnotherTestEventMessage());
        }
    }

    public class SubscribingComponent
    {
        public int Count { get; set; }

        public void SubscribeThisComponent<T>(ComponentBus bus)
        {
            bus.Subscribe<T>(ReceiveMessage);
        }

        public void UnsubscribeThisComponent<T>(ComponentBus bus)
        {
            bus.UnSubscribe<T>(ReceiveMessage);
        }
        
        public void ReceiveMessage(MessageArgs args)
        {
            Count++;
        }
    }
    
    public class SecondSubscribingComponent
    {
        public int Count { get; set; }

        public void SubscribeThisComponent<T>(ComponentBus bus)
        {
            bus.Subscribe<T>(ReceiveMessage);
        }

        public void UnsubscribeThisComponent<T>(ComponentBus bus)
        {
            bus.UnSubscribe<T>(ReceiveMessage);
        }
        
        public void ReceiveMessage(MessageArgs args)
        {
            Count++;
        }
    }
    
    public class TestEventMessage
    {

    }
    
    public class AnotherTestEventMessage
    {

    }
}
