namespace Acreator.Models
{
    public class RepoResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T Content { get; set; }
        public int StatusCode { get; set; }
    }
}