namespace SREES.Common.Models
{
    public class ResponsePackage<T>
    {
        public string? Message { get; set; }
        public T? Data { get; set; }

        public ResponsePackage()
        {
            Message = string.Empty;
            //Data = null;
        }

        public ResponsePackage(T? data, string message)
        {
            Data = data;
            Message = message;
        }

        public ResponsePackage(string message)
        {
            Message = message;
        }
    }
}
