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

        [Fact]
        public async Task ShouldAllowGenericSubscribeToAndPublishAnEvent()
        {
          var bus = new ComponentBus();
          var publisher = new PublishingComponent(bus);
          var subscriber = new GenericSubscribingComponent(bus);

          await publisher.PublishEvent();

          Assert.Equal(1, subscriber.Count);
        }
    }

    public class SubscribingComponent
    {
        public int Count { get; set; }

        public SubscribingComponent(ComponentBus bus)
        {
            bus.Subscribe(typeof(TestEventMessage), (sender, e) => ReceiveMessage(e));
        }

        public void ReceiveMessage(MessageArgs args)
        {
            Count++;
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

    public class GenericSubscribingComponent
	  {
        public int Count { get; set; }

        public GenericSubscribingComponent(ComponentBus bus)
        {
            bus.Subscribe<TestEventMessage>((sender, e) => ReceiveMessage(e));
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
