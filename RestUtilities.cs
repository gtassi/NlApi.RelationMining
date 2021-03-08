using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace NlApi.RelationMining {
  public static class RestUtilities {
    public static async Task<string> Post(this HttpClient httpClient, string url, object request) {
      var httpResponseMessage = await httpClient.PostAsJsonAsync(url, request);
      return await httpResponseMessage.Content.ReadAsStringAsync();
    }
  }
}
