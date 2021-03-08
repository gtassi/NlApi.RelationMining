using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NlApi.RelationMining {
  public class NlApiCaller : IDisposable {
    public string TokenUrl { get; set; }
    public string ApiUrl { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    HttpClient HttpClient;

    public async Task Init() {
      HttpClient = new HttpClient();
      var token = await HttpClient.Post(TokenUrl, new {
        username = Username,
        password = Password
      });
      HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
    }

    public void Dispose() {
      if (HttpClient != null) {
        HttpClient.Dispose();
        HttpClient = null;
      }
    }

    public async Task<string> Call(string document) {
      return await HttpClient.Post(ApiUrl, new {
        document = new {
          text = document
        }
      });
    }
  }
}
