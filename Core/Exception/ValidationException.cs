namespace Core.Exception
{
    public class ValidationException : System.Exception
    {
        public int Code { get; set; }
        public ValidationException()
        {

        }

        public ValidationException(string message) : base(message)
        {

        }

        public ValidationException(int code, string message) : base(message)
        {
            Code = code;
        }
    }
}
