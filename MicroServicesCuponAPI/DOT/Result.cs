namespace MicroServicesCuponAPI.DOT
{
    public class Result
    {
        public bool Success { get; set; } = true;
        public string ErrorTxt { get; set; }
        public Exception Error { get; set; }
        public object Data { get; set; }
        public List<object> DataList { get; set; }
    }
}
