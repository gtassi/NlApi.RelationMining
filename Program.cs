using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace NlApi.RelationMining {
  class Program {
    static async Task Main(string[] args) {
      var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", true, true)
        .Build();

      using var reader = config.GetSection("reader").Get<FileSystemReader>();
      using var caller = config.GetSection("caller").Get<NlApiCaller>();
      using var writer = config.GetSection("writer").Get<Neo4JWriter>();

      await reader.Init();
      await caller.Init();
      await writer.Init();

      await foreach (var document in reader.Read()) {
        var apiResult = await caller.Call(document);
        await writer.Write(apiResult);
      }
    }
  }
}
