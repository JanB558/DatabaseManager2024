using ASPWebApp.Models;

namespace ASPWebApp.Services
{
    public interface IAPIService<T>
    {
        Task<HttpResponseMessage?> Get(string endpoint);
        Task<HttpResponseMessage?> Get(string enpoint, int id);
        Task<HttpResponseMessage?> Post(string endpoint, T model);
        Task<HttpResponseMessage?> Put(string endpoint, T model);
        Task<HttpResponseMessage?> Delete(string endpoint, int id);
    }
}
