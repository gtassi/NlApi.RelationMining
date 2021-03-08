using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NlApi.RelationMining {
  public class FileSystemReader : IDisposable {
    public string InputDirectory { get; set; }
    public string DocSearchPattern { get; set; }

    public async Task Init() {
    }

    public void Dispose() {
    }

    public async IAsyncEnumerable<string> Read() {
      foreach (var filePath in GetAllFilePaths(InputDirectory, DocSearchPattern)) {
        yield return await File.ReadAllTextAsync(filePath);
      }
    }

    public static IEnumerable<string> GetAllFilePaths(string directory, string docSearchPattern) {
      foreach (var filePath in Directory.EnumerateFiles(directory, docSearchPattern)) {
        yield return filePath;
      }

      foreach (var childDirectory in Directory.EnumerateDirectories(directory)) {
        foreach (var filePath in GetAllFilePaths(childDirectory, docSearchPattern)) {
          yield return filePath;
        }
      }
    }
  }
}
