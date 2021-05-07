namespace Common.Exception
{
    public class ManualException : System.Exception
    {
        public ManualException(string message, ExceptionType type, string parameter) : base(message)
        {
            Type = type;
            Parameter = parameter;
        }

        public ExceptionType Type { get; set; }
        public string Parameter { get; set; }
    }
}
