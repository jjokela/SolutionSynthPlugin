namespace SolutionSynthPlugin.Services;

public static class EnvLoader
{
    private const string OpenAiApiKeyEnv = "OPENAI_API_KEY";
    private const string OpenAiModelEnv = "OPENAI_MODEL";
    private const string LogBaseFolderPath = "LOG_BASE_FOLDER_PATH";

    public static (string openaiApiKey, string openaiModel, string? logBaseFolder) LoadEnvVariables()
    {
        DotNetEnv.Env.TraversePath().Load();

        var openaiApiKey = Environment.GetEnvironmentVariable(OpenAiApiKeyEnv);
        var openaiModel = Environment.GetEnvironmentVariable(OpenAiModelEnv);
        var logBaseFolderpPath = Environment.GetEnvironmentVariable(LogBaseFolderPath);

        ArgumentNullException.ThrowIfNull(openaiApiKey,
            "OPENAI_API_KEY not found from .env file. Ensure that you have a .env file set up, and " +
            "that it contains OPENAI_API_KEY=XXXXX -row, where XXXXX is your api key");

        ArgumentNullException.ThrowIfNull(openaiModel,
            "OPENAI_MODEL not found from .env file. Ensure that you have a .env file set up, and " +
            "that it contains OPENAI_MODEL=XXXXX -row, where XXXXX is model name. For example: " +
            "OPENAI_MODEL=gpt-4-1106-preview");

        return (openaiApiKey, openaiModel, logBaseFolderpPath);
    }
}