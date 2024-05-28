namespace MicroserviceAuth.DTO
{
    public class Result
    {
        public bool Success { get; set; } = true;
        public Exception Error { get; set; }
        public string MessageError { get; set; }
        public object Data { get; set; }

    }
}
