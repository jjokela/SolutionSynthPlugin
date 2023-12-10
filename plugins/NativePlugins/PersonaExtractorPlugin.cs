using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace SolutionSynthPlugin.plugins.NativePlugins;

public sealed class PersonaExtractorPlugin
{
    [KernelFunction, Description("Extracts personas from input")]
    public static string[] ExtractPersonas([Description("Text containing personas, separated by START PERSONA and END PERSONA")] string? input)
    {
        if (input == null) return Array.Empty<string>();

        var personas = new List<string>();
        const string startTag = "START PERSONA";
        const string endTag = "END PERSONA";

        var startIndex = 0;

        while ((startIndex = input.IndexOf(startTag, startIndex, StringComparison.Ordinal)) != -1)
        {
            var endIndex = input.IndexOf(endTag, startIndex, StringComparison.Ordinal);

            if (endIndex != -1)
            {
                var start = startIndex + startTag.Length;
                var persona = input[start..endIndex].Trim();

                personas.Add(persona);

                startIndex = endIndex + endTag.Length;
            }
            else
            {
                break;
            }
        }

        return personas.ToArray();
    }
}