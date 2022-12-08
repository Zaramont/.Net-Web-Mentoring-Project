using System.Threading.Tasks;

namespace MyCatalogSite.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}