namespace Common.Exception
{
    public class ManualException : System.Exception
    {
        public ManualException(string message, ExceptionType type, string[] parameters = null) : base(message)
        {
            Type = type;
            Parameters = parameters;
        }

        public ExceptionType Type { get; set; }
        public string[] Parameters { get; set; }
    }
}
