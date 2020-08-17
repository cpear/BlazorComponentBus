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
            var subscriber = new SubscribingComponent(bus);

            await publisher.PublishEvent();

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

        public async Task PublishEvent()
        {
            await _bus.Publish(new TestEventMessage());
        }
    }

    public class SubscribingComponent
    {
        public int Count { get; set; }

        public SubscribingComponent(ComponentBus bus)
        {
            bus.Subscribe<TestEventMessage>(ReceiveMessage);
        }

        public void ReceiveMessage(MessageArgs args)
        {
            Count++;
        }
    }

    public class TestEventMessage
    {

    }
}
