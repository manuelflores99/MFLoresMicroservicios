namespace MVC.Services.Interfaces
{
    public interface ITokenProvider
    {
        void Set(string token);
        string Get();
        void Clear();
    }
}
