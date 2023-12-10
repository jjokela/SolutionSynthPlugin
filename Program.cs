using SolutionSynthPlugin;

var runner = new SolutionSynthRunner();
Console.WriteLine("Starting process...");
await runner.RunSolutionSynth();
Console.WriteLine("Process ended.");
