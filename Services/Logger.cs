namespace SolutionSynthPlugin.Services;

public class Logger
{
    private string _folderPath = string.Empty;
    private const string DefaultFolderBasePath = "files";

    public Logger(string? folderBasePath = null)
    {
        GetFolderPath(folderBasePath ?? DefaultFolderBasePath);
    }

    private void GetFolderPath(string folderBasePath)
    {
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        _folderPath = Path.Combine(folderBasePath, timestamp);
    }

    public Task WriteToFile(string fileName, string? content)
    {
        EnsureDirectoryExists(_folderPath);
        var path = Path.Combine(_folderPath, fileName);

        Console.WriteLine($"Writing results to: {path}");
        return File.WriteAllTextAsync(path, content);
    }

    public async Task WriteProposalsToFile(IEnumerable<string?> proposals, string proposalFileName)
    {
        var index = 1;
        foreach (var proposal in proposals)
        {
            var fileName = $"{index++}_{proposalFileName}";
            await WriteToFile(fileName, proposal);
        }
    }

    public void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}