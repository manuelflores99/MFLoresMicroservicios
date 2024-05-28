namespace MicroservicioProductoAPI.DTO
{
    public class Result
    {
        public bool Success { get; set; } = true;
        public string ErrorMessage { get; set; }
        public Exception Error { get; set; }
        public object Data { get; set; }
    }
}
