using System.Text.Json;

namespace Common.Feedback
{
    /// <summary>
    /// Out put class for rest api requests
    /// </summary>
    /// <typeparam name="T">Output Value of an action</typeparam>
    public class Feedback<T>
    {
        public Feedback(T value, MessageType messageType, string message = "", string[] parameters = null)
        {
            SetFeedBack(value, messageType, message, parameters);
        }
        public MessageType MessageType { get; private set; }
        public T Value { get; private set; }
        public string Message { get; private set; }
        public string[] Parameters { get; private set; }

        public void SetFeedBack(T value, MessageType messageType, string message = "", string[] parameters = null)
        {
            Value = value;
            MessageType = messageType;            
            Parameters = parameters;
            Message = message;
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);

        }
    }
}
