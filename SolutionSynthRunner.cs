using Microsoft.SemanticKernel;
using Polly;
using SolutionSynthPlugin.plugins.NativePlugins;
using SolutionSynthPlugin.Services;

namespace SolutionSynthPlugin;

public class SolutionSynthRunner
{
    private const string PersonasFileName = "personas.txt";
    private const string ProposalFileName = "proposal.txt";
    private const string DecisionFileName = "decision.txt";
    private const int NumberOfPersonas = 3;

    private const string ProposalPrefix = "*** PROPOSAL ";

    private static ResiliencePipeline? _pipeline;
    private readonly Logger _logger;
    private readonly Kernel _kernel;

    public SolutionSynthRunner()
    {
        var (openaiApiKey, openaiModel, logBaseFolder) = EnvLoader.LoadEnvVariables();

        _logger = new Logger(logBaseFolder!);

        _pipeline = Settings.PolicySettings.GetResiliencePipeline();

        _kernel = new KernelBuilder()
            .AddOpenAIChatCompletion(openaiModel, openaiApiKey)
            .Build();
    }

    public async Task RunSolutionSynth()
    {
        var pluginsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "plugins", "SolutionSynthPlugin");
        var personaFunctions = _kernel.ImportPluginFromPromptDirectory(pluginsDirectory);
        var personaExtractorPlugin = _kernel.ImportPluginFromType<PersonaExtractorPlugin>();

        var scenarioDescription = ReadScenario();

        var personaResult = await _pipeline!.ExecuteAsync(
            async token => await _kernel.InvokeAsync(personaFunctions["PersonaCreator"],
                new KernelArguments(new Dictionary<string, object?>
                {
                    { "input", scenarioDescription },
                    { "numberOfPersonas", NumberOfPersonas.ToString() }
                }), token),
            CancellationToken.None);

        await _logger.WriteToFile(PersonasFileName, personaResult.GetValue<string>());

        var personas = await _kernel.InvokeAsync<string[]>(personaExtractorPlugin["ExtractPersonas"], new(personaResult.GetValue<string>()));

        var proposals = await GetProposals(personas, _kernel, personaFunctions, scenarioDescription);

        await _logger.WriteProposalsToFile(proposals, ProposalFileName);

        var decisionResult = await _pipeline.ExecuteAsync(
            async token => await _kernel.InvokeAsync(personaFunctions["DecisionMaker"],
                new KernelArguments(new Dictionary<string, object?>
                {
                    {"scenario", scenarioDescription},
                    {"proposals", string.Join(Environment.NewLine, proposals)}
                }), token),
            CancellationToken.None);

        await _logger.WriteToFile(DecisionFileName, decisionResult.GetValue<string>());

        Console.WriteLine(decisionResult);
    }

    private async Task<List<string?>> GetProposals(string[]? personas, Kernel kernel, IKernelPlugin personaFunctions, string scenarioDescription)
    {
        if (personas is null || personas.Length == 0)
        {
            return [];
        }

        var proposals = new List<string?>();

        foreach (var persona in personas)
        {
            var proposal = await GenerateProposalForPersona(kernel, personaFunctions, scenarioDescription, persona);
            proposals.Add(proposal);
        }

        return proposals;
    }

    private async Task<string?> GenerateProposalForPersona(Kernel kernel, IKernelPlugin personaFunctions, string scenarioDescription, string persona)
    {
        var proposalResult = await _pipeline!.ExecuteAsync(
            async token => await kernel.InvokeAsync(personaFunctions["Proposer"],
                new KernelArguments(new Dictionary<string, object?>
                {
                    { "scenario", scenarioDescription },
                    { "persona", persona }
                }), token),
            CancellationToken.None);


        return $"{ProposalPrefix}{proposalResult.GetValue<string>()}";
    }

    private string ReadScenario()
    {
        try
        {
            return File.ReadAllText("scenario.txt");
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine("File not found, ensure that scenario.txt is present: " + ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
            throw;
        }
    }
}