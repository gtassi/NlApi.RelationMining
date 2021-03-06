using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

using static NlApi.RelationMining.JsonUtilities;

namespace NlApi.RelationMining {

  public class Neo4JWriter : IDisposable {
    public string ApiUrl { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public async Task Init() {
      HttpClient = new HttpClient();
      HttpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + Base64Encode(Username + ":" + Password));
    }

    public void Dispose() {
      if (HttpClient != null) {
        HttpClient.Dispose();
        HttpClient = null;
      }
    }

    HttpClient HttpClient;

    public async Task Write(string output) {
      var json = await ParseJson(output);

      foreach (var relation in json["data"]["relations"]) {
        var startId = await InsertOrRetrieve(relation["verb"], false);
        await WriteRelated(relation, startId);
      }
    }

    Regex TypesToRetrieve = new Regex("^[A-Z]+$");
    const int NullId = -1;

    async Task WriteRelated(JToken token, int startId) {
      if (token["related"] == null) {
        return;
      }

      foreach (var related in token["related"]) {
        var endId = await InsertOrRetrieve(related, true);
        var relation = related["relation"].Value<string>();

        await HttpClient.Post(ApiUrl, new {
          statements = new[] {
            new {
              statement = "MATCH (start) MATCH (end) WHERE ID(start) = $startId AND ID(end) = $endId MERGE (start)-[r: " + relation + "]->(end) RETURN ID(r)",
              parameters = new { startId, endId }
            }
          }
        });

        await WriteRelated(related, endId);
      }
    }

    async Task<int> InsertOrRetrieve(JToken token, bool reuse) {
      var lemma = token["lemma"].Value<string>();
      var type = token["type"].Value<string>();

      var result = await ParseJson(await HttpClient.Post(ApiUrl, new {
        statements = new[] {
          new {
            statement = (reuse && TypesToRetrieve.IsMatch(type) ? "MERGE" : "CREATE") + " (x: " + (type == "" ? "UNK" : type) + " { lemma: $lemma }) RETURN ID(x)",
            parameters = new { lemma }
          }
        }
      }));

      return result.SelectToken("$.results[0].data[0].row[0]").Value<int>();
    }

    public static string Base64Encode(string textToEncode) {
      var textAsBytes = Encoding.UTF8.GetBytes(textToEncode);
      return Convert.ToBase64String(textAsBytes);
    }
  }
}
