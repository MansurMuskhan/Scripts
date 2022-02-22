using System.Threading.Tasks;

namespace Goons.Network
{
    public interface INetworkInteractions
    {
        string MetamaskToken { get; }
        Task<string> GetToken();
        Task<string> SentPostRequest(string url, string json, bool needAuthorize = true);
        Task<string> SentGetRequest(string url, bool needAuthorize = true);
    }
}