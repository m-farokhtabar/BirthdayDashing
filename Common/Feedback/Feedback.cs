using System.Collections.Generic;
using System.Text.Json;

namespace Common.Feedback
{
    /// <summary>
    /// Out put class for rest api requests
    /// </summary>
    /// <typeparam name="T">Output Value of an action</typeparam>
    public class Feedback<T>
    {
        private Feedback(T value, MessageType messageType)
        {
            Value = value;
            MessageType = messageType;            
        }
        public Feedback(T value, MessageType messageType, string message, string parameter) : this(value, messageType)
        {
            Messages = new Dictionary<string, string[]>
            {
                { parameter, new string[] { message } }
            };
        }
        public Feedback(T value, MessageType messageType, string[] messages, string parameter) : this(value, messageType)
        {
            Messages = new Dictionary<string, string[]>
            {
                { parameter, messages }
            };
        }
        public Feedback(T value, MessageType messageType, Dictionary<string, string[]> messages) : this(value, messageType)
        {
            Messages = messages;
        }
        public MessageType MessageType { get; private set; }
        public T Value { get; private set; }
        public Dictionary<string, string[]> Messages { get; private set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
