using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NlApi.RelationMining {
  static class JsonUtilities {
    public static Task<JObject> ParseJson(string input) {
      using var stringReader = new StringReader(input);
      using var jsonTextReader = new JsonTextReader(stringReader);
      return JObject.LoadAsync(jsonTextReader);
    }
  }
}
