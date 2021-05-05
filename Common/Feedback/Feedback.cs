using Common.Extension;

namespace Common.Feedback
{
    /// <summary>
    /// Out put class for rest api requests
    /// </summary>
    /// <typeparam name="T">Output Value of an action</typeparam>
    public class Feedback<T>
    {
        public Feedback(FeedbackType status, MessageType messageType, T value, string message = "", string exceptionMessage = "")
        {
            SetFeedBack(status, messageType, value, message, exceptionMessage);
        }
        private FeedbackType _Status;
        public FeedbackType Status
        {
            get
            {
                return _Status;
            }
            private set
            {
                _Status = value;
                if (value != FeedbackType.OperationFailed)
                    Message = value.GetDisplayNameOfEnum();
            }
        }
        public MessageType MessageType { get; private set; }
        public T Value { get; private set; }
        public string Message { get; private set; }
        public string ExceptionMessage { get; private set; }

        public void SetFeedBack(FeedbackType status, MessageType messageType, T value, string message = "", string exceptionMessage = "")
        {
            Status = status;
            MessageType = messageType;
            Value = value;
            ExceptionMessage = exceptionMessage;
            if (Status == FeedbackType.OperationFailed)
                Message = message;
            Message = Message.Replace("{0}", message);
        }
    }
}
