namespace BlazorComponentBus
{
    public class MessageArgs : EventArgs
    {
        private readonly object _message;

        public MessageArgs(object message)
        {
            _message = message;
        }

        public TypeIExpect GetMessage<TypeIExpect>()
        {
            return (TypeIExpect)_message;
        }
    }
}